using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ctr.AhphOcelot.Middleware;
using IdentityServer4.AccessTokenValidation;
using Ocelot.Administration;

namespace Ctr.AuthPlatform.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationProviderKey = "TestKey";
            Action<IdentityServerAuthenticationOptions> gatewayoptions = o =>
            {
                o.Authority = "http://localhost:6611";
                o.ApiName = "gateway";
                o.RequireHttpsMetadata = false;
            };

            services.AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey, gatewayoptions);

            Action<IdentityServerAuthenticationOptions> options = o =>
            {
                o.Authority = "http://localhost:6611"; //IdentityServer地址
                o.RequireHttpsMetadata = false;
                o.ApiName = "gateway_admin"; //网关管理的名称，对应的为客户端授权的scope
            };
            services.AddOcelot().AddAhphOcelot(option =>
            {
                //option.DbConnectionStrings = "Server=localhost;Database=Ctr_AuthPlatform;User ID=root;Password=bl123456;";
                option.DbConnectionStrings = "Server=.;Database=Ctr_AuthPlatform;User ID=sa;Password=bl123456;";
                option.RedisConnectionStrings=new List<string>() {         "192.168.1.111:6379,password=bl123456,defaultDatabase=0,poolsize=50,ssl=false,writeBuffer=10240,connectTimeout=1000,connectRetry=1;"
                };
                //option.EnableTimer = true;//启用定时任务
                //option.TimerDelay = 10 * 000;//周期10秒
                option.ClientAuthorization = true;
            })
            //.UseMySql()
            .AddAdministration("/CtrOcelot", options);
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseAhphOcelot().Wait();
        }
    }
}
