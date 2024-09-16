using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CalmSpace.Helpers;

namespace CalmSpace.Views
{
    public partial class FavouritesView : ContentView
    {
        private readonly FavoriteManager _favoriteManager;
        public ObservableCollection<string> Favourites { get; set; }

        public FavouritesView()
        {
            InitializeComponent();
            _favoriteManager = new FavoriteManager();
            Favourites = new ObservableCollection<string>(_favoriteManager.GetFavorites());
            BindingContext = this;
        }

        private async void OnRemoveFavourite(object sender, EventArgs e)
        {
            var button = sender as Button;
            var soundFilePath = button?.BindingContext as string;

            if (soundFilePath != null)
            {
                await _favoriteManager.RemoveFavoriteAsync(soundFilePath);
                Favourites.Remove(soundFilePath);
            }
        }

        private async Task AddFavoriteAsync(string soundFilePath)
        {
            if (!_favoriteManager.IsFavorite(soundFilePath))
            {
                await _favoriteManager.AddFavoriteAsync(soundFilePath);
                Favourites.Add(soundFilePath);
            }
        }
        private void CloseFavourites(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }
    }
}
