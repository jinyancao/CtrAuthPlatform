using Dapper;
using IdentityServer4.Dapper.Mappers;
using IdentityServer4.Dapper.Options;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace IdentityServer4.Dapper.Stores.SqlServer
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-03
    /// 实现提取客户端存储信息
    /// </summary>
    public class SqlServerClientStore: IClientStore
    {
        private readonly ILogger<SqlServerClientStore> _logger;
        private readonly DapperStoreOptions _configurationStoreOptions;

        public SqlServerClientStore(ILogger<SqlServerClientStore> logger, DapperStoreOptions configurationStoreOptions)
        {
            _logger = logger;
            _configurationStoreOptions = configurationStoreOptions;
        }

        /// <summary>
        /// 根据客户端ID 获取客户端信息内容
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var cModel = new Client();
            var _client = new Entities.Client();
            using (var connection = new SqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                //由于后续未用到，暂不实现 ClientPostLogoutRedirectUris ClientClaims ClientIdPRestrictions ClientCorsOrigins ClientProperties,有需要的自行添加。
                string sql = @"select * from Clients where ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientGrantTypes t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientRedirectUris t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientScopes t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientSecrets t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
                      ";
                var multi = await connection.QueryMultipleAsync(sql, new { client = clientId });
                var client = multi.Read<Entities.Client>();
                var ClientGrantTypes = multi.Read<Entities.ClientGrantType>();
                var ClientRedirectUris = multi.Read<Entities.ClientRedirectUri>();
                var ClientScopes = multi.Read<Entities.ClientScope>();
                var ClientSecrets = multi.Read<Entities.ClientSecret>();

                if (client != null && client.AsList().Count > 0)
                {//提取信息
                    _client = client.AsList()[0];
                    _client.AllowedGrantTypes = ClientGrantTypes.AsList();
                    _client.RedirectUris = ClientRedirectUris.AsList();
                    _client.AllowedScopes = ClientScopes.AsList();
                    _client.ClientSecrets = ClientSecrets.AsList();
                    cModel = _client.ToModel();
                }
            }
            _logger.LogDebug("{clientId} found in database: {clientIdFound}", clientId, _client != null);

            return cModel;
        }
    }
}
