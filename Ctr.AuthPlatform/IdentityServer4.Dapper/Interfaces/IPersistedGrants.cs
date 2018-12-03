using System;
using System.Threading.Tasks;

namespace IdentityServer4.Dapper.Interfaces
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-03
    /// 过期授权清理接口
    /// </summary>
    public interface IPersistedGrants
    {
        /// <summary>
        /// 移除指定时间的过期信息
        /// </summary>
        /// <param name="dt">过期时间</param>
        /// <returns></returns>
        Task RemoveExpireToken(DateTime dt);
    }
}
