using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SensorTestUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;



        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.


        }

        private void LaunchChange_Click(object sender, RoutedEventArgs e)
        {
            SimpleOrientationSensor.GetDefault().OrientationChanged += (s, a) =>
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, 
                () => textBlock.Text += PrintOrientation("orientationChanged", a.Orientation));

        }

        private SimpleOrientation _orientation;
        private void LaunchTimer_Click(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(1000) };
            timer.Tick += (s, a) =>
            {
                var currentOrientation = SimpleOrientationSensor.GetDefault().GetCurrentOrientation();
                if (currentOrientation == _orientation) 
                    return;

                _orientation = currentOrientation;
                textBlock.Text += PrintOrientation("timer", currentOrientation);
            };
            timer.Start();
        }

        private static string PrintOrientation(string text, SimpleOrientation currentOrientation)
        {
            return string.Format("\r\n[{2}] {1} orientation: {0}", currentOrientation,
                DateTime.Now.ToString("HH:mm:ss.ttt"),
                text);
        }

        private void LaunchWeb_Click(object sender, RoutedEventArgs e)
        {
            var webView = new WebView { Source = new Uri("http://www.microsoft.com") };
            webView.SetValue(Grid.ColumnProperty, 1);
            webView.SetValue(Grid.RowProperty, 1);
            mainGrid.Children.Add(webView);
        }
    }
}
