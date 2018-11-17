using Ctr.AhphOcelot.Authentication;
using Ctr.AhphOcelot.Configuration;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace Ctr.AhphOcelot.DataBase.SqlServer
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-16
    /// 使用sqlserver实现客户端授权仓储
    /// </summary>
    public class MySqlClientAuthenticationRepository : IClientAuthenticationRepository
    {
        private readonly AhphOcelotConfiguration _option;
        public MySqlClientAuthenticationRepository(AhphOcelotConfiguration option)
        {
            _option = option;
        }
        /// <summary>
        /// 校验获取客户端是否有访问权限
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">请求路由</param>
        /// <returns></returns>
        public async Task<bool> ClientAuthenticationAsync(string clientid, string path)
        {
            using (var connection = new MySqlConnection(_option.DbConnectionStrings))
            {
                string sql = @"SELECT COUNT(1) FROM  AhphClients T1 INNER JOIN AhphClientGroup T2 ON T1.Id=T2.Id INNER JOIN AhphAuthGroup T3 ON T2.GroupId = T3.GroupId INNER JOIN AhphReRouteGroupAuth T4 ON T3.GroupId = T4.GroupId INNER JOIN AhphReRoute T5 ON T4.ReRouteId = T5.ReRouteId WHERE Enabled = 1 AND ClientId = @ClientId AND T5.InfoStatus = 1 AND UpstreamPathTemplate = @Path";
                var result= await connection.QueryFirstOrDefaultAsync<int>(sql, new { ClientId = clientid, Path = path });
                return result > 0 ? true : false;
            }
        }
    }
}
