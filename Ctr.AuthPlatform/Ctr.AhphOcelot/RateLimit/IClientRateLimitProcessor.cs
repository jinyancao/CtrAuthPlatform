using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ctr.AhphOcelot.RateLimit
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-18
    /// 客户端限流处理器
    /// </summary>
    public interface IClientRateLimitProcessor
    {
        /// <summary>
        /// 校验客户端限流结果
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">应用策略地址</param>
        /// <returns></returns>
        Task<bool> CheckClientRateLimitResultAsync(string clientid, string path);
    }
}
