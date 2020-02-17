using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExceptionsInAsync
{
    public static class AsyncTaskIsBetterWithWait
    {
        public static void CheckWhy()
        {
            try
            {
                Console.WriteLine("All right, let's do this!");
                CantCatchMeAsync().Wait();
                //CantCatchMeAsync().GetAwaiter().GetResult(); // check difference in stack trace
                Console.WriteLine("Where did that exception go?");
            }
            catch (AggregateException e)
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
