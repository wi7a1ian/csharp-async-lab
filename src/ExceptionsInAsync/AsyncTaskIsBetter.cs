using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExceptionsInAsync
{
    public static class AsyncTaskIsBetter
    {
        public static void CheckWhy()
        {
            try
            {
                Console.WriteLine("All right, let's do this!");
                CantCatchMeAsync();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Console.WriteLine("Where did that exception go?");
            }
            catch (Exception e)
            {
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
