namespace Tempus;

public interface IApplicationFocusTracker
{
    event EventHandler<ApplicationFocusChangedEventArgs> ApplicationFocusChanged;
    void StartTracking();
    void StopTracking();
}