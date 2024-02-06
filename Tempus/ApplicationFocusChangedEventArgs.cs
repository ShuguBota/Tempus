using System.Diagnostics;

namespace Tempus;

public class ApplicationFocusChangedEventArgs(Process process) : EventArgs
{
    public void HandleFocusChange()
    {
        // process.ProcessName e.g. "notepad"
        // process.MainModule e.g. "C:\Windows\System32\notepad.exe"
        
        Debug.WriteLine($"Application focus changed to {process.ProcessName}, File Name: {process.MainModule.FileName}");
        Debug.WriteLine($"{process.MainWindowTitle}, {process.Handle}");
    }
}