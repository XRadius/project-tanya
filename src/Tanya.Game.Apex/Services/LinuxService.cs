using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tanya.Driver.Linux;
using Tanya.Driver.Linux.Models;
using Tanya.Game.Apex.Core;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Services
{
    public class LinuxService : IDisposable, IHostedService
    {
        private readonly Config _config;
        private readonly CancellationTokenSource _cts;
        private readonly DataService _dataService;
        private readonly Linux _linux;
        private readonly ILogger<LinuxService> _logger;
        private readonly ConcurrentDictionary<int, Runner> _runners;
        private readonly IServiceProvider _serviceProvider;

        #region Constructors

        public LinuxService(Config config, DataService dataService, Linux linux, ILogger<LinuxService> logger, IServiceProvider serviceProvider)
        {
            _config = config;
            _cts = new CancellationTokenSource();
            _dataService = dataService;
            _linux = linux;
            _logger = logger;
            _runners = new ConcurrentDictionary<int, Runner>();
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Methods

        private void Cleanup()
        {
            foreach (var (pid, _) in _runners)
            {
                if (_runners.TryRemove(pid, out var runner))
                {
                    runner.Dispose();
                }
            }
        }

        private async Task ProcessAsync()
        {
            IOffsets? previousOffsets = null;

            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    var knownSet = new HashSet<int>();
                    var offsets = await _dataService.OffsetsAsync().ConfigureAwait(false);

                    if (previousOffsets == null || offsets != previousOffsets)
                    {
                        Cleanup();
                        previousOffsets = offsets;
                    }

                    await foreach (var process in _linux.ProcessesAsync().ConfigureAwait(false))
                    {
                        if (_runners.ContainsKey(process.Pid))
                        {
                            knownSet.Add(process.Pid);
                            continue;
                        }

                        if (process.Command.EndsWith("r5apex.exe", StringComparison.InvariantCultureIgnoreCase))
                        {
                            await foreach (var map in _linux.MapsAsync(process.Pid).ConfigureAwait(false))
                            {
                                if (map.Pathname.ToLower().EndsWith("r5apex.exe")
                                    || (map.Perms == MapEntryPermissions.Read && map.Pathname.StartsWith("/memfd"))
                                    || map.Start == 0x140000000)
                                {
                                    _runners.GetOrAdd(process.Pid, _ => Start(offsets, process.Pid, map.Start));
                                    knownSet.Add(process.Pid);
                                    break;
                                }
                            }
                        }
                    }

                    foreach (var (pid, _) in _runners)
                    {
                        if (!knownSet.Contains(pid) && _runners.TryRemove(pid, out var runner))
                        {
                            runner.Dispose();
                        }
                    }

                    await Task.Delay(_config.ProcessRefresh, _cts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    break;
                }
            }
        }

        private Runner Start(IOffsets offsets, int pid, ulong address)
        {
            using var scope = _serviceProvider.CreateScope();
            var driver = new LinuxDriver(pid);
            var features = scope.ServiceProvider.GetRequiredService<IEnumerable<IFeature>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<State>>();
            var state = new State(driver, logger, offsets, address);
            return Runner.Create(_config, features, state);
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IHostedService

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return Task.CompletedTask;
            Task.Factory.StartNew(ProcessAsync, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }

        #endregion
    }
}