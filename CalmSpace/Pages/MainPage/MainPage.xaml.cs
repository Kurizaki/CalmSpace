using CalmSpace.Helpers;

namespace CalmSpace.Pages.MainPage
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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
        }

        private void OnSettingsClicked(object sender, EventArgs e)
        {
        }

        private void OnFavouritesClicked(object sender, EventArgs e)
        {
        }

        private void OnContactClicked(object sender, EventArgs e)
        {
        }
    }
}
