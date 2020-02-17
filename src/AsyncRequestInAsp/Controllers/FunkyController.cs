using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AsyncRequestInAsp.Controllers
{
    public class FunkyController : ApiController
    {
        private readonly HttpClient client = new HttpClient() { BaseAddress = new Uri("https://google.com") };

        private async Task<string> PingServer()
        {
            var response = await client.GetAsync("/");
            return response.StatusCode.ToString();
        }

        [HttpGet]
        [Route("api/async")]
        public async Task<IHttpActionResult> Async() => Ok(await PingServer());

        [HttpGet]
        [Route("api/blocking")]
        public IHttpActionResult Blocking() => Ok(client.GetAsync("/").Result.StatusCode);

        [HttpGet]
        [Route("api/deadlock")]
        public IHttpActionResult Deadlock() => Ok(PingServer().Result);

        [HttpGet]
        [Route("api/deadlock-getawaiter")]
        public IHttpActionResult DeadlockGetAwaiter() => Ok(PingServer().GetAwaiter().GetResult());

        [HttpGet]
        [Route("api/safe-configawait")]
        public IHttpActionResult SafeWithConfigureAwait()
        {
            async Task<string> PingServer()
            {
                var response = await client.GetAsync("/").ConfigureAwait(false);
                return response.StatusCode.ToString();
            }

            return Ok(PingServer().Result);
        }

        [HttpGet]
        [Route("api/safe-task-run")]
        public IHttpActionResult SafeWithTaskRun()
        {
            return Ok(Task.Run(PingServer).Result);
        }

    }
}
