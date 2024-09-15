using System.Text.Json;

namespace CalmSpace.Helpers;

public class FavoriteManager
{
    private readonly string _filePath;
    private List<string> _favorites; // Store the file paths of favorite sounds

    public FavoriteManager()
    {
        _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favorites.json");
        _favorites = new List<string>();
        LoadFavorites();
    }

    // Load favorites from the file when the app starts
    private void LoadFavorites()
    {
        if (File.Exists(_filePath))
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                _favorites = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading favorites: {ex.Message}");
                _favorites = new List<string>();
            }
        }
    }

    // Save favorites to the file
    private async Task SaveFavoritesAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_favorites);
            await File.WriteAllTextAsync(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving favorites: {ex.Message}");
        }
    }

    // Add a sound to the favorites
    public async Task AddFavoriteAsync(string soundFilePath)
    {
        if (!_favorites.Contains(soundFilePath))
        {
            _favorites.Add(soundFilePath);
            await SaveFavoritesAsync();
        }
    }

    // Remove a sound from the favorites
    public async Task RemoveFavoriteAsync(string soundFilePath)
    {
        if (_favorites.Contains(soundFilePath))
        {
            _favorites.Remove(soundFilePath);
            await SaveFavoritesAsync();
        }
    }

    // Check if a sound is favorited
    public bool IsFavorite(string soundFilePath)
    {
        return _favorites.Contains(soundFilePath);
    }

    // Get all favorite sounds
    public List<string> GetFavorites()
    {
        return _favorites;
    }
}