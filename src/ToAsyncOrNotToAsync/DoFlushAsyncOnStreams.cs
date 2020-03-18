using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    public static class DoFlushAsyncOnStreams
    {
        public static async Task GreetFileAsync(string filename)
        {
            await using (var sw = new StreamWriter(filename))
            {
                await sw.WriteAsync("Hello World"); // written to a buffer
                await sw.FlushAsync(); // flushed
            }

            await using (var sw = new StreamWriter(filename))
            {
                await sw.WriteAsync("Hello World"); // written to a buffer
            } // flushed
        }
    }
}
