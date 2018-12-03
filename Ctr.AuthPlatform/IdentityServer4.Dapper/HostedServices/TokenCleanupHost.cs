using IdentityServer4.Dapper.Options;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Dapper.HostedServices
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-03
    /// 授权后端清理服务
    /// </summary>
    public class TokenCleanupHost : IHostedService
    {
        private readonly TokenCleanup _tokenCleanup;
        private readonly DapperStoreOptions _options;

        public TokenCleanupHost(TokenCleanup tokenCleanup, DapperStoreOptions options)
        {
            _tokenCleanup = tokenCleanup;
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_options.EnableTokenCleanup)
            {
                _tokenCleanup.Start(cancellationToken);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_options.EnableTokenCleanup)
            {
                _tokenCleanup.Stop();
            }
            return Task.CompletedTask;
        }
    }
}
