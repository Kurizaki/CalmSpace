using CalmSpace.Helpers;

namespace CalmSpace.Pages.SpringPage;

public partial class SpringPage : ContentPage
{
	public SpringPage()
	{
		InitializeComponent();
	}
    private void OnSwiped(object sender, SwipedEventArgs e)
    {
        var shell = (AppShell)Application.Current.MainPage;
        SwipeHandler.OnSwiped(shell, e);
    }
}