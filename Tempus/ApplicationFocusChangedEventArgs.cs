using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Tempus;

public class ApplicationFocusChangedEventArgs(Process process) : EventArgs
{
    // Import the FindWindowEx method from the user32.dll library
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    // Import the GetClassName method from the user32.dll library
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    // Import the GetWindowText method from the user32.dll library
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    // Import the GetWindowLong method from the user32.dll library
    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    // Define some constants for the GetWindowLong method
    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20;
    
    public void HandleFocusChange()
    {
        // process.ProcessName e.g. "notepad"
        // process.MainModule e.g. "C:\Windows\System32\notepad.exe"
        
        Debug.WriteLine($"Application focus changed to {process.ProcessName}, File Name: {process.MainModule.FileName}");
        Debug.WriteLine($"{process.MainWindowTitle}, {process.Handle}");
        
        var chromeHandle = process.MainWindowHandle;

        // Get the handle of the tab window
        var tabHandle = FindWindowEx(chromeHandle, IntPtr.Zero, null, null);

        // Get the class name of the tab window
        var className = new StringBuilder(256);
        GetClassName(tabHandle, className, className.Capacity);

        // Get the text of the tab window
        var text = new StringBuilder(256);
        GetWindowText(tabHandle, text, text.Capacity);

        // Get the style of the tab window
        var style = GetWindowLong(tabHandle, GWL_STYLE);

        // Get the extended style of the tab window
        var exStyle = GetWindowLong(tabHandle, GWL_EXSTYLE);

        // Print the information
        Debug.WriteLine("Handle: " + tabHandle);
        Debug.WriteLine("Class name: " + className);
        Debug.WriteLine("Text: " + text);
        Debug.WriteLine("Style: " + style);
        Debug.WriteLine("Extended style: " + exStyle);
    }
}