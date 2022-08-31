using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tanya.Game.Apex.Core.Interfaces;
using Tanya.Game.Apex.Models;

namespace Tanya.Game.Apex.Services
{
    public class DataService : IDisposable, IHostedService
    {
        private readonly Config _config;
        private readonly CancellationTokenSource _cts;
        private readonly HttpClient _http;
        private readonly ILogger<DataService> _logger;
        private readonly TaskCompletionSource _tcs;
        private IOffsets? _offsets;

        #region Constructors

        public DataService(Config config, ILogger<DataService> logger)
        {
            _config = config;
            _cts = new CancellationTokenSource();
            _http = new HttpClient();
            _logger = logger;
            _tcs = new TaskCompletionSource();
        }

        #endregion

        #region Methods

        public async Task<IOffsets> OffsetsAsync()
        {
            await _tcs.Task.ConfigureAwait(false);
            return _offsets ?? throw new OperationCanceledException();
        }

        private async Task FetchAsync()
        {
            var url = _config.OffsetsUrl;
            _logger.LogInformation("Fetching offsets: {0}", url);
            _offsets = IniOffsets.Create(await _http.GetStringAsync(url, _cts.Token).ConfigureAwait(false));
            _logger.LogInformation("Finished offsets: {0}", _offsets);
            _tcs.TrySetResult();
        }

        private async Task ProcessAsync()
        {
            var previousTime = DateTime.MinValue;
            var previousUrl = _config.OffsetsUrl;

            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    if (previousUrl != _config.OffsetsUrl || previousTime + TimeSpan.FromMilliseconds(_config.OffsetsRefresh) < DateTime.UtcNow)
                    {
                        await FetchAsync().ConfigureAwait(false);
                        previousTime = DateTime.UtcNow;
                        previousUrl = _config.OffsetsUrl;
                    }
                    else
                    {
                        await Task.Delay(_config.OffsetsCheck, _cts.Token).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    await Task.Delay(_config.OffsetsCheck, _cts.Token).ConfigureAwait(false);
                }
            }
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _http.Dispose();
            _tcs.TrySetCanceled();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IHostedService

        public Task StartAsync(CancellationToken cancellationToken)
        {
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