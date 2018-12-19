using Czar.AuthPlatform.Web.Application.IServices;
using IdentityModel;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czar.AuthPlatform.Web.Application.Ids4
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-18
    /// 自定义用户名密码校验
    /// </summary>
    public class CzarResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ICzarUsersServices _czarUsersServices;
        public CzarResourceOwnerPasswordValidator(ICzarUsersServices czarUsersServices)
        {
            _czarUsersServices = czarUsersServices;
        }
        /// <summary>
        /// 验证用户身份
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = _czarUsersServices.FindUserByuAccount(context.UserName, context.Password);
            if (user != null)
            {
                context.Result = new GrantValidationResult(
                    user.Uid.ToString(),
                    OidcConstants.AuthenticationMethods.Password, 
                    DateTime.UtcNow,
                    user.Claims);
            }
            return Task.CompletedTask;
        }
    }
}
