namespace Tempus;

public partial class MainPage : ContentPage
{
    private readonly IApplicationFocusTracker _applicationFocusTracker;
    private int _count = 0;
    
    public MainPage(IApplicationFocusTracker applicationFocusTracker)
    {
        InitializeComponent();
        
        _applicationFocusTracker = applicationFocusTracker;
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        _count++;

        CounterBtn.Text = _count == 1 ? $"Clicked {_count} time" : $"Clicked {_count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
        
        _applicationFocusTracker.StartTracking();
    }
}