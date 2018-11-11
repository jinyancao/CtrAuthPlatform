using Ctr.AhphOcelot.Configuration;
using Ctr.AhphOcelot.Model;
using Dapper;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Ctr.AhphOcelot.DataBase.SqlServer
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-11
    /// 使用SqlServer来实现配置文件仓储接口
    /// </summary>
    public class SqlServerFileConfigurationRepository : IFileConfigurationRepository
    {
        private readonly AhphOcelotConfiguration _option;
        public SqlServerFileConfigurationRepository(AhphOcelotConfiguration option)
        {
            _option = option;
        }

        /// <summary>
        /// 从数据库中获取配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<FileConfiguration>> Get()
        {
            #region 提取配置信息
            var file = new FileConfiguration();
            //提取默认启用的路由配置信息
            string glbsql = "select * from AhphGlobalConfiguration where IsDefault=1 and InfoStatus=1";
            //提取全局配置信息
            using (var connection = new SqlConnection(_option.DbConnectionStrings))
            {
                var result = await connection.QueryFirstOrDefaultAsync<AhphGlobalConfiguration>(glbsql);
                if (result != null)
                {
                    var glb = new FileGlobalConfiguration();
                    //赋值全局信息
                    glb.BaseUrl = result.BaseUrl;
                    glb.DownstreamScheme = result.DownstreamScheme;
                    glb.RequestIdKey = result.RequestIdKey;
                    //glb.HttpHandlerOptions = result.HttpHandlerOptions?.ToObject<FileHttpHandlerOptions>();
                    //glb.LoadBalancerOptions = result.LoadBalancerOptions?.ToObject<FileLoadBalancerOptions>();
                    //glb.QoSOptions = result.QoSOptions?.ToObject<FileQoSOptions>();
                    //glb.ServiceDiscoveryProvider = result.ServiceDiscoveryProvider?.ToObject<FileServiceDiscoveryProvider>();
                    if (!String.IsNullOrEmpty(result.HttpHandlerOptions))
                    {
                        glb.HttpHandlerOptions = result.HttpHandlerOptions.ToObject<FileHttpHandlerOptions>();
                    }
                    if (!String.IsNullOrEmpty(result.LoadBalancerOptions))
                    {
                        glb.LoadBalancerOptions = result.LoadBalancerOptions.ToObject<FileLoadBalancerOptions>();
                    }
                    if (!String.IsNullOrEmpty(result.QoSOptions))
                    {
                        glb.QoSOptions = result.QoSOptions.ToObject<FileQoSOptions>();
                    }
                    if (!String.IsNullOrEmpty(result.ServiceDiscoveryProvider))
                    {
                        glb.ServiceDiscoveryProvider = result.ServiceDiscoveryProvider.ToObject<FileServiceDiscoveryProvider>();
                    }
                    file.GlobalConfiguration = glb;

                    //提取所有路由信息
                    string routesql = "select T2.* from AhphConfigReRoutes T1 inner join AhphReRoute T2 on T1.ReRouteId=T2.ReRouteId where AhphId=@AhphId and InfoStatus=1";
                    var routeresult = (await connection.QueryAsync<AhphReRoute>(routesql, new { result.AhphId }))?.AsList();
                    if (routeresult != null && routeresult.Count > 0)
                    {
                        var reroutelist = new List<FileReRoute>();
                        foreach (var model in routeresult)
                        {
                            var m = new FileReRoute();
                            //m.AuthenticationOptions = model.AuthenticationOptions?.ToObject<FileAuthenticationOptions>();
                            //m.FileCacheOptions = model.CacheOptions?.ToObject<FileCacheOptions>();
                            //m.DelegatingHandlers = model.DelegatingHandlers?.ToObject<List<string>>();
                            //m.LoadBalancerOptions = model.LoadBalancerOptions?.ToObject<FileLoadBalancerOptions>();
                            //m.QoSOptions = model.QoSOptions?.ToObject<FileQoSOptions>();
                            //m.DownstreamHostAndPorts = model.DownstreamHostAndPorts?.ToObject<List<FileHostAndPort>>();
                            if (!String.IsNullOrEmpty(model.AuthenticationOptions))
                            {
                                m.AuthenticationOptions = model.AuthenticationOptions.ToObject<FileAuthenticationOptions>();
                            }
                            if (!String.IsNullOrEmpty(model.CacheOptions))
                            {
                                m.FileCacheOptions = model.CacheOptions.ToObject<FileCacheOptions>();
                            }
                            if (!String.IsNullOrEmpty(model.DelegatingHandlers))
                            {
                                m.DelegatingHandlers = model.DelegatingHandlers.ToObject<List<string>>();
                            }
                            if (!String.IsNullOrEmpty(model.LoadBalancerOptions))
                            {
                                m.LoadBalancerOptions = model.LoadBalancerOptions.ToObject<FileLoadBalancerOptions>();
                            }
                            if (!String.IsNullOrEmpty(model.QoSOptions))
                            {
                                m.QoSOptions = model.QoSOptions.ToObject<FileQoSOptions>();
                            }
                            if (!String.IsNullOrEmpty(model.DownstreamHostAndPorts))
                            {
                                m.DownstreamHostAndPorts = model.DownstreamHostAndPorts.ToObject<List<FileHostAndPort>>();
                            }
                            //开始赋值
                            m.DownstreamPathTemplate = model.DownstreamPathTemplate;
                            m.DownstreamScheme = model.DownstreamScheme;
                            m.Key = model.RequestIdKey;
                            m.Priority = model.Priority ?? 0;
                            m.RequestIdKey = model.RequestIdKey;
                            m.ServiceName = model.ServiceName;
                            m.UpstreamHost = model.UpstreamHost;
                            m.UpstreamHttpMethod = model.UpstreamHttpMethod?.ToObject<List<string>>();
                            m.UpstreamPathTemplate = model.UpstreamPathTemplate;
                            reroutelist.Add(m);
                        }
                        file.ReRoutes = reroutelist;
                    }
                }
                else
                {
                    throw new Exception("未监测到任何可用的配置信息");
                }
            }
            #endregion
            if (file.ReRoutes == null || file.ReRoutes.Count == 0)
            {
                return new OkResponse<FileConfiguration>(null);
            }
            return new OkResponse<FileConfiguration>(file);
        }

        //由于数据库存储可不实现Set接口直接返回
        public async Task<Response> Set(FileConfiguration fileConfiguration)
        {
            return new OkResponse();
        }
    }
}
