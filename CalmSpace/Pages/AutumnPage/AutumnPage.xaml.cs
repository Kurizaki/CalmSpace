using CalmSpace.Helpers;

namespace CalmSpace.Pages.AutumnPage;

public partial class AutumnPage : ContentPage
{
    public AutumnPage()
    {
        InitializeComponent();
    }
    private void OnSwiped(object sender, SwipedEventArgs e)
    {
        var shell = (AppShell)Application.Current.MainPage;
        SwipeHandler.OnSwiped(shell, e);
    }
}