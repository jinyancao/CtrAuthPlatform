using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ctr.AhphOcelot.RateLimit
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-19
    /// 客户端限流计数器
    /// </summary>
    public struct AhphClientRateLimitCounter
    {
        [JsonConstructor]
        public AhphClientRateLimitCounter(DateTime timestamp, long totalRequests)
        {
            Timestamp = timestamp;
            TotalRequests = totalRequests;
        }

        /// <summary>
        /// 最后请求时间
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// 请求总数
        /// </summary>
        public long TotalRequests { get; private set; }
    }
}
