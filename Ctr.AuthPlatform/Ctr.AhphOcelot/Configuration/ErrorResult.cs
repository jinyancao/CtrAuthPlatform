using System;
using System.Collections.Generic;
using System.Text;

namespace Ctr.AhphOcelot.Configuration
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-15
    /// 统一的异常显示结构
    /// </summary>
    public class ErrorResult
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string errmsg { get; set; }
    }
}
