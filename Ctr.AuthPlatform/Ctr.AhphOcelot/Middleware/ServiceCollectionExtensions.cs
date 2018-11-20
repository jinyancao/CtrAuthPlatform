using Ctr.AhphOcelot.Authentication;
using Ctr.AhphOcelot.Cache;
using Ctr.AhphOcelot.Configuration;
using Ctr.AhphOcelot.DataBase.MySql;
using Ctr.AhphOcelot.DataBase.SqlServer;
using Ctr.AhphOcelot.RateLimit;
using Ctr.AhphOcelot.Responder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Cache;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.Responder;
using System;

namespace Ctr.AhphOcelot.Middleware
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-12
    /// 扩展Ocelot实现的自定义的注入
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加默认的注入方式，所有需要传入的参数都是用默认值
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IOcelotBuilder AddAhphOcelot(this IOcelotBuilder builder, Action<AhphOcelotConfiguration> option)
        {
            builder.Services.Configure(option);
            //配置信息
            builder.Services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<AhphOcelotConfiguration>>().Value);
            //配置文件仓储注入
            builder.Services.AddSingleton<IFileConfigurationRepository, SqlServerFileConfigurationRepository>();
            builder.Services.AddSingleton<IClientAuthenticationRepository, SqlServerClientAuthenticationRepository>();
            builder.Services.AddSingleton<IClientRateLimitRepository, SqlServerClientRateLimitRepository>();
            //注册后端服务
            builder.Services.AddHostedService<DbConfigurationPoller>();
            //使用Redis重写缓存
            builder.Services.AddSingleton<IOcelotCache<FileConfiguration>, InRedisCache<FileConfiguration>>();
            builder.Services.AddSingleton<IOcelotCache<CachedResponse>, InRedisCache<CachedResponse>>();
            builder.Services.AddSingleton<IInternalConfigurationRepository, RedisInternalConfigurationRepository>();
            builder.Services.AddSingleton<IOcelotCache<ClientRoleModel>, InRedisCache<ClientRoleModel>>();
            builder.Services.AddSingleton<IOcelotCache<RateLimitRuleModel>, InRedisCache<RateLimitRuleModel>>();
            builder.Services.AddSingleton<IOcelotCache<AhphClientRateLimitCounter?>, InRedisCache<AhphClientRateLimitCounter?>>();
            //注入授权
            builder.Services.AddSingleton<IAhphAuthenticationProcessor, AhphAuthenticationProcessor>();
            //注入限流实现
            builder.Services.AddSingleton<IClientRateLimitProcessor, AhphClientRateLimitProcessor>();

            //重写错误状态码
            builder.Services.AddSingleton<IErrorsToHttpStatusCodeMapper, AhphErrorsToHttpStatusCodeMapper>();
            return builder;
        }

        /// <summary>
        /// 扩展使用Mysql存储。
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IOcelotBuilder UseMySql(this IOcelotBuilder builder)
        {
            builder.Services.AddSingleton<IFileConfigurationRepository, MySqlFileConfigurationRepository>();
            builder.Services.AddSingleton<IClientAuthenticationRepository, MySqlClientAuthenticationRepository>();
            builder.Services.AddSingleton<IClientRateLimitRepository, MySqlClientRateLimitRepository>();
            return builder;
        }
    }
}
