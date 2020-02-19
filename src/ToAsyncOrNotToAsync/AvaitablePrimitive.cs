using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    public static class AvaitablePrimitive
    {
        public static async Task DoSomething()
        {
            await 3000;
        }


        public static TaskAwaiter GetAwaiter(this int ms) => Task.Delay(ms).GetAwaiter();
    }
}
