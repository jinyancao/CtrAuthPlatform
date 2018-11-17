using Ocelot.Middleware.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ctr.AhphOcelot.Authentication.Middleware
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-15
    /// 使用自定义授权中间件
    /// </summary>
    public static class AhphAuthenticationMiddlewareExtensions
    {
        public static IOcelotPipelineBuilder UseAhphAuthenticationMiddleware(this IOcelotPipelineBuilder builder)
        {
            return builder.UseMiddleware<AhphAuthenticationMiddleware>();
        }
    }
}
