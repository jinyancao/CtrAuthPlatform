using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;
namespace Czar.AuthPlatform.UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            #region 验证JWT
            var jwks = "{\"kty\":\"RSA\",\"use\":\"sig\",\"kid\":\"518944b70ec6a75ce85a1757e6412125\",\"e\":\"AQAB\",\"n\":\"xb5gf45mzaUkpYgD5e1YNrZomGfPrkLtMbUa53o36U7HbJdOXxuJ9TEcEIDKA1wRhqTSizmHoc2KrBhRj_LzhOysys0iQQoEBsLla8iiTIPIOBRRJtvognG7hYuTP04waVayL0O5UXjwllUl - WUkAWEMDfDS9OfbbBFQhCsfxU6Fp2kVpuR3tR_4OeOYqhk8CcPndLV7XLHe8mzKpZ8o9sTQ6QlBnsU_fUzTzz6TPoN2hFM9IxwAv5LCuVAPhXNp5f2SxXQLlJq6kjgZAgNbQ6UUpEG - doElE58kU5Fg_4Y0KqFkQo2sr98EIQ1cCzpfQav_tJ7BMZ67lulVWTPiDQ\",\"alg\":\"RS256\"}";
            var jwk= JsonConvert.DeserializeObject<JsonWebKey>(jwks);
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = "http://localhost:7777",
                IssuerSigningKeys = new[] { jwk },
                ValidateLifetime = true,
                ValidAudience = "mpc_gateway"
            };
            var handler = new JwtSecurityTokenHandler();
            string jwt = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjUxODk0NGI3MGVjNmE3NWNlODVhMTc1N2U2NDEyMTI1IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NDU5MTkwNTgsImV4cCI6MTU0NTkyMjY1OCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3Nzc3IiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6Nzc3Ny9yZXNvdXJjZXMiLCJtcGNfZ2F0ZXdheSJdLCJjbGllbnRfaWQiOiJjbGllbnRhIiwic3ViIjoiMSIsImF1dGhfdGltZSI6MTU0NTkxOTA1OCwiaWRwIjoibG9jYWwiLCJuaWNrbmFtZSI6IumHkeeEsOeahOS4lueVjCIsImVtYWlsIjoiNTQxODY5NTQ0QHFxLmNvbSIsIm1vYmlsZSI6IjEzODg4ODg4ODg4Iiwic2NvcGUiOlsibXBjX2dhdGV3YXkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicHdkIl19.DXWaeHu9SWA_wdycZJgWK0sW9WmanYlTNR-Ipmwockhk2kBzASxn-PVZbIdykv3R9Sr7NqOV0DW6_oZ-WpdhAh3JcbqI-1T94_16ronHNAo_yeWHWAFk6UH5Ai7GyMIQS0zR2BPtgHgxnaH5Ae1e8eCdS0_uSJeqVrLBFGiIPAJl6xbq78Mqt4h37rZZqoXgdGp2imaiO_uEOicKkcgrya2TQQc6rZrnii7sO6SnhykdYOcyV9GfFVYo5Y_2gh4Z7FThi2MUq5F9F6cXHwJ3meIIDqrd4F9wql8ndM2F1ehAJv4TkJK1kTTQuGsKxWLG_T-7pxJ3UrtoPkTMaL_ilw";
            try
            {
                var id = handler.ValidateToken(jwt, parameters, out var ad);
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            #endregion
            return new string[] { ".NetCore values"};

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return id + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
