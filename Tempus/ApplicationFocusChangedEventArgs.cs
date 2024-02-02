using System.Diagnostics;

namespace Tempus;

public class ApplicationFocusChangedEventArgs(string processName, string fileName) : EventArgs
{
    public string ProcessName { get; } = processName;
    public string FileName { get; } = fileName;
    
    public void HandleFocusChange()
    {
        Debug.WriteLine($"Application focus changed to {ProcessName}, File Name: {FileName}");
    }
}