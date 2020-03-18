using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeadlocksInWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task<string> PingServer(string url)
        {
            var response = await client.GetAsync(url);
            return response.StatusCode.ToString();
        }

        private Task DoSomeBackgroundWork()
        {
            var tasks = new Task[]{
                Task.Run(() => Thread.Sleep(TimeSpan.FromMilliseconds(250))),
                Task.Run(async () => await Task.Delay(TimeSpan.FromMilliseconds(250)))
            };
            return Task.WhenAll(tasks);
        }
        
        private async void AsyncAllTheWay_Click(object sender, RoutedEventArgs e)
        {
            responseCodeTb.Text = string.Empty;

            await DoSomeBackgroundWork(); // awaiting a task
            var statusCode = await PingServer(targetTB.Text); // awaiting on an async task

            responseCodeTb.Text = statusCode;
        }

        private void Deadlocking_Click(object sender, RoutedEventArgs e)
        {
            responseCodeTb.Text = string.Empty;

            DoSomeBackgroundWork().Wait(); // blocking on a task

            var statusCode = PingServer(targetTB.Text).Result; // blocking on a asynchronous task
            /*var t2 = PingServer(targetTB.Text);
            var statusCode = t2.Result;*/

            responseCodeTb.Text = statusCode;
        }

        private void Workaround_1_Click(object sender, RoutedEventArgs e)
        {
            responseCodeTb.Text = string.Empty;

            var url = targetTB.Text;
            var statusCode = Task.Run( () => PingServer(url) ).Result;

            //var statusCode = Task.Run( () => PingServer(targetTB.Text)).Result; // is anyting wrong with this?
            //var statusCode = Task.Run( async () => await PingServer(url) ).Result; // is anyting wrong with this?
            //var statusCode = Task.Run( () => PingServer(url).Result ).Result; // is anyting wrong with this?

            responseCodeTb.Text = statusCode;
        }

        private void Workaround_2_Click(object sender, RoutedEventArgs e)
        {
            responseCodeTb.Text = string.Empty;

            var url = targetTB.Text;
            PingServer(url).ContinueWith( t => {
                var statusCode = t.Result; 
                responseCodeTb.Text = statusCode;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Workaround_3_Click(object sender, RoutedEventArgs e)
        {
            async Task<string> PingServer(string url)
            {
                var response = await client.GetAsync(url)
                    .ConfigureAwait(continueOnCapturedContext: false);

                //responseCodeTb.Text = response.StatusCode.ToString(); // is anyting wrong with this?
                return response.StatusCode.ToString();
            }

            responseCodeTb.Text = string.Empty;

            var statusCode = PingServer(targetTB.Text).Result;

            responseCodeTb.Text = statusCode;
        }

        private void Workaround_4_Click(object sender, RoutedEventArgs e)
        {
            responseCodeTb.Text = string.Empty;

            var url = targetTB.Text;
            Task.Run(async () => {
                var statusCode = await PingServer(url);

                await Dispatcher.BeginInvoke((Action)(() => {
                    responseCodeTb.Text = statusCode;
                }));
            }); // fire and forget
        }
    }
}
