using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ctr.AhphOcelot
{
    public static class AhphOcelotHelper
    {
        /// <summary>
        /// 获取加密后缓存的KEY
        /// </summary>
        /// <param name="CounterPrefix">缓存前缀，防止重复</param>
        /// <param name="ClientId">客户端ID</param>
        /// <param name="Period">限流策略，如 1s 2m 3h 4d</param>
        /// <param name="Path">限流地址,可使用通配符</param>
        /// <returns></returns>
        public static string ComputeCounterKey(string CounterPrefix, string ClientId, string Period, string Path)
        {
            var key = $"{CounterPrefix}_{ClientId}_{Period}_{Path}";
            var idBytes = Encoding.UTF8.GetBytes(key);
            byte[] hashBytes;
            using (var algorithm = SHA1.Create())
            {
                hashBytes = algorithm.ComputeHash(idBytes);
            }
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }
    }
}
