using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ctr.AhphOcelot.Authentication
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-15
    /// 客户端授权仓储接口
    /// </summary>
    public interface IClientAuthenticationRepository
    {
        /// <summary>
        /// 校验当前的请求地址是否有权限访问
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        Task<bool> ClientAuthenticationAsync(string clientid, string path);
    }
}
