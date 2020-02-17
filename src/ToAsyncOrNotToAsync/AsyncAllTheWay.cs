using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    public sealed class AsyncAllTheWay
    {
        private readonly HttpClient client = new HttpClient();

        public async Task CheckAvailabilityOf(Uri[] targets)
        {
            Console.WriteLine($"Running test against {targets?.Length} targets");
            foreach(var target in targets)
            {
                var isOnline = await IsServerOk(target);
                if(!isOnline)
                {
                    throw new Exception("Blarg");
                }
            }
            Console.WriteLine("Cooling down...");
            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine("Completed!");
        }

        private async Task<bool> IsServerOk(Uri targetServer)
        {
            var uri = targetServer.AbsoluteUri;
            await GetStatusCodeFor(uri);
            await GetStatusCodeFor(uri);
            var response = await GetStatusCodeFor(uri);
            return response.Equals("OK");
        }

        private async Task<string> GetStatusCodeFor(string url)
        {
            var response = await client.GetAsync(url);
            return response.StatusCode.ToString();
        }
    }
}
