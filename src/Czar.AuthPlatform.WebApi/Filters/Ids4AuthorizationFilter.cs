using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Czar.AuthPlatform.WebApi
{
    public class Ids4AuthAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// 认证服务器地址
        /// </summary>
        private string issUrl = "";
        /// <summary>
        /// 保护的API名称
        /// </summary>
        private string apiName = "";

        public Ids4AuthAttribute(string IssUrl,string ApiName)
        {
            issUrl = IssUrl;
            apiName = ApiName;
        }
        /// <summary>
        /// 重写验证方式
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                var access_token = actionContext.Request.Headers.Authorization?.Parameter; //获取请求的access_token
                if (String.IsNullOrEmpty(access_token))
                {//401
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    actionContext.Response.Content = new StringContent("{\"errcode\":401,\"errmsg\":\"未授权\"}");
                }
                else
                {//开始验证请求的Token是否合法
                    //1、获取公钥
                    var httpclient = new HttpClient();
                    var jwtKey= httpclient.GetStringAsync(issUrl + "/.well-known/openid-configuration/jwks").Result;
                    var Ids4keys = JsonConvert.DeserializeObject<Ids4Keys>(jwtKey);
                    var jwk = Ids4keys.keys;
                    var parameters = new TokenValidationParameters
                    { //可以增加自定义的验证项目
                        ValidIssuer = issUrl,
                        IssuerSigningKeys = jwk ,
                        ValidateLifetime = true,
                        ValidAudience = apiName
                    };
                    var handler = new JwtSecurityTokenHandler();
                    //2、使用公钥校验是否合法,如果验证失败会抛出异常
                    var id = handler.ValidateToken(access_token, parameters, out var _);
                    //设置access_token相关内容，可自行保存
                    actionContext.RequestContext.Principal = id;
                }
            }
            catch(Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Content = new StringContent("{\"errcode\":401,\"errmsg\":\"未授权\"}");
            }
        }
    }

    public class Ids4Keys
    {
        public JsonWebKey[] keys { get; set; }
    }
}