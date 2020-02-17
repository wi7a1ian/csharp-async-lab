using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExceptionsInAsync
{
    public static class AsyncVoidIsBad
    {
        public static void CheckWhy()
        {
            try
            {
                Console.WriteLine("All right, let's do this!");
                CantCatchMeAsync();
                //Thread.Sleep(TimeSpan.FromSeconds(1)); //check this out!
                Console.WriteLine("Where did that exception go?");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async void CantCatchMeAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(50));
            throw new Exception("Bazinga!");
        }
    }
}
