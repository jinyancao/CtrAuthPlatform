using System;
using System.Collections.Generic;
using System.Text;

namespace Ctr.AhphOcelot.RateLimit
{
    public class RateLimitRuleModel
    {
        /// <summary>
        /// 是否启用限流
        /// </summary>
        public bool RateLimit { get; set; }

        /// <summary>
        /// 限流配置信息
        /// </summary>
        public List<AhphClientRateLimitOptions> rateLimitOptions { get; set; }
    }
}
