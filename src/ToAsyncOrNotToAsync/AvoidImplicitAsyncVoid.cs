using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    public static class AvoidImplicitAsyncVoid
    {
        public static void DoSomething()
        {
            FireAndForget(async () => await Task.Delay(100) );
        }

        private static void FireAndForget(Action act)
        {
            // without second overload the async lambda would normally end up here
            act();
        }

        private static async void FireAndForget(Func<Task> act)
        {
            try 
            { 
                await act();
            } 
            catch(Exception e)
            {
                // log the exception
            }
        }
    }
}
