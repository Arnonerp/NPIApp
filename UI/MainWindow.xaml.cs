using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using NPIApp.Helpers;
using NPIApp.Services;


namespace NPIApp.UI
{
    public partial class MainWindow : Window
    {
        private readonly WindowManager _windowManager;
        private readonly LoadingManager _loadingManager;

        public MainWindow()
        {
            InitializeComponent();

            _windowManager = new WindowManager();
            _loadingManager = new LoadingManager();

            _loadingManager.ProgressUpdated += OnProgressUpdated;
            _loadingManager.StatusUpdated += OnStatusUpdated;

            // Set full-screen mode on startup
            _windowManager.SetFullScreen(this);

            // Simulate loading
            SimulateLoading();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the current window
            this.Close();
        }

        private void OnProgressUpdated(int progress)
        {
            ProgressBar.Value = progress; // Update the progress bar
        }

        private void OnStatusUpdated(string status)
        {
            LoadingTextBlock.Text = status; // Update the loading text
        }

private async void Window_Loaded(object sender, RoutedEventArgs e)
{
    _windowManager.SetFullScreen(this); // Ensure full-screen on load
    await SimulateLoading(); // Trigger loading process
}


private async Task SimulateLoading()
{
    // Set the cursor to the default arrow (not busy) at the start
    Mouse.OverrideCursor = Cursors.Arrow;

    // Ensure the loading screen elements (logo, text, and progress bar) are visible
    LoadingScreen.Visibility = Visibility.Visible;
    MainContent.Visibility = Visibility.Collapsed;

    for (int i = 0; i <= 100; i += 10)
    {
        _loadingManager.UpdateProgress(i);
        _loadingManager.UpdateStatus($"Loading... {i}%");
        await Task.Delay(500); // Simulate work
    }

    _loadingManager.UpdateStatus("Loading complete!");

    // Hide the loading screen and show the main content after loading
    LoadingScreen.Visibility = Visibility.Collapsed;
    MainContent.Visibility = Visibility.Visible;

    // Ensure the cursor is set back to the default arrow
    Mouse.OverrideCursor = null;
}
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                // Exit full-screen mode
                _windowManager.ExitFullScreen(this);
            }
        }
    }
}