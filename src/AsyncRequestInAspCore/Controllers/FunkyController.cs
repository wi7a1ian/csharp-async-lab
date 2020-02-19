using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsyncRequestInAspCore.Controllers
{
    [ApiController]
    public class FunkyController : ControllerBase
    {
        private readonly IHttpClientFactory clientFactory;
        private const string targetUrl = "https://google.com";

        public FunkyController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        private async Task<string> PingServer(CancellationToken ct)
        {
            var client = clientFactory.CreateClient();
            var response = await client.GetAsync(targetUrl, ct);
            return response.StatusCode.ToString();
        }

        [HttpGet]
        [Route("/api/async")]
        public async Task<IActionResult> Async(CancellationToken ct = default) => Ok(await PingServer(ct));

        [HttpGet("/api/blocking")]
        public IActionResult Blocking(CancellationToken ct = default)
        {
            var client = clientFactory.CreateClient();
            return Ok(client.GetAsync(targetUrl, ct).Result.StatusCode.ToString());
        }

        [HttpGet("/api/no-deadlock")]
        public IActionResult NoDeadlock(CancellationToken ct = default) => Ok(PingServer(ct).Result);
    }
}