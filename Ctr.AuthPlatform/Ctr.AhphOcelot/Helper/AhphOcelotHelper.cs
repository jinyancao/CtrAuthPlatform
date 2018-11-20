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

        /// <summary>
        /// 金焰的世界
        /// 2018-11-19
        /// 根据限流标识，获取周期秒数
        /// </summary>
        /// <param name="timeSpan">标识</param>
        /// <returns></returns>
        public static int ConvertToSecond(string timeSpan)
        {
            var l = timeSpan.Length - 1;
            var value = timeSpan.Substring(0, l);
            var type = timeSpan.Substring(l, 1);

            switch (type)
            {
                case "d":
                    return Convert.ToInt32(double.Parse(value) * 24 * 3600);
                case "h":
                    return Convert.ToInt32(double.Parse(value) * 3600);
                case "m":
                    return Convert.ToInt32(double.Parse(value) * 60);
                case "s":
                    return Convert.ToInt32(value);
                default:
                    throw new FormatException($"{timeSpan} can't be converted to TimeSpan, unknown type {type}");
            }
        }
    }
}
