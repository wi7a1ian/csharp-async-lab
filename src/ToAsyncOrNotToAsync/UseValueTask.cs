using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    static class UseValueTask
    {
        public static Task<int> BadAddAsync(int a, int b)
        {
            return Task.Run(() => a + b);
        }

        public static Task<int> BetterAddAsync(int a, int b)
        {
            return Task.FromResult(a + b); // Task.FromResult for "known" data
            // Task is a reference type (=> on heap)
        }

        public static ValueTask<int> BestAddAsync(int a, int b)
        {
            return new ValueTask<int>(a + b);
        }

        public static async ValueTask<int> MaybeAsync()
        {
            var rngValue = new Random().Next(0, 6);

            //Use ValueTask<T> anytime method might not hit `await` most of the time
            if (rngValue == 0)
            { 
                await Task.Delay(1000);
            }

            return rngValue;
        }
    }
}
