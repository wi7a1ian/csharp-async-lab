using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    public class AvoidReturnAwait
    {
        public async Task CheckAvailabilityOf(Uri target)
        {
            Console.WriteLine($"Target server response: {await IsServerOk(target)}");
        }

        private /*async*/ Task<string> IsServerOk(Uri targetServer)
        {
            var url = targetServer.AbsoluteUri;
            return GetStatusCodeFor(url);
            //return await GetStatusCodeFor(url); // no need for
        }

        private async Task<string> GetStatusCodeFor(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            return response.StatusCode.ToString();
        }
    }
}
