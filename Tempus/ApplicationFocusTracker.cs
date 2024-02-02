using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Tempus;

public class ApplicationFocusTracker : IApplicationFocusTracker
{
    private const uint WinEventOutOfContext = 0;
    private const uint EventSystemForeground = 3;

    private IntPtr _hook;

    public event EventHandler<ApplicationFocusChangedEventArgs>? ApplicationFocusChanged;

    public ApplicationFocusTracker()
    {
        ApplicationFocusChanged += (sender, e) => e.HandleFocusChange();
    }

    /*
     * StartTracking method
     * Use to track what application is currently focused
     */
    public void StartTracking()
    {
        // Making a singleton pattern
        if (_hook != IntPtr.Zero)
        {
            return;
        }
        
        /*
         * SetWinEventHook function
         * https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwineventhook
         * 
         * EVENT_SYSTEM_FOREGROUND,   // eventMin: The event constant that identifies the event of interest (in this case, foreground window change)
         * EVENT_SYSTEM_FOREGROUND,   // eventMax: The event constant that identifies the last possible event (again, foreground window change)
         * IntPtr.Zero,               // hmodWinEventProc: Handle to the DLL that contains the callback function (in this case, it's not used, so set to IntPtr.Zero)
         * WinEventProc,              // lpfnWinEventProc: A pointer to the event hook function (your callback method)
         * 0,                         // idProcess: Identifies the process that you want to associate with the hook (0 means all processes)
         * 0,                         // idThread: Identifies the thread that you want to associate with the hook (0 means all threads)
         * WINEVENT_OUTOFCONTEXT      // dwFlags: Specifies whether the hook function is in the same process as the caller or a different one (WINEVENT_OUTOFCONTEXT means the hook function is in a different thread or process)
         */
        _hook = SetWinEventHook(EventSystemForeground, EventSystemForeground, IntPtr.Zero, WinEventProc, 0, 0, WinEventOutOfContext);
    }

    public void StopTracking()
    {
        UnhookWinEvent(_hook);
    }

    /*
     * WinEventProc function
     * https://docs.microsoft.com/en-us/windows/win32/api/winuser/nc-winuser-wineventproc
     *
     * Extracts the information about the newly focused window, retrieving the process ID, and then triggering the OnApplicationFocusChanged event with the relevant details.
     */
    private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
        try
        {
            GetWindowThreadProcessId(hwnd, out var processId);
        
            var process = Process.GetProcessById((int) processId);

            if (process.MainModule is null)
            {
                return;
            }

            OnApplicationFocusChanged(new ApplicationFocusChangedEventArgs(process.ProcessName, process.MainModule.FileName));
        }
        catch(Exception e)
        {
            Debug.WriteLine(e);
        }
    }
    
    private void OnApplicationFocusChanged(ApplicationFocusChangedEventArgs e)
    {
        ApplicationFocusChanged?.Invoke(this, e);
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    [DllImport("user32.dll")]
    private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll")]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
}