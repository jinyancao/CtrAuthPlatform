using IdentityServer4.Dapper.Interfaces;
using IdentityServer4.Dapper.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Dapper.HostedServices
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-03
    /// 清理过期Token方法
    /// </summary>
    public class TokenCleanup
    {
        private readonly ILogger<TokenCleanup> _logger;
        private readonly DapperStoreOptions _options;
        private readonly IPersistedGrants _persistedGrants;
        private CancellationTokenSource _source;

        public TimeSpan CleanupInterval => TimeSpan.FromSeconds(_options.TokenCleanupInterval);

        public TokenCleanup(IPersistedGrants persistedGrants, ILogger<TokenCleanup> logger, DapperStoreOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            if (_options.TokenCleanupInterval < 1) throw new ArgumentException("Token cleanup interval must be at least 1 second");

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _persistedGrants = persistedGrants;
        }

        public void Start()
        {
            Start(CancellationToken.None);
        }

        public void Start(CancellationToken cancellationToken)
        {
            if (_source != null) throw new InvalidOperationException("Already started. Call Stop first.");

            _logger.LogDebug("Starting token cleanup");

            _source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Factory.StartNew(() => StartInternal(_source.Token));
        }

        public void Stop()
        {
            if (_source == null) throw new InvalidOperationException("Not started. Call Start first.");

            _logger.LogDebug("Stopping token cleanup");

            _source.Cancel();
            _source = null;
        }

        private async Task StartInternal(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("CancellationRequested. Exiting.");
                    break;
                }

                try
                {
                    await Task.Delay(CleanupInterval, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogDebug("TaskCanceledException. Exiting.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Task.Delay exception: {0}. Exiting.", ex.Message);
                    break;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("CancellationRequested. Exiting.");
                    break;
                }

                ClearTokens();
            }
        }

        public void ClearTokens()
        {
            try
            {
                _logger.LogTrace("Querying for tokens to clear");

                //提取满足条件的信息进行删除
                _persistedGrants.RemoveExpireToken(DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception clearing tokens: {exception}", ex.Message);
            }
        }
    }
}
