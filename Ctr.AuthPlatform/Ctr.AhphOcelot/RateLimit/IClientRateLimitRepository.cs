using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ctr.AhphOcelot.RateLimit
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-19
    /// 客户端限流相关仓储接口
    /// </summary>
    public interface IClientRateLimitRepository
    {
        /// <summary>
        /// 校验是否启用限流规则
        /// </summary>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        Task<bool> CheckReRouteRuleAsync(string path);

        /// <summary>
        /// 校验客户端限流规则
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        Task<(bool RateLimit, List<AhphClientRateLimitOptions> rateLimitOptions)> CheckClientRateLimitAsync(string clientid, string path);

        /// <summary>
        /// 校验是否设置了路由白名单
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        Task<bool> CheckClientReRouteWhiteListAsync(string clientid, string path);
    }
}
