using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Czar.AuthPlatform.Web.Infrastructure.Models
{
    /// <summary>
    /// 授权用户信息
    /// </summary>
    public class CzarUsers
    {
        public CzarUsers() { }

        public int Uid { get; set; }
        public string uAccount { get; set; }
        public string uPassword { get; set; }
        public string uNickName { get; set; }
        public string uMobile { get; set; }
        public string uEmail { get; set; }
        public string uStatus { get; set; }

        public List<Claim> Claims
        {
            get
            {
                return new List<Claim>() {
                    new Claim("nickname",uNickName??""),
                    new Claim("email",uEmail??""),
                    new Claim("mobile",uMobile??"")
                };
            }
        }
    }
}
