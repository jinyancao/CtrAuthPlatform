using Ocelot.Errors;
using Ocelot.Responder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctr.AhphOcelot.Responder
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-19
    /// 错误信息转换成输出状态码
    /// </summary>
    public class AhphErrorsToHttpStatusCodeMapper : IErrorsToHttpStatusCodeMapper
    {
        public int Map(List<Error> errors)
        {
            if (errors.Any(e => e.Code == OcelotErrorCode.UnauthenticatedError))
            {
                return 401;
            }

            if (errors.Any(e => e.Code == OcelotErrorCode.UnauthorizedError
                || e.Code == OcelotErrorCode.ClaimValueNotAuthorisedError
                || e.Code == OcelotErrorCode.ScopeNotAuthorisedError
                || e.Code == OcelotErrorCode.UserDoesNotHaveClaimError
                || e.Code == OcelotErrorCode.CannotFindClaimError))
            {
                return 403;
            }

            if (errors.Any(e => e.Code == OcelotErrorCode.RequestTimedOutError))
            {
                return 503;
            }

            if (errors.Any(e => e.Code == OcelotErrorCode.UnableToFindDownstreamRouteError))
            {
                return 404;
            }

            if (errors.Any(e => e.Code == OcelotErrorCode.UnableToCompleteRequestError))
            {
                return 500;
            }

            if (errors.Any(e => e.Code == OcelotErrorCode.RateLimitOptionsError))
            {
                return 429;
            }

            return 404;
        }
    }
}
