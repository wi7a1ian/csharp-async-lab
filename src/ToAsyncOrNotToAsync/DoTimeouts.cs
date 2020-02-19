using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    public static class DoTimeouts
    {
        public static async Task CheckAvailabilityOf(Uri target)
        {
            Console.WriteLine($"Target server response: {await IsServerOk(target)}");
        }

        private static async Task<string> IsServerOk(Uri targetServer)
        {
            var url = targetServer.AbsoluteUri;
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            return await GetStatusCodeFor(url, cts.Token);
        }

        private static async Task<string> GetStatusCodeFor(string url, CancellationToken ct)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url, ct);
            return response.StatusCode.ToString();
        }
    }
}
