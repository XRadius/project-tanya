using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tanya.Driver;
using Tanya.Game.Apex.Core;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Services
{
    public class WindowsService : IDisposable, IHostedService
    {
        private readonly Config _config;
        private readonly CancellationTokenSource _cts;
        private readonly DataService _dataService;
        private readonly ILogger<WindowsService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Runner? _runner;

        #region Constructors

        public WindowsService(Config config, DataService dataService, ILogger<WindowsService> logger, IServiceProvider serviceProvider)
        {
            _config = config;
            _cts = new CancellationTokenSource();
            _dataService = dataService;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Methods

        private async Task ProcessAsync()
        {
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var features = scope.ServiceProvider.GetRequiredService<IEnumerable<IFeature>>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<State>>();
                    var offsets = await _dataService.OffsetsAsync().ConfigureAwait(false);
                    var state = new State(EmptyDriver.Instance, logger, offsets, 0);
                    _runner = Runner.Create(_config, features, state);
                    return;
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

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _runner?.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IHostedService

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return Task.CompletedTask;
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