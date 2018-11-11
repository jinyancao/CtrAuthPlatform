using System;
using System.Collections.Generic;
using System.Text;

namespace Ctr.AhphOcelot.Configuration
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-11
    /// 自定义配置信息
    /// </summary>
    public class AhphOcelotConfiguration
    {
        /// <summary>
        /// 数据库连接字符串，使用不同数据库时自行修改,默认实现了SQLSERVER
        /// </summary>
        public string DbConnectionStrings { get; set; }
    }
}
