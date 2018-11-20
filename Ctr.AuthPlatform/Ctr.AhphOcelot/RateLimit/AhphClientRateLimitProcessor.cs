using Ctr.AhphOcelot.Configuration;
using Ocelot.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ctr.AhphOcelot.RateLimit
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-19
    /// 实现客户端限流处理器
    /// </summary>
    public class AhphClientRateLimitProcessor : IClientRateLimitProcessor
    {
        private readonly AhphOcelotConfiguration _options;
        private readonly IOcelotCache<ClientRoleModel> _ocelotCache;
        private readonly IOcelotCache<RateLimitRuleModel> _rateLimitRuleCache;
        private readonly IOcelotCache<AhphClientRateLimitCounter?> _clientRateLimitCounter;
        private readonly IClientRateLimitRepository _clientRateLimitRepository;
        private static readonly object _processLocker = new object();
        public AhphClientRateLimitProcessor(AhphOcelotConfiguration options,IClientRateLimitRepository clientRateLimitRepository, IOcelotCache<AhphClientRateLimitCounter?> clientRateLimitCounter, IOcelotCache<ClientRoleModel> ocelotCache, IOcelotCache<RateLimitRuleModel> rateLimitRuleCache)
        {
            _options = options;
            _clientRateLimitRepository = clientRateLimitRepository;
            _clientRateLimitCounter = clientRateLimitCounter;
            _ocelotCache = ocelotCache;
            _rateLimitRuleCache = rateLimitRuleCache;
        }

        /// <summary>
        /// 校验客户端限流结果
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        public async Task<bool> CheckClientRateLimitResultAsync(string clientid, string path)
        {

            var result = false;
            var clientRule = new List<AhphClientRateLimitOptions>();
            //1、校验路由是否有限流策略
            result = !await CheckReRouteRuleAsync(path);
            if (!result)
            {//2、校验客户端是否被限流了
                var limitResult = await CheckClientRateLimitAsync(clientid, path);
                result = !limitResult.RateLimit;
                clientRule = limitResult.rateLimitOptions;
            }
            if (!result)
            {//3、校验客户端是否启动白名单
                result = await CheckClientReRouteWhiteListAsync(clientid, path);
            }
            if (!result)
            {//4、校验是否触发限流及计数
                result = CheckRateLimitResult(clientRule);
            }
            return result;

        }

        /// <summary>
        /// 检验是否启用限流规则
        /// </summary>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        private async Task<bool> CheckReRouteRuleAsync(string path)
        {
            var region = _options.RedisKeyPrefix + "CheckReRouteRuleAsync";
            var key = region + path;
            var cacheResult = _ocelotCache.Get(key, region);
            if (cacheResult != null)
            {//提取缓存数据
                return cacheResult.Role;
            }
            else
            {//重新获取限流策略
                var result = await _clientRateLimitRepository.CheckReRouteRuleAsync(path);
                _ocelotCache.Add(key, new ClientRoleModel() { CacheTime = DateTime.Now, Role = result }, TimeSpan.FromSeconds(_options.ClientRateLimitCacheTime), region);
                return result;
            }
            
        }

        /// <summary>
        /// 校验客户端限流规则
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        private async Task<(bool RateLimit, List<AhphClientRateLimitOptions> rateLimitOptions)> CheckClientRateLimitAsync(string clientid, string path)
        {
            var region = _options.RedisKeyPrefix + "CheckClientRateLimitAsync";
            var key = region + clientid + path;
            var cacheResult = _rateLimitRuleCache.Get(key, region);
            if (cacheResult != null)
            {//提取缓存数据
                return (cacheResult.RateLimit, cacheResult.rateLimitOptions);
            }
            else
            {//重新获取限流策略
                var result = await _clientRateLimitRepository.CheckClientRateLimitAsync(clientid, path);
                _rateLimitRuleCache.Add(key, new RateLimitRuleModel() { RateLimit=result.RateLimit, rateLimitOptions=result.rateLimitOptions }, TimeSpan.FromSeconds(_options.ClientRateLimitCacheTime), region);
                return result;
            }
        }

        /// <summary>
        /// 校验是否设置了路由白名单
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        private async Task<bool> CheckClientReRouteWhiteListAsync(string clientid, string path)
        {
            var region = _options.RedisKeyPrefix + "CheckClientReRouteWhiteListAsync";
            var key = region +clientid+ path;
            var cacheResult = _ocelotCache.Get(key, region);
            if (cacheResult != null)
            {//提取缓存数据
                return cacheResult.Role;
            }
            else
            {//重新获取限流策略
                var result = await _clientRateLimitRepository.CheckClientReRouteWhiteListAsync(clientid,path);
                _ocelotCache.Add(key, new ClientRoleModel() { CacheTime = DateTime.Now, Role = result }, TimeSpan.FromSeconds(_options.ClientRateLimitCacheTime), region);
                return result;
            }
        }

        /// <summary>
        /// 校验完整的限流规则
        /// </summary>
        /// <param name="rateLimitOptions">限流配置</param>
        /// <returns></returns>
        private bool CheckRateLimitResult(List<AhphClientRateLimitOptions> rateLimitOptions)
        {
            bool result = true;
            if (rateLimitOptions != null && rateLimitOptions.Count > 0)
            {//校验策略
                foreach (var op in rateLimitOptions)
                {
                    AhphClientRateLimitCounter counter = new AhphClientRateLimitCounter(DateTime.UtcNow, 1);
                    //分别对每个策略校验
                    var enablePrefix = _options.RedisKeyPrefix + "RateLimitRule";
                    var key = AhphOcelotHelper.ComputeCounterKey(enablePrefix, op.ClientId, op.Period, op.RateLimitPath);
                    var periodTimestamp = AhphOcelotHelper.ConvertToSecond(op.Period);
                    lock (_processLocker)
                    {
                        var rateLimitCounter = _clientRateLimitCounter.Get(key, enablePrefix);
                        if (rateLimitCounter.HasValue)
                        {//提取当前的计数情况
                         // 请求次数增长
                            var totalRequests = rateLimitCounter.Value.TotalRequests + 1;
                            // 深拷贝
                            counter = new AhphClientRateLimitCounter(rateLimitCounter.Value.Timestamp, totalRequests);
                        }
                        else
                        {//写入限流策略
                            _clientRateLimitCounter.Add(key, counter,TimeSpan.FromSeconds(periodTimestamp), enablePrefix);
                        }
                    }
                    if (counter.TotalRequests > op.Limit)
                    {//更新请求记录，并标记为失败
                        result = false;
                    }
                    if (counter.TotalRequests > 1 && counter.TotalRequests <= op.Limit)
                    {//更新缓存配置信息
                        //获取限流剩余时间
                        var cur = (int)(counter.Timestamp.AddSeconds(periodTimestamp) - DateTime.UtcNow).TotalSeconds;
                        _clientRateLimitCounter.Add(key, counter, TimeSpan.FromSeconds(cur), enablePrefix);
                    }
                }
            }
            return result;
        }
    }
}
