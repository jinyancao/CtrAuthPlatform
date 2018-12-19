using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Czar.AuthPlatform.Core.Helpers
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-18
    /// 加密相关帮助类
    /// </summary>
    public class SecretHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string ToMD5(string str)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }
    }
}
