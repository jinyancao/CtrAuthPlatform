using Ctr.AhphOcelot.Configuration;
using Ocelot.Configuration;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ctr.AhphOcelot.Cache
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-14
    /// 使用redis存储内部配置信息
    /// </summary>
    public class RedisInternalConfigurationRepository : IInternalConfigurationRepository
    {
        private readonly AhphOcelotConfiguration _options;
        private IInternalConfiguration _internalConfiguration;
        public RedisInternalConfigurationRepository(AhphOcelotConfiguration options)
        {
            _options = options;
            CSRedis.CSRedisClient csredis;
            if (options.RedisConnectionStrings.Count == 1)
            {
                //普通模式
                csredis = new CSRedis.CSRedisClient(options.RedisConnectionStrings[0]);
            }
            else
            {
                //集群模式
                //实现思路：根据key.GetHashCode() % 节点总数量，确定连向的节点
                //也可以自定义规则(第一个参数设置)
                csredis = new CSRedis.CSRedisClient(null, options.RedisConnectionStrings.ToArray());
            }
            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
        }

        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="internalConfiguration">配置信息</param>
        /// <returns></returns>
        public Response AddOrReplace(IInternalConfiguration internalConfiguration)
        {
            var key = _options.RedisKeyPrefix + "-internalConfiguration";
            RedisHelper.Set(key, internalConfiguration.ToJson());
            return new OkResponse();
        }

        /// <summary>
        /// 从缓存中获取配置信息
        /// </summary>
        /// <returns></returns>
        public Response<IInternalConfiguration> Get()
        {
            var key = _options.RedisKeyPrefix + "-internalConfiguration";
            var result = RedisHelper.Get<InternalConfiguration>(key);
            if (result!=null)
            {
                return new OkResponse<IInternalConfiguration>(result);
            }
            return new OkResponse<IInternalConfiguration>(default(InternalConfiguration));
        }
    }
}
