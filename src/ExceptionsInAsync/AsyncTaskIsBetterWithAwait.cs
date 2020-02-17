using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExceptionsInAsync
{
    public static class AsyncTaskIsBetterWithAwait
    {
        public static void CheckWhy() => CheckWhyAsync().Wait();

        public static async Task CheckWhyAsync()

        {
            try
            {
                Console.WriteLine("All right, let's do this!");
                await CantCatchMeAsync();
                Console.WriteLine("Where did that exception go?");
            }
            catch (Exception e) when (!(e is AggregateException)) 
            {
                Console.WriteLine("Oh, here you are.");
                Console.WriteLine(e.Message);
            }
        }

        private static async Task CantCatchMeAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(50));
            throw new Exception("Bazinga!");
        }
    }
}
