using CalmSpace.Views;
using CalmSpace.Models;
using Microsoft.Maui.Controls;

namespace CalmSpace.Pages.MainPage
{
    public partial class MainPage : ContentPage
    {
        private FavouritesView _favouritesView;

        public MainPage()
        {
            InitializeComponent();
            PermissionRequest();
        }

        private static async void PermissionRequest()
        {
            if (await Permissions.RequestAsync<Permissions.StorageRead>() != PermissionStatus.Granted)
            {
            }
            else if (await Permissions.RequestAsync<Permissions.StorageWrite>() != PermissionStatus.Granted)
            {
            }
            else if (await Permissions.RequestAsync<Permissions.Sensors>() != PermissionStatus.Granted)
            {
            }
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            var shell = (AppShell)Application.Current.MainPage;

            SwipeHandler.OnSwiped(shell, e);
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
            if (_favouritesView != null)
            {
                ContentLayout.Children.Remove(_favouritesView);
                _favouritesView = null;
            }

            _favouritesView = new FavouritesView();
            _favouritesView.CloseFavouritesAction = CloseFavouritesView;
            ContentLayout.Children.Add(_favouritesView);

            _favouritesView.IsVisible = true;
        }

        private void CloseFavouritesView()
        {
            if (_favouritesView != null)
            {
                ContentLayout.Children.Remove(_favouritesView);
                _favouritesView = null;
            }
        }

        private void OnContactClicked(object sender, EventArgs e)
        {
            ContactUsView.IsVisible = true;
        }
    }
}
