using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Ocelot.Configuration;
using Ocelot.Configuration.File;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Middleware.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Ocelot.Configuration.Creator;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.Configuration.Setter;
using Ocelot.Responses;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;

namespace Ctr.AhphOcelot.Middleware
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-10
    /// 基于Ocelot扩展网关中间件，主要是重写了CreateConfiguration方法，其他内容保持与原有的中间件内容不变。
    /// </summary>
    public static class AhphOcelotMiddlewareExtensions
    {
        
        public static async Task<IApplicationBuilder> UseAhphOcelot(this IApplicationBuilder builder)
        {
            await builder.UseAhphOcelot(new OcelotPipelineConfiguration());
            return builder;
        }

        public static async Task<IApplicationBuilder> UseAhphOcelot(this IApplicationBuilder builder, Action<OcelotPipelineConfiguration> pipelineConfiguration)
        {
            var config = new OcelotPipelineConfiguration();
            pipelineConfiguration?.Invoke(config);
            return await builder.UseAhphOcelot(config);
        }

        public static async Task<IApplicationBuilder> UseAhphOcelot(this IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
        {
            //重写创建配置方法
            var configuration = await CreateConfiguration(builder);

            ConfigureDiagnosticListener(builder);

            return CreateOcelotPipeline(builder, pipelineConfiguration);
        }

        private static IApplicationBuilder CreateOcelotPipeline(IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
        {
            var pipelineBuilder = new OcelotPipelineBuilder(builder.ApplicationServices);

            //pipelineBuilder.BuildOcelotPipeline(pipelineConfiguration);
            //使用自定义管道扩展 2018-11-15 金焰的世界
            pipelineBuilder.BuildAhphOcelotPipeline(pipelineConfiguration);

            var firstDelegate = pipelineBuilder.Build();

            /*
            inject first delegate into first piece of asp.net middleware..maybe not like this
            then because we are updating the http context in ocelot it comes out correct for
            rest of asp.net..
            */

            builder.Properties["analysis.NextMiddlewareName"] = "TransitionToOcelotMiddleware";

            builder.Use(async (context, task) =>
            {
                var downstreamContext = new DownstreamContext(context);
                await firstDelegate.Invoke(downstreamContext);
            });

            return builder;
        }

        private static async Task<IInternalConfiguration> CreateConfiguration(IApplicationBuilder builder)
        {
            //提取文件配置信息
            var fileConfig = await builder.ApplicationServices.GetService<IFileConfigurationRepository>().Get();
            var internalConfigCreator = builder.ApplicationServices.GetService<IInternalConfigurationCreator>();
            var internalConfig = await internalConfigCreator.Create(fileConfig.Data);
            //如果配置文件错误直接抛出异常
            if (internalConfig.IsError)
            {
                ThrowToStopOcelotStarting(internalConfig);
            }
            //配置信息缓存，这块需要注意实现方式，因为后期我们需要改造下满足分布式架构,这篇不做讲解
            var internalConfigRepo = builder.ApplicationServices.GetService<IInternalConfigurationRepository>();
            internalConfigRepo.AddOrReplace(internalConfig.Data);
            //获取中间件配置委托
            var configurations = builder.ApplicationServices.GetServices<OcelotMiddlewareConfigurationDelegate>();
            foreach (var configuration in configurations)
            {
                await configuration(builder);
            }
            return GetOcelotConfigAndReturn(internalConfigRepo);
        }

        private static bool IsError(Response response)
        {
            return response == null || response.IsError;
        }

        private static IInternalConfiguration GetOcelotConfigAndReturn(IInternalConfigurationRepository provider)
        {
            var ocelotConfiguration = provider.Get();

            if (ocelotConfiguration?.Data == null || ocelotConfiguration.IsError)
            {
                ThrowToStopOcelotStarting(ocelotConfiguration);
            }

            return ocelotConfiguration.Data;
        }

        private static void ThrowToStopOcelotStarting(Response config)
        {
            throw new Exception($"Unable to start Ocelot, errors are: {string.Join(",", config.Errors.Select(x => x.ToString()))}");
        }

        private static void ConfigureDiagnosticListener(IApplicationBuilder builder)
        {
            var env = builder.ApplicationServices.GetService<IHostingEnvironment>();
            var listener = builder.ApplicationServices.GetService<OcelotDiagnosticListener>();
            var diagnosticListener = builder.ApplicationServices.GetService<DiagnosticListener>();
            diagnosticListener.SubscribeWithAdapter(listener);
        }
    }
}
