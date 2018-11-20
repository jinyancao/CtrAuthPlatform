using Ocelot.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ctr.AhphOcelot.Errors
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-18
    /// 限流错误信息
    /// </summary>
    public class RateLimitOptionsError:Error
    {
        public RateLimitOptionsError(string message) : base(message, OcelotErrorCode.RateLimitOptionsError)
        {

        }
    }
}
