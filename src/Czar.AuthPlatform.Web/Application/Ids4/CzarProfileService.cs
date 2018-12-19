using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czar.AuthPlatform.Web.Application.Ids4
{
    public class CzarProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //把用户返回的Claims应用到返回
            context.IssuedClaims = context.Subject.Claims.ToList();
            return Task.CompletedTask;
        }

        /// <summary>
        /// 验证用户是否有效
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
