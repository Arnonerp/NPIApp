using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace NPIApp.Helpers
{
    public static class WindowHelper
    {
        // DLL Imports
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowWindow(int hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // Constants
        public const int GWL_STYLE = -16;
        public const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
        public const int SWP_NOOWNERZORDER = 0x0200;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_SHOWWINDOW = 0x0040;

        // Example Helper Methods
        public static void ShowWindowByHandle(int hWnd, int command)
        {
            ShowWindow(hWnd, command);
        }

        public static void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close(); // Call the close method on the window
            }
        }
    }
}
