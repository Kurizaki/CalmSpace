using CalmSpace.Helpers;

namespace CalmSpace.Pages.WinterPage;

public partial class WinterPage : ContentPage
{
	public WinterPage()
	{
		InitializeComponent();
	}
    private void OnSwiped(object sender, SwipedEventArgs e)
    {
        var shell = (AppShell)Application.Current.MainPage;
        SwipeHandler.OnSwiped(shell, e);
    }

    private void OnPlayPauseButtonClicked(object sender, EventArgs e)
    {

    }

    private void OnTimerButtonClicked(object sender, EventArgs e)
    {

    }
    private void OnFavoriteButtonClicked(object sender, EventArgs e)
    {

    }
}