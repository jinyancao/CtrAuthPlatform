using Ctr.AhphOcelot.Configuration;
using Microsoft.AspNetCore.Http;
using Ocelot.Configuration;
using Ocelot.Logging;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ctr.AhphOcelot.Authentication.Middleware
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-15
    /// 自定义授权中间件
    /// </summary>
    public class AhphAuthenticationMiddleware : OcelotMiddleware
    {
        private readonly OcelotRequestDelegate _next;
        private readonly AhphOcelotConfiguration _options;
        private readonly IAhphAuthenticationProcessor _ahphAuthenticationProcessor;
        public AhphAuthenticationMiddleware(OcelotRequestDelegate next,
            IOcelotLoggerFactory loggerFactory,
            IAhphAuthenticationProcessor ahphAuthenticationProcessor,
            AhphOcelotConfiguration options)
            : base(loggerFactory.CreateLogger<AhphAuthenticationMiddleware>())
        {
            _next = next;
            _ahphAuthenticationProcessor = ahphAuthenticationProcessor;
            _options = options;
        }

        public async Task Invoke(DownstreamContext context)
        {
            if (!context.IsError && context.HttpContext.Request.Method.ToUpper() != "OPTIONS" && IsAuthenticatedRoute(context.DownstreamReRoute))
            {
                if (!_options.ClientAuthorization)
                {
                    Logger.LogInformation($"未启用客户端认证中间件");
                    await _next.Invoke(context);
                }
                else
                {
                    Logger.LogInformation($"{context.HttpContext.Request.Path} 是认证路由. {MiddlewareName} 开始校验授权信息");
                    #region 提取客户端ID
                    var clientId = "client_cjy";
                    var path = context.DownstreamReRoute.UpstreamPathTemplate.OriginalValue; //路由地址
                    var clientClaim = context.HttpContext.User.Claims.FirstOrDefault(p => p.Type == _options.ClientKey);
                    if (!string.IsNullOrEmpty(clientClaim?.Value))
                    {//从Claims中提取客户端id
                        clientId = clientClaim?.Value;
                    }
                    #endregion
                    if (await _ahphAuthenticationProcessor.CheckClientAuthenticationAsync(clientId, path))
                    {
                        await _next.Invoke(context);
                    }
                    else
                    {//未授权直接返回错误
                        var error = new UnauthenticatedError($"请求认证路由 {context.HttpContext.Request.Path}客户端未授权");
                        Logger.LogWarning($"路由地址 {context.HttpContext.Request.Path} 自定义认证管道校验失败. {error}");
                        SetPipelineError(context, error);
                    }
                }
            }
            else
            {
                await _next.Invoke(context);
            }

        }
        private static bool IsAuthenticatedRoute(DownstreamReRoute reRoute)
        {
            return reRoute.IsAuthenticated;
        }
    }
}
