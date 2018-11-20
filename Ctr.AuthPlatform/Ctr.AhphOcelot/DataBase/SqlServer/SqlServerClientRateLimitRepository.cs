using Ctr.AhphOcelot.Configuration;
using Ctr.AhphOcelot.RateLimit;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Ctr.AhphOcelot.DataBase.SqlServer
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-19
    /// 客户端限流信息提取
    /// </summary>
    public class SqlServerClientRateLimitRepository : IClientRateLimitRepository
    {
        private readonly AhphOcelotConfiguration _option;
        public SqlServerClientRateLimitRepository(AhphOcelotConfiguration option)
        {
            _option = option;
        }

        /// <summary>
        /// 校验客户端限流规则
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        public async Task<(bool RateLimit, List<AhphClientRateLimitOptions> rateLimitOptions)> CheckClientRateLimitAsync(string clientid, string path)
        {
            using (var connection = new SqlConnection(_option.DbConnectionStrings))
            {
                string sql = @"SELECT DISTINCT UpstreamPathTemplate AS RateLimitPath,LimitPeriod AS Period,LimitNum AS Limit,ClientId FROM AhphReRoute T1 INNER JOIN AhphReRouteLimitRule T2 ON T1.ReRouteId=T2.ReRouteId
                INNER JOIN AhphLimitRule T3 ON T2.RuleId=T3.RuleId INNER JOIN AhphLimitGroupRule T4 ON
                T2.ReRouteLimitId=T4.ReRouteLimitId INNER JOIN AhphLimitGroup T5 ON T4.LimitGroupId=T5.LimitGroupId
                INNER JOIN AhphClientLimitGroup T6 ON T5.LimitGroupId=T6.LimitGroupId INNER JOIN
                AhphClients T7 ON T6.Id=T7.Id
                WHERE T1.InfoStatus=1 AND T1.UpstreamPathTemplate=@path AND T3.InfoStatus=1 AND T5.InfoStatus=1
                AND ClientId=@clientid AND Enabled=1";
                var result = (await connection.QueryAsync<AhphClientRateLimitOptions>(sql, new { clientid, path }))?.AsList();
                if (result != null && result.Count > 0)
                {
                    return (true, result);
                }
                else
                {
                    return (false, null);
                }
            }
        }

        /// <summary>
        /// 校验是否设置了路由白名单
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        public async Task<bool> CheckClientReRouteWhiteListAsync(string clientid, string path)
        {
            using (var connection = new SqlConnection(_option.DbConnectionStrings))
            {
                string sql = @"SELECT COUNT(1) FROM AhphReRoute T1 INNER JOIN AhphClientReRouteWhiteList T2 ON T1.ReRouteId=T2.ReRouteId
                            INNER JOIN AhphClients T3 ON T2.Id=T3.Id WHERE T1.InfoStatus=1 AND UpstreamPathTemplate=@path AND
                            ClientId=@clientid AND Enabled=1";
                var result = await connection.QueryFirstOrDefaultAsync<int>(sql, new { clientid,path });
                return result > 0;
            }
        }

        /// <summary>
        /// 校验是否启用限流规则
        /// </summary>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        public async Task<bool> CheckReRouteRuleAsync(string path)
        {
            using (var connection = new SqlConnection(_option.DbConnectionStrings))
            {
                string sql = @"SELECT COUNT(1) FROM AhphReRoute T1 INNER JOIN AhphReRouteLimitRule T2 ON T1.ReRouteId=T2.ReRouteId
                        INNER JOIN AhphLimitRule T3 ON T2.RuleId=T3.RuleId WHERE T1.InfoStatus=1 AND UpstreamPathTemplate=@path
                        AND T3.InfoStatus=1";
                var result = await connection.QueryFirstOrDefaultAsync<int>(sql, new { path });
                return result > 0;
            }
        }
    }
}
