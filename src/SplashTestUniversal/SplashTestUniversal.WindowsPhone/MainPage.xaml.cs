using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SplashTestUniversal
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

            //var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

            _image.ImageOpened += _image_ImageOpened;
            _image.ImageFailed += _image_ImageFailed;
            Debug.WriteLine("before ctor wait");
            _imageLock.Wait();
            Debug.WriteLine("after ctor wait");


            var image = new BitmapImage();
            var fileTask = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/WindowsSplashCopy.png"));
            var file = fileTask.GetAwaiter().GetResult();
            var stream = file.OpenStreamForReadAsync().Result;
            image.SetSource(stream.AsRandomAccessStream());
            _image.Source = image;

            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("loaded");
        }

        
        void _image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Debug.WriteLine("image failed");

        }

        void _image_ImageOpened(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("releasing");
            _imageLock.Release();
        }
        
        SemaphoreSlim _imageLock = new SemaphoreSlim(1);
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("onNaviTo");
            await Task.Delay(5000);
            Debug.WriteLine("navi waiting");
            await _imageLock.WaitAsync();
            Debug.WriteLine("navi after wait");
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
    }
}
