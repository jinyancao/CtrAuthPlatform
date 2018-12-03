using Dapper;
using IdentityServer4.Dapper.Interfaces;
using IdentityServer4.Dapper.Options;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace IdentityServer4.Dapper.Stores.MySql
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-03
    /// 实现授权信息自定义管理
    /// </summary>
    public class MySqlPersistedGrants : IPersistedGrants
    {
        private readonly ILogger<MySqlPersistedGrants> _logger;
        private readonly DapperStoreOptions _configurationStoreOptions;

        public MySqlPersistedGrants(ILogger<MySqlPersistedGrants> logger, DapperStoreOptions configurationStoreOptions)
        {
            _logger = logger;
            _configurationStoreOptions = configurationStoreOptions;
        }


        /// <summary>
        /// 移除指定的时间过期授权信息
        /// </summary>
        /// <param name="dt">Utc时间</param>
        /// <returns></returns>
        public async Task RemoveExpireToken(DateTime dt)
        {
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                string sql = "delete from PersistedGrants where Expiration>@dt";
                await connection.ExecuteAsync(sql, new { dt });
            }
        }
    }
}
