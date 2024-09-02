using CalmSpace.Helpers;

namespace CalmSpace.Pages.SummerPage;

public partial class SummerPage : ContentPage
{
	public SummerPage()
	{
		InitializeComponent();
	}
    private void OnSwiped(object sender, SwipedEventArgs e)
    {
        var shell = (AppShell)Application.Current.MainPage;
        SwipeHandler.OnSwiped(shell, e);
    }
}