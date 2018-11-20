using Ocelot.Middleware.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ctr.AhphOcelot.RateLimit.Middleware
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-18
    /// 限流中间件扩展
    /// </summary>
    public static class AhphClientRateLimitMiddlewareExtensions
    {
        public static IOcelotPipelineBuilder UseAhphClientRateLimitMiddleware(this IOcelotPipelineBuilder builder)
        {
            return builder.UseMiddleware<AhphClientRateLimitMiddleware>();
        }
    }
}
