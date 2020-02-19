using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncEventsInWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Current.DispatcherUnhandledException += (sender, e) =>
            {
                // log unhandled exceptions thrown from within main UI thread
                e.Handled = true; // Prevent default unhandled exception processing
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                // log unhandled exceptions thrown from background threads
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                // log fire-n-forget tasks that got GCed
            };
        }
    }
}
