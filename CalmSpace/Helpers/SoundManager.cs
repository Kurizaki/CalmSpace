using Plugin.Maui.Audio;
using System.Collections.ObjectModel;

namespace CalmSpace.Helpers
{
    public class SoundManager
    {
        public ObservableCollection<SoundItem> SoundItems { get; private set; }
        private readonly IAudioManager _audioManager;
        private readonly FavoriteManager _favoriteManager;
        private readonly TimerManager _TimerManager;
        private IAudioPlayer? _audioPlayer;
        private int _currentSoundIndex = -1;

        public event Action<bool>? OnPlayStateChanged; 
        public event Action<int>? OnRemainingTimeUpdated;

        public bool IsPlaying => _audioPlayer?.IsPlaying ?? false;
        public int RemainingSeconds => _TimerManager.RemainingSeconds;
        public string? _currentSoundFilePath;

        public SoundManager(IAudioManager audioManager)
        {
            _audioManager = audioManager;
            SoundItems = new ObservableCollection<SoundItem>();
            _favoriteManager = new FavoriteManager();
            _TimerManager = new TimerManager();
            _TimerManager.OnRemainingTimeUpdated += remainingSeconds => OnRemainingTimeUpdated?.Invoke(remainingSeconds);
            _TimerManager.OnTimerFinished += StopSound;
        }

        public async Task LoadSoundsAsync(string category, Dictionary<string, string> soundMappings)
        {
            try
            {
                SoundItems.Clear();
                foreach (var soundFile in soundMappings)
                {
                    var fullPath = $"Resources/Sounds/{category}/{soundFile.Key}";
                    using var stream = await FileSystem.OpenAppPackageFileAsync(fullPath);

                    if (stream != null)
                    {
                        var isFavorite = _favoriteManager.IsFavorite(fullPath);
                        SoundItems.Add(new SoundItem
                        {
                            SoundName = soundFile.Value,
                            SoundFilePath = fullPath,
                            IsFavorite = isFavorite
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading sounds: {ex.Message}");
            }
        }

        public async Task PlaySoundAsync(string filePath)
        {
            try
            {
                StopSound();
                _currentSoundFilePath = filePath;
                _currentSoundIndex = SoundItems.ToList().FindIndex(s => s.SoundFilePath == filePath);

                using var stream = await FileSystem.OpenAppPackageFileAsync(filePath);
                if (stream != null)
                {
                    _audioPlayer = _audioManager.CreatePlayer(stream);
                    _audioPlayer.Loop = true;
                    _audioPlayer.Play();
                    OnPlayStateChanged?.Invoke(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing sound: {ex.Message}");
            }
        }

        public async Task ResumeSoundAsync()
        {
            if (_currentSoundFilePath != null)
            {
                await PlaySoundAsync(_currentSoundFilePath);
                OnPlayStateChanged?.Invoke(true);
            }
        }

        public void PauseSound()
        {
            if (_audioPlayer?.IsPlaying ?? false)
            {
                _audioPlayer.Pause();
                Console.WriteLine("Sound paused, updating PlayPauseButton to show play icon.");
                OnPlayStateChanged?.Invoke(false); // Fire event when paused
            }
        }


        public void StopSound()
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.Stop();
                OnPlayStateChanged?.Invoke(false);
            }
        }

        public async Task PlayNextSoundAsync()
        {
            if (SoundItems.Count == 0) return;

            _currentSoundIndex = (_currentSoundIndex + 1) % SoundItems.Count;
            var nextSound = SoundItems[_currentSoundIndex];
            await PlaySoundAsync(nextSound.SoundFilePath);
        }

        public async Task PlayPreviousSoundAsync()
        {
            if (SoundItems.Count == 0) return;

            _currentSoundIndex = (_currentSoundIndex - 1 + SoundItems.Count) % SoundItems.Count;
            var previousSound = SoundItems[_currentSoundIndex];
            await PlaySoundAsync(previousSound.SoundFilePath);
        }

        public void StartTimer(int totalSeconds)
        {
            _TimerManager.StartTimer(totalSeconds);
        }
    }
}
