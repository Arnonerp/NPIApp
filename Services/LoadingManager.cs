using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace NPIApp.Services
{
    public class LoadingManager
    {
        public event Action<int> ProgressUpdated; // Event for progress updates
        public event Action<string> StatusUpdated; // Event for status updates

        public void UpdateProgress(int progress)
        {
            ProgressUpdated?.Invoke(progress); // Notify listeners
        }

        public void UpdateStatus(string status)
        {
            StatusUpdated?.Invoke(status); // Notify listeners
        }
    }
}
