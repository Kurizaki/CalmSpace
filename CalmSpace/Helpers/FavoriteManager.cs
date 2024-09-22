using System.Text.Json;
using Microsoft.Maui.Storage;

namespace CalmSpace.Helpers
{
    public class FavoriteManager
    {
        private const string FavoritesKey = "favoriteSounds";
        private List<string> _favorites;

        public FavoriteManager()
        {
            _favorites = new List<string>();
            LoadFavorites();
        }

        private void LoadFavorites()
        {
            var json = Preferences.Get(FavoritesKey, string.Empty);

            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    _favorites = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
                    Console.WriteLine($"Loaded {_favorites.Count} favorites from preferences.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading favorites: {ex.Message}");
                    _favorites = new List<string>();
                }
            }
            else
            {
                _favorites = new List<string>();
            }
        }

        public async Task SaveFavoritesAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(_favorites);
                Preferences.Set(FavoritesKey, json);
                Console.WriteLine("Favorites saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving favorites: {ex.Message}");
            }
        }

        public async Task AddFavoriteAsync(string soundFilePath)
        {
            if (!_favorites.Contains(soundFilePath))
            {
                _favorites.Add(soundFilePath);
                await SaveFavoritesAsync();
            }
        }

        public async Task RemoveFavoriteAsync(string soundFilePath)
        {
            if (_favorites.Remove(soundFilePath))
            {
                await SaveFavoritesAsync();
            }
        }

        public bool IsFavorite(string soundFilePath)
        {
            return _favorites.Contains(soundFilePath);
        }

        public List<string> GetFavorites()
        {
            return _favorites;
        }
    }
}
