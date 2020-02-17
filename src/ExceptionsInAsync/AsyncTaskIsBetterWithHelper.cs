using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExceptionsInAsync
{
    public static class AsyncTaskIsBetterWithHelper
    {
        public static void CheckWhy()
        {
            Console.WriteLine("All right, let's do this!");
            CantCatchMeAsync().FireAndForget( e => 
            {
                Console.WriteLine("Oh, here you are.");
                Console.WriteLine(e.Message);
            });
            Console.WriteLine("Where did that exception go?");
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        private static async Task CantCatchMeAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(50));
            throw new Exception("Bazinga!");
        }

        static async void FireAndForget(this Task t, Action<Exception> onException)
        {
            try
            {
                await t;
            }
            catch(Exception e)
            {
                onException(e);
            }
        }
    }
}
