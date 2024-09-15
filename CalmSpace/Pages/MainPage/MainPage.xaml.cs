using CalmSpace.Helpers;
using CalmSpace.Views;

namespace CalmSpace.Pages.MainPage
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            MainPage.PermissionRequest();
        }
        private static async void PermissionRequest()
        {
            if (await Permissions.RequestAsync<Permissions.StorageRead>() != PermissionStatus.Granted)
            {
            }
            else if (await Permissions.RequestAsync<Permissions.StorageWrite>() != PermissionStatus.Granted)
            {
            }
        }
        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            var shell = (AppShell)Application.Current.MainPage;
            SwipeHandler.OnSwiped(shell, e);
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue;
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
        }

        private void OnIntroductionClicked(object sender, EventArgs e)
        {
            IntroductionView.IsVisible = true;
        }

        private void OnSettingsClicked(object sender, EventArgs e)
        {
            SettingsView.IsVisible = true;
        }

        private void OnFavouritesClicked(object sender, EventArgs e)
        {
            FavouritesView.IsVisible = true;
        }

        private void OnContactClicked(object sender, EventArgs e)
        {
            ContactUsView.IsVisible = true;
        }
    }
}
