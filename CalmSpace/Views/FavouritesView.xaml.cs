using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using CalmSpace.Models;
using System;

namespace CalmSpace.Views
{
    public partial class FavouritesView : ContentView
    {
        private readonly FavoriteManager _favoriteManager;
        public ObservableCollection<SoundItem> Favourites { get; set; }

        public Action CloseFavouritesAction { get; set; }

        public FavouritesView()
        {
            InitializeComponent();
            _favoriteManager = new FavoriteManager();
            Favourites = new ObservableCollection<SoundItem>();
            BindingContext = this;

            ReloadFavorites();
        }

        public void ReloadFavorites()
        {
            var favoritesList = _favoriteManager.GetFavorites();
            Console.WriteLine($"Found {favoritesList.Count} favorites.");

            Favourites.Clear();
            foreach (var favoriteFilePath in favoritesList)
            {
                var soundItem = new SoundItem
                {
                    SoundFilePath = favoriteFilePath,
                    SoundName = System.IO.Path.GetFileNameWithoutExtension(favoriteFilePath),
                };

                Favourites.Add(soundItem);
                Console.WriteLine($"Added favorite: {soundItem.SoundName}");
            }
        }

        private void CloseFavourites(object sender, EventArgs e)
        {
            CloseFavouritesAction?.Invoke();
        }

        private async void OnRemoveFavourite(object sender, EventArgs e)
        {
            var button = sender as Button;
            var soundItem = button?.BindingContext as SoundItem;

            if (soundItem != null)
            {
                Console.WriteLine($"Removing favorite: {soundItem.SoundName}");
                await _favoriteManager.RemoveFavoriteAsync(soundItem.SoundFilePath);
                Favourites.Remove(soundItem);
            }
        }
    }
}
