using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    public class AvoidFakeAsync
    {
        private readonly HttpClient client = new HttpClient();

        public async Task CheckAvailabilityOf(Uri[] targets)
        {
            Console.WriteLine($"Running test against {targets?.Length} targets");
            foreach (var target in targets)
            {
                var isOnline = await IsServerOk(target);
                if (!isOnline)
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

            var response = await Task.Run( () => GetStatusCodeFor(uri) );

            return response.Equals("OK");
        }

        private string GetStatusCodeFor(string url)
        {
            /* don't mind the `.Result` here, just want to have sync method */
            var response = client.GetAsync(url).Result;
            return response.StatusCode.ToString();
        }
    }
}
