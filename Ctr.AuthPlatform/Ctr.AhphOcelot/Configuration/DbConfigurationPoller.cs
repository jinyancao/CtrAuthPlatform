using Microsoft.Extensions.Hosting;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using Ocelot.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ctr.AhphOcelot.Configuration
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-12
    /// 数据库配置信息更新策略
    /// </summary>
    public class DbConfigurationPoller : IHostedService, IDisposable
    {
        private readonly IOcelotLogger _logger;
        private readonly IFileConfigurationRepository _repo;
        private readonly AhphOcelotConfiguration _option;
        private Timer _timer;
        private bool _polling;
        private readonly IInternalConfigurationRepository _internalConfigRepo;
        private readonly IInternalConfigurationCreator _internalConfigCreator;
        public DbConfigurationPoller(IOcelotLoggerFactory factory,
            IFileConfigurationRepository repo,
            IInternalConfigurationRepository internalConfigRepo,
            IInternalConfigurationCreator internalConfigCreator, 
            AhphOcelotConfiguration option)
        {
            _internalConfigRepo = internalConfigRepo;
            _internalConfigCreator = internalConfigCreator;
            _logger = factory.CreateLogger<DbConfigurationPoller>();
            _repo = repo;
            _option = option;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_option.EnableTimer)
            {//判断是否启用自动更新
                _logger.LogInformation($"{nameof(DbConfigurationPoller)} is starting.");
                _timer = new Timer(async x =>
                {
                    if (_polling)
                    {
                        return;
                    }
                    _polling = true;
                    await Poll();
                    _polling = false;
                }, null, _option.TimerDelay, _option.TimerDelay);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_option.EnableTimer)
            {//判断是否启用自动更新
                _logger.LogInformation($"{nameof(DbConfigurationPoller)} is stopping.");
                _timer?.Change(Timeout.Infinite, 0);
            }
            return Task.CompletedTask;
        }

        private async Task Poll()
       {
            _logger.LogInformation("Started polling");

            var fileConfig = await _repo.Get();

            if (fileConfig.IsError)
            {
                _logger.LogWarning($"error geting file config, errors are {string.Join(",", fileConfig.Errors.Select(x => x.Message))}");
                return;
            }
            else
            {
                var config = await _internalConfigCreator.Create(fileConfig.Data);
                if (!config.IsError)
                {
                    _internalConfigRepo.AddOrReplace(config.Data);
                }
            }
            _logger.LogInformation("Finished polling");
        }
    }
}
