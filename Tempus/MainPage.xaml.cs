using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Tempus;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
        
        ReadDeviceFocusedApplication();
    }

    private void ReadDeviceFocusedApplication()
    {
        while (true)
        {
            // Import the user32.dll library
            [DllImport("user32.dll")]
            static extern IntPtr GetForegroundWindow();

            // Import the user32.dll library
            [DllImport("user32.dll")]
            static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

            // Get the handle of the active window
            IntPtr hWnd = GetForegroundWindow();

            // Get the process ID of the window
            uint procId = 0;
            GetWindowThreadProcessId(hWnd, out procId);

            // Get the Process object of the window
            var proc = Process.GetProcessById((int)procId);

            // Print the name of the application
            Debug.WriteLine("The current application name is: " + proc.ProcessName);
            Debug.WriteLine("The current application file name is: " + proc.MainModule.FileName);
            
            Thread.Sleep(5000);
        }
    }
    
    private void ReadDeviceDisplay()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine($"Pixel width: {DeviceDisplay.Current.MainDisplayInfo.Width} / Pixel Height: {DeviceDisplay.Current.MainDisplayInfo.Height}");
        sb.AppendLine($"Density: {DeviceDisplay.Current.MainDisplayInfo.Density}");
        sb.AppendLine($"Orientation: {DeviceDisplay.Current.MainDisplayInfo.Orientation}");
        sb.AppendLine($"Rotation: {DeviceDisplay.Current.MainDisplayInfo.Rotation}");
        sb.AppendLine($"Refresh Rate: {DeviceDisplay.Current.MainDisplayInfo.RefreshRate}");

        Debug.WriteLine(sb.ToString());
    }
}