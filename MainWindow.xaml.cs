using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NPIApp
{
    public partial class MainWindow : Window
    {
        // DllImport declarations for Windows API
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(int hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // Constants for window styles and flags
        private const int GWL_STYLE = -16;
        private const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
        private const int SWP_NOOWNERZORDER = 0x0200;
        private const int SWP_FRAMECHANGED = 0x0020;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private static readonly IntPtr HWND_TOP = IntPtr.Zero;

        // Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Start in full-screen mode
            EnterFullScreen();

            // Start the initial loading process
            StartLoading();
        }

        private async void StartLoading()
        {
            await ShowLoadingScreenAsync(async () =>
            {
                // Simulate a loading process
                for (int i = 0; i <= 100; i++)
                {
                    // Update the progress bar value
                    ProgressBar.Value = i;

                    // Simulate work with a delay
                    await Task.Delay(50); // Adjust delay as needed
                }
            }, "Loading, please wait...");
        }

        // Reusable method to show the loading screen
        public async Task ShowLoadingScreenAsync(Func<Task> action, string loadingMessage = "Loading, please wait...")
        {
            try
            {
                // Hide the cursor and show the loading screen
                this.Cursor = System.Windows.Input.Cursors.None;
                LoadingScreen.Visibility = Visibility.Visible;
                MainContent.Visibility = Visibility.Collapsed;

                // Update the loading text
                var loadingText = LoadingScreen.FindName("LoadingTextBlock") as TextBlock;
                if (loadingText != null)
                {
                    loadingText.Text = loadingMessage;
                }

                // Execute the passed action
                await action();
            }
            finally
            {
                // Restore the cursor and hide the loading screen
                this.Cursor = System.Windows.Input.Cursors.Arrow;
                LoadingScreen.Visibility = Visibility.Collapsed;
                MainContent.Visibility = Visibility.Visible;
            }
        }

        private void EnterFullScreen()
        {
            IntPtr hWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            // Get the monitor where the window is currently displayed
            var screen = System.Windows.Forms.Screen.FromHandle(hWnd);

            // Use monitor's bounds to calculate fullscreen dimensions
            int screenWidth = screen.Bounds.Width;
            int screenHeight = screen.Bounds.Height;

            // Remove the title bar and borders
            int style = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, style & ~WS_OVERLAPPEDWINDOW);

            // Resize and reposition the window to cover the entire screen
            SetWindowPos(hWnd, HWND_TOP, screen.Bounds.X, screen.Bounds.Y, screenWidth, screenHeight,
                SWP_NOOWNERZORDER | SWP_FRAMECHANGED | SWP_NOZORDER | SWP_SHOWWINDOW);

            this.Topmost = true;
            this.Focus();
        }

        private void ExitFullScreen()
        {
            IntPtr hWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            // Restore the title bar and borders
            int style = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, style | WS_OVERLAPPEDWINDOW);

            // Set the window to normal mode
            this.WindowState = WindowState.Normal; // Directly go to small screen with one square button
            this.ResizeMode = ResizeMode.CanResize; // Ensure resizing is allowed in this state

            // Resize and reposition the window to its default size
            SetWindowPos(hWnd, HWND_TOP, 100, 100, 800, 600,
                SWP_NOOWNERZORDER | SWP_FRAMECHANGED | SWP_NOZORDER | SWP_SHOWWINDOW);

            // Allow other windows to overlap
            this.Topmost = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Escape)
            {
                // Exit fullscreen and restore taskbar
                int taskBarHandle = FindWindow("Shell_TrayWnd", null);
                ShowWindow(taskBarHandle, SW_SHOW);

                ExitFullScreen();
            }
            else if (e.Key == Key.F11)
            {
                // Hide the taskbar and enter fullscreen
                int taskBarHandle = FindWindow("Shell_TrayWnd", null);
                ShowWindow(taskBarHandle, SW_HIDE);

                EnterFullScreen();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Show the taskbar again when app is closed
            int taskBarHandle = FindWindow("Shell_TrayWnd", null);
            ShowWindow(taskBarHandle, SW_SHOW);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the application
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            // Check if the window is in fullscreen mode
            if (this.WindowState == WindowState.Maximized && this.WindowStyle == WindowStyle.None)
            {
                EnterFullScreen(); // Ensure fullscreen is re-applied
            }
        }
    }
}
