using System;

namespace ExceptionsInAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncVoidIsBad.CheckWhy();
            //AsyncTaskIsBetter.CheckWhy();
            //AsyncTaskIsBetterWithWait.CheckWhy();
            //AsyncTaskIsBetterWithAwait.CheckWhy();
            //AsyncTaskIsBetterWithHelper.CheckWhy();
        }
    }
}
 