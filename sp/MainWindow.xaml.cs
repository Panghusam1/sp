using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace sp
{
    public sealed partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private ScreenCaptureHelper _captureHelper;

        public MainWindow()
        {
            this.InitializeComponent();
            _captureHelper = new ScreenCaptureHelper();
            StartCapture();
            GlobalHookHelper.Start();
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, WindowEventArgs e)
        {
            GlobalHookHelper.Stop();
        }

        private void StartCapture()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += async (s, e) => await CaptureScreen();
            _timer.Start();
        }

        private async Task CaptureScreen()
        {
            await _captureHelper.CaptureScreenAsync();
        }
    }
}
