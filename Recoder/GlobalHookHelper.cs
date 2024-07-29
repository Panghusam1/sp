using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


namespace Recoder
{
    public class GlobalHookHelper
    {
        private static StreamWriter _writer;
        public static void Start()
        {
            _keyboardHookID = SetKeyboardHook(_keyboardProc);
            _mouseHookID = SetMouseHook(_mouseProc);

            var directoryPath = @"D:\Recoder";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            Debug.WriteLine("start to record");
            _writer = new StreamWriter(Path.Combine(directoryPath, "Key_mouse_events.csv"), true, Encoding.UTF8)
            {
                AutoFlush = true
            };
            _writer.WriteLine("EventType,Timestamp,KeyCode,KeyChar,Button,X,Y");
        }
        public static void Stop()
        {
            UnhookWindowsHookEx(_keyboardHookID);
            UnhookWindowsHookEx(_mouseHookID);
            _writer?.Close();
        } 
        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONUP = 0x0205;

        //键盘监听
        private delegate IntPtr LowLevelKeyboardProc(int Code, IntPtr wParam, IntPtr lParam);
        private static LowLevelKeyboardProc _keyboardProc = KeyboardHookCallback;
        private static IntPtr _keyboardHookID = IntPtr.Zero;
        private static IntPtr SetKeyboardHook(LowLevelKeyboardProc proc)//设置键盘钩子
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private static IntPtr KeyboardHookCallback(int nCode,IntPtr wParam,IntPtr lparam)
        {
            if (nCode >= 0&& (wParam == (IntPtr)WM_KEYDOWN)||(wParam == (IntPtr)WM_KEYUP))
            {
                int vkCode = Marshal.ReadInt32(lparam);//using System.Runtime.InteropServices;
                string eventType = wParam == (IntPtr)WM_KEYDOWN ? "KeyDown" : "KeyUp";
                string timestamp = System.DateTime.Now.ToString("o");
                string keyChar = ((Keys)vkCode).ToString();
                _writer.WriteLine($"{eventType},{timestamp},{vkCode},{keyChar},,,");
            }
            return CallNextHookEx(_keyboardHookID, nCode, wParam, lparam);
        }
        //鼠标监听
        //定义低级鼠标钩子委托
        private delegate IntPtr LowLevelMouseProc(int Code, IntPtr wParam, IntPtr lParam);
        //一个静态字段，保存指向 MouseHookCallback 方法的委托实例。
        private static LowLevelMouseProc _mouseProc = MouseHookCallback;
        //一个静态字段，用于存储鼠标钩子的句柄。
        private static IntPtr _mouseHookID = IntPtr.Zero;
        //设置钩子的方法
        private static IntPtr SetMouseHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        //钩子回调函数
        private static IntPtr MouseHookCallback(int nCode,IntPtr wParam,IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_LBUTTONDOWN || wParam == (IntPtr)WM_RBUTTONDOWN || wParam == (IntPtr)WM_LBUTTONUP || wParam == (IntPtr)WM_RBUTTONUP))
            {
                MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                string eventType = wParam == (IntPtr)WM_LBUTTONDOWN ? "MouseDown" : wParam == (IntPtr)WM_RBUTTONDOWN ? "MouseDown" : wParam == (IntPtr)WM_LBUTTONUP ? "MouseUp" : "MouseUp";
                string button = wParam == (IntPtr)WM_LBUTTONDOWN || wParam == (IntPtr)WM_LBUTTONUP ? "Left" : "Right";
                string timestamp = System.DateTime.Now.ToString("o");
                _writer.WriteLine($"{eventType},{timestamp},,,{button},{hookStruct.pt.x},{hookStruct.pt.y}");
            }
            return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]//using System.Runtime.InteropServices;
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll",CharSet = CharSet.Auto,SetLastError =true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,LowLevelMouseProc lpfn,IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll",CharSet =CharSet.Auto,SetLastError =true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT 
        {
            public int x;
            public int y;
        }

        private enum Keys
        {
            A = 65, B = 66, C = 67, D = 68, E = 69, F = 70, G = 71, H = 72, I = 73, J = 74, K = 75, L = 76, M = 77,
            N = 78, O = 79, P = 80, Q = 81, R = 82, S = 83, T = 84, U = 85, V = 86, W = 87, X = 88, Y = 89, Z = 90,
            D0 = 48, D1 = 49, D2 = 50, D3 = 51, D4 = 52, D5 = 53, D6 = 54, D7 = 55, D8 = 56, D9 = 57,
            F1 = 112, F2 = 113, F3 = 114, F4 = 115, F5 = 116, F6 = 117, F7 = 118, F8 = 119, F9 = 120, F10 = 121, F11 = 122, F12 = 123,
            Enter = 13, Space = 32, LeftCtrl = 162, RightCtrl = 163, LeftAlt = 164, RightAlt = 165, LeftShift = 160, RightShift = 161
            // Add other keys as needed
        }
    }
}
