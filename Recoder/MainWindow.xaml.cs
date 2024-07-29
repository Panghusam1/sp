using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Recoder
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private ScreeenCaptureHelper _captureHelper;
        public MainWindow()
        {
            this.InitializeComponent();
            _captureHelper = new ScreeenCaptureHelper();
            StartCapture();
            GlobalHookHelper.Start();
            Debug.WriteLine("open");
            this.Closed += MainWindow_Closed;
        }
        private void MainWindow_Closed(object sender,WindowEventArgs e)
        {
            GlobalHookHelper.Stop();
        }
        private void StartCapture()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += async (s, e) =>
            {
                await CaptureScreen();
            };
            _timer.Start();
        }
        private async Task CaptureScreen()
        {
            await _captureHelper.CaptureScreenAsync();
        }
    }
}
