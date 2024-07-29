
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Recoder
{
    public class ScreeenCaptureHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();//获取桌面窗口的句柄，Intptr表明返回类型为指针类型

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);//获取指定窗口设备上下文的句柄

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);//创建一个与指定设备上下文所兼容的内存设备上下文

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc,IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest,int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        private static extern IntPtr DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        private static extern bool ReleaseDC(IntPtr  hwnd,IntPtr hDC);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("gdi32.dll")]
        private static extern int DeleteDC(IntPtr hdc);

        public async Task CaptureScreenAsync()
        {
            var desktopWnd = GetDesktopWindow();
            var desktopDC = GetWindowDC(desktopWnd);
            var memoryDC = CreateCompatibleDC(desktopDC);

            var width = GetSystemMetrics(0);
            var height = GetSystemMetrics(1);

            var bitmap = CreateCompatibleBitmap(desktopDC, width, height);
            var oldmap = SelectObject(memoryDC, bitmap);

            BitBlt(memoryDC, 0, 0, width, height, desktopDC, 0, 0, 0x00CC0020);

            SelectObject(memoryDC, oldmap);
            DeleteDC(memoryDC);
            ReleaseDC(desktopWnd, desktopDC);

            var bmp = Image.FromHbitmap(bitmap);//需要,装包
            DeleteObject(bitmap);

            var directoryPath = @"D:\Recoder";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Debug.WriteLine("create path");
            }

            var filePath = Path.Combine(directoryPath, $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            bmp.Save(filePath,ImageFormat.Png);
        }
    }

}
