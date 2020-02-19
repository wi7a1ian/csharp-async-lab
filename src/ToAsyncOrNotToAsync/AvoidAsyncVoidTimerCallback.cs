using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    public static class AvoidAsyncVoidTimerCallback
    {
        public static void StartPing()
        {
            var httpClient = new HttpClient{ BaseAddress = new Uri("http://google.com") };
            var pinger = new UnsafePinger(httpClient);
            //var pinger = new SafePinger(httpClient);
        }
    }

    public class UnsafePinger
    {
        readonly Timer timer;
        readonly HttpClient client;

        public UnsafePinger(HttpClient client)
        {
            this.client = client;
            timer = new Timer(Heartbeat, null, 1000, 1000);
        }
        public async void Heartbeat(object state)
        {
            // exception thrown from here will kill the app
            //throw new Exception("Bazinga!");

            var response = await client.GetAsync("/");
            Console.WriteLine(response.StatusCode);
        }
    }

    public class SafePinger
    {
        readonly Timer timer;
        readonly HttpClient client;

        public SafePinger(HttpClient client)
        {
            this.client = client;
            timer = new Timer(Heartbeat, null, 1000, 1000);
        }
        public void Heartbeat(object state)
        {
            _ = DoAsyncPing();
        }
        private async Task DoAsyncPing()
        {
            // exception thrown from here won't kill the app
            //throw new Exception("Bazinga!");

            var response = await client.GetAsync("/"); 
            Console.WriteLine(response.StatusCode);
        }
    }
}
