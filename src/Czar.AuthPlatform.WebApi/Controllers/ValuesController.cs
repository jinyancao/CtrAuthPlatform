using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Czar.AuthPlatform.WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        [Ids4Auth("http://localhost:6611", "mpc_gateway")]
        public IEnumerable<string> Get()
        {
            var Context = RequestContext.Principal; 
            return new string[] { "WebApi Values" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
