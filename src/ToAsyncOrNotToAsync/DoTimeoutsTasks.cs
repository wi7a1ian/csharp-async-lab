using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    public static class DoTimeoutsTasks
    {
        public static async Task CheckAvailabilityOf(Uri target)
        {
            Console.WriteLine($"Target server response: {await IsServerOk(target)}");
        }

        private static async Task<string> IsServerOk(Uri targetServer)
        {
            var url = targetServer.AbsoluteUri;
            return await GetStatusCodeFor(url).TimeoutAfter(TimeSpan.FromSeconds(10));
            //return await GetStatusCodeFor(url).TimeoutAfterWithCts(TimeSpan.FromSeconds(10));
        }

        private static async Task<string> GetStatusCodeFor(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            return response.StatusCode.ToString();
        }

        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
        {
            var delayTask = Task.Delay(timeout);
            var resultTask = await Task.WhenAny(task, delayTask);
            if (resultTask == delayTask)
            {
                throw new OperationCanceledException();
            }
            return await task;
        }

        public static async Task<T> TimeoutAfterWithCts<T>(this Task<T> task, TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource())
            {
                var delayTask = Task.Delay(timeout, cts.Token);
                var resultTask = await Task.WhenAny(task, delayTask);
                if (resultTask == delayTask)
                {
                    throw new OperationCanceledException();
                }
                else
                {
                    cts.Cancel();
                }
                return await task;
            }
        }

    }
}
