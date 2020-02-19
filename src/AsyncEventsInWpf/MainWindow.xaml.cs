using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

namespace AsyncEventsInWpf
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

        private void Blocking_Click(object sender, RoutedEventArgs e)
        {
            responseCodeTb.Text = string.Empty;

            var response = client.GetAsync(targetTB.Text).Result;

            responseCodeTb.Text = response.StatusCode.ToString();
        }

        private async void Async_Click(object sender, RoutedEventArgs e)
        {
            responseCodeTb.Text = string.Empty;

            var response = await client.GetAsync(targetTB.Text);

            responseCodeTb.Text = response.StatusCode.ToString();
        }

        private void GetAwaiter_Click(object sender, RoutedEventArgs e)
        {
            responseCodeTb.Text = string.Empty;

            var response = client.GetAsync(targetTB.Text).GetAwaiter().GetResult();

            responseCodeTb.Text = response.StatusCode.ToString();
        }

        private void FireNForget_Click(object sender, RoutedEventArgs e)
        {
            responseCodeTb.Text = string.Empty;
            var targetServer = targetTB.Text;

            Task.Run( async () => {
                var response = await client.GetAsync(targetServer);
                return response.StatusCode.ToString();
            }).ContinueWith( t => {
                responseCodeTb.Text = t.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void AsyncException_Click(object sender, RoutedEventArgs e)
        {
            // leave a brk point in DispatcherUnhandledException event handler
            throw new NotImplementedException("Bazinga");
        }

    }
}
