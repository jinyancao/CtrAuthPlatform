using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czar.AuthPlatform.Web.Infrastructure.Config
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class CzarConfig
    {
        /// <summary>
        /// 网关授权地址
        /// </summary>
        public string PublicOrigin { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DbConnectionStrings { get; set; }
    }
}
