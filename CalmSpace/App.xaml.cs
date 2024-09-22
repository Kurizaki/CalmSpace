using CalmSpace.Models;

namespace CalmSpace
{
    public partial class App : Application
    {
        private readonly FavoriteManager _favoriteManager;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            _favoriteManager = new FavoriteManager();
        }

        // Called when the app starts
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override async void OnSleep()
        {
            base.OnSleep();
            await _favoriteManager.SaveFavoritesAsync();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}
