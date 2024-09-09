using System.Collections.ObjectModel;

namespace CalmSpace.Helpers;

public class SoundManager
{
    public ObservableCollection<SoundItem> SoundItems { get; private set; }

    public SoundManager()
    {
        SoundItems = new ObservableCollection<SoundItem>();
    }

    public async Task LoadSoundsAsync(string season, Dictionary<string, string> soundMappings)
    {
        try
        {
            foreach (var soundFile in soundMappings)
            {
                var fullPath = $"Resources/Sounds/{season}/{soundFile.Key}";
                using var stream = await FileSystem.OpenAppPackageFileAsync(fullPath);

                if (stream != null)
                {
                    SoundItems.Add(new SoundItem
                    {
                        SoundName = soundFile.Value,
                        SoundFilePath = fullPath
                    });
                    stream?.Dispose();
                }
                else
                {
                    Console.WriteLine($"Error loading sound: {fullPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading sounds: {ex.Message}");
        }
    }


    public void PlaySound(string filePath)
    {
        Console.WriteLine($"Playing sound from: {filePath}");
    }
}