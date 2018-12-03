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
    /// 重写授权信息存储
    /// </summary>
    public class MySqlPersistedGrantStore : IPersistedGrantStore
    {
        private readonly ILogger<MySqlPersistedGrantStore> _logger;
        private readonly DapperStoreOptions _configurationStoreOptions;

        public MySqlPersistedGrantStore(ILogger<MySqlPersistedGrantStore> logger, DapperStoreOptions configurationStoreOptions)
        {
            _logger = logger;
            _configurationStoreOptions = configurationStoreOptions;
        }

        /// <summary>
        /// 根据用户标识获取所有的授权信息
        /// </summary>
        /// <param name="subjectId">用户标识</param>
        /// <returns></returns>
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                string sql = "select * from PersistedGrants where SubjectId=@subjectId";
                var data = (await connection.QueryAsync<Entities.PersistedGrant>(sql, new { subjectId }))?.AsList();
                var model = data.Select(x => x.ToModel());

                _logger.LogDebug("{persistedGrantCount} persisted grants found for {subjectId}", data.Count, subjectId);
                return model;
            }
        }

        /// <summary>
        /// 根据key获取授权信息
        /// </summary>
        /// <param name="key">认证信息</param>
        /// <returns></returns>
        public async Task<PersistedGrant> GetAsync(string key)
        {
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                string sql = "select * from PersistedGrants where [Key]=@key";
                var result = await connection.QueryFirstOrDefaultAsync<Entities.PersistedGrant>(sql, new { key });
                var model = result.ToModel();

                _logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);
                return model;
            }
        }

        /// <summary>
        /// 根据用户标识和客户端ID移除所有的授权信息
        /// </summary>
        /// <param name="subjectId">用户标识</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns></returns>
        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                string sql = "delete from PersistedGrants where ClientId=@clientId and SubjectId=@subjectId";
                await connection.ExecuteAsync(sql, new { subjectId, clientId });
                _logger.LogDebug("remove {subjectId} {clientId} from database success", subjectId, clientId);
            }
        }

        /// <summary>
        /// 移除指定的标识、客户端、类型等授权信息
        /// </summary>
        /// <param name="subjectId">标识</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="type">授权类型</param>
        /// <returns></returns>
        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                string sql = "delete from PersistedGrants where ClientId=@clientId and SubjectId=@subjectId and Type=@type";
                await connection.ExecuteAsync(sql, new { subjectId, clientId });
                _logger.LogDebug("remove {subjectId} {clientId} {type} from database success", subjectId, clientId, type);
            }
        }

        /// <summary>
        /// 移除指定KEY的授权信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string key)
        {
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                string sql = "delete from PersistedGrants where [Key]=@key";
                await connection.ExecuteAsync(sql, new { key });
                _logger.LogDebug("remove {key} from database success", key);
            }
        }

        /// <summary>
        /// 存储授权信息
        /// </summary>
        /// <param name="grant">实体</param>
        /// <returns></returns>
        public async Task StoreAsync(PersistedGrant grant)
        {
            using (var connection = new MySqlConnection(_configurationStoreOptions.DbConnectionStrings))
            {
                //移除防止重复
                await RemoveAsync(grant.Key);
                string sql = "insert into PersistedGrants([Key],ClientId,CreationTime,Data,Expiration,SubjectId,Type) values(@Key,@ClientId,@CreationTime,@Data,@Expiration,@SubjectId,@Type)";
                await connection.ExecuteAsync(sql, grant);
            }
        }
    }
}
