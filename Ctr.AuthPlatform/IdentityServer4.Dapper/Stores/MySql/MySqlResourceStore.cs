using Dapper;
using IdentityServer4.Dapper.Mappers;
using IdentityServer4.Dapper.Options;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Dapper.Stores.MySql
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-03
    /// 重写资源存储方法
    /// </summary>
    public class MySqlResourceStore : IResourceStore
    {
        private readonly ILogger<MySqlResourceStore> _logger;
        private readonly DapperStoreOptions _configurationStoreOptions;

        public MySqlResourceStore(ILogger<MySqlResourceStore> logger, DapperStoreOptions configurationStoreOptions)
        {
            _logger = logger;
            _configurationStoreOptions = configurationStoreOptions;
        }

        /// <summary>
        /// 根据api名称获取相关信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var model = new ApiResource();
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                string sql = @"select * from ApiResources where Name=@Name and Enabled=1;
                       select * from ApiResources t1 inner join ApiScopes t2 on t1.Id=t2.ApiResourceId where t1.Name=@name and Enabled=1;
                    ";
                var multi = await connection.QueryMultipleAsync(sql, new { name });
                var ApiResources = multi.Read<Entities.ApiResource>();
                var ApiScopes = multi.Read<Entities.ApiScope>();
                if (ApiResources != null && ApiResources.AsList()?.Count > 0)
                {
                    var apiresource = ApiResources.AsList()[0];
                    apiresource.Scopes = ApiScopes.AsList();
                    if (apiresource != null)
                    {
                        _logger.LogDebug("Found {api} API resource in database", name);
                    }
                    else
                    {
                        _logger.LogDebug("Did not find {api} API resource in database", name);
                    }
                    model = apiresource.ToModel();
                }
            }
            return model;
        }

        /// <summary>
        /// 根据作用域信息获取接口资源
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var apiResourceData = new List<ApiResource>();
            string _scopes = "";
            foreach (var scope in scopeNames)
            {
                _scopes += "'" + scope + "',";
            }
            if (_scopes == "")
            {
                return null;
            }
            else
            {
                _scopes = _scopes.Substring(0, _scopes.Length - 1);
            }
            string sql = "select distinct t1.* from ApiResources t1 inner join ApiScopes t2 on t1.Id=t2.ApiResourceId where t2.Name in(" + _scopes + ") and Enabled=1;";
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                var apir = (await connection.QueryAsync<Entities.ApiResource>(sql))?.AsList();
                if (apir != null && apir.Count > 0)
                {
                    foreach (var apimodel in apir)
                    {
                        sql = "select * from ApiScopes where ApiResourceId=@id";
                        var scopedata = (await connection.QueryAsync<Entities.ApiScope>(sql, new { id = apimodel.Id }))?.AsList();
                        apimodel.Scopes = scopedata;
                        apiResourceData.Add(apimodel.ToModel());
                    }
                    _logger.LogDebug("Found {scopes} API scopes in database", apiResourceData.SelectMany(x => x.Scopes).Select(x => x.Name));
                }
            }
            return apiResourceData;
        }

        /// <summary>
        /// 根据scope获取身份资源
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var apiResourceData = new List<IdentityResource>();
            string _scopes = "";
            foreach (var scope in scopeNames)
            {
                _scopes += "'" + scope + "',";
            }
            if (_scopes == "")
            {
                return null;
            }
            else
            {
                _scopes = _scopes.Substring(0, _scopes.Length - 1);
            }
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                //暂不实现 IdentityClaims
                string sql = "select * from IdentityResources where Enabled=1 and Name in(" + _scopes + ")";
                var data = (await connection.QueryAsync<Entities.IdentityResource>(sql))?.AsList();
                if (data != null && data.Count > 0)
                {
                    foreach (var model in data)
                    {
                        apiResourceData.Add(model.ToModel());
                    }
                }
            }
            return apiResourceData;
        }

        /// <summary>
        /// 获取所有资源实现
        /// </summary>
        /// <returns></returns>
        public async Task<Resources> GetAllResourcesAsync()
        {
            var apiResourceData = new List<ApiResource>();
            var identityResourceData = new List<IdentityResource>();
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                string sql = "select * from IdentityResources where Enabled=1";
                var data = (await connection.QueryAsync<Entities.IdentityResource>(sql))?.AsList();
                if (data != null && data.Count > 0)
                {

                    foreach (var m in data)
                    {
                        identityResourceData.Add(m.ToModel());
                    }
                }
                //获取apiresource
                sql = "select * from ApiResources where Enabled=1";
                var apidata = (await connection.QueryAsync<Entities.ApiResource>(sql))?.AsList();
                if (apidata != null && apidata.Count > 0)
                {
                    foreach (var m in apidata)
                    {
                        sql = "select * from ApiScopes where ApiResourceId=@id";
                        var scopedata = (await connection.QueryAsync<Entities.ApiScope>(sql, new { id = m.Id }))?.AsList();
                        m.Scopes = scopedata;
                        apiResourceData.Add(m.ToModel());
                    }
                }
            }
            var model = new Resources(identityResourceData, apiResourceData);
            return model;
        }
    }
}
