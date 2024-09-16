using CalmSpace.Helpers;
using CalmSpace.Views;
using Plugin.Maui.Audio;

namespace CalmSpace.Pages.SpringPage;

public partial class SpringPage : ContentPage
{
    private readonly SoundManager _soundManager;
    private readonly FavoriteManager _favoriteManager;
    private ShakeDetector _shakeDetector;

    public SpringPage(IAudioManager audioManager)
    {
        InitializeComponent();
        _soundManager = new SoundManager(audioManager);
        LoadSpringSounds();
        BindingContext = _soundManager;

        _soundManager.OnPlayStateChanged += UpdatePlayPauseButtonIcon;
        _soundManager.OnRemainingTimeUpdated += UpdateRemainingTime;

        TimerViewControl.TimerSet += OnTimerSet;
        _shakeDetector = new ShakeDetector(OnShakeDetected);
        _shakeDetector.Start();
    }

    private async void OnShakeDetected()
    {
        if (_soundManager.SoundItems.Count > 0)
        {
            await _soundManager.PlayNextSoundAsync();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _shakeDetector.Stop();
    }

    private async void LoadSpringSounds()
    {
        var SpringSoundMappings = new Dictionary<string, string>
        {
            { "midnight_rainfall.mp3", "Midnight Rainfall" },
            { "tranquil_stream.mp3", "Tranquil Stream" },
            { "gentle_creek_splash.mp3", "Gentle Creek Splash" },
            { "evening_birdsong.mp3", "Evening Birdsong" },
            { "umbrella_rain_comfort.mp3", "Umbrella Rain Comfort" }
        };

        await _soundManager.LoadSoundsAsync("Spring", SpringSoundMappings);
    }

    private void OnTimerButtonClicked(object sender, EventArgs e)
    {
        TimerViewControl.UpdateRemainingTime(_soundManager.RemainingSeconds);
        TimerViewControl.ShowTimerView();
    }

    private void OnTimerSet(int totalSeconds)
    {
        if (!_soundManager.IsPlaying)
        {
            if (_soundManager.SoundItems.Count > 0)
            {
                _soundManager.PlaySoundAsync(_soundManager.SoundItems[0].SoundFilePath);
            }
        }

        _soundManager.StartTimer(totalSeconds);
        TimerViewControl.IsVisible = false;
    }

    private void UpdateRemainingTime(int remainingSeconds)
    {
        TimerViewControl.UpdateRemainingTime(remainingSeconds);
    }

    private async void OnSoundButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var soundItem = button?.BindingContext as SoundItem;

        if (soundItem != null && !string.IsNullOrEmpty(soundItem.SoundFilePath))
        {
            await _soundManager.PlaySoundAsync(soundItem.SoundFilePath);
        }
    }

    private void OnPlayPauseButtonClicked(object sender, EventArgs e)
    {
        if (_soundManager.IsPlaying)
        {
            _soundManager.PauseSound();
        }
        else
        {
            if (_soundManager.SoundItems.Count > 0 && _soundManager._currentSoundFilePath != null)
            {
                _soundManager.ResumeSoundAsync();
            }
        }
    }

    private void UpdatePlayPauseButtonIcon(bool isPlaying)
    {
        PlayPauseButton.ImageSource = isPlaying ? "Icons/Player/stop_icon.svg" : "Icons/Player/play_icon.svg";
    }

    private async void OnSkipButtonClicked(object sender, EventArgs e)
    {
        if (_soundManager.SoundItems.Count > 0)
        {
            await _soundManager.PlayNextSoundAsync();
        }
    }

    private async void OnPreviousButtonClicked(object sender, EventArgs e)
    {
        if (_soundManager.SoundItems.Count > 0)
        {
            await _soundManager.PlayPreviousSoundAsync();
        }
    }

    private void OnSwiped(object sender, SwipedEventArgs e)
    {
        var shell = (AppShell)Application.Current.MainPage;
        SwipeHandler.OnSwiped(shell, e);
    }

    private async void OnFavoriteButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var filePath = button?.BindingContext as string;

        if (!string.IsNullOrEmpty(filePath))
        {
            if (_favoriteManager.IsFavorite(filePath))
            {
                await _favoriteManager.RemoveFavoriteAsync(filePath);
                await DisplayAlert("Favorite Removed", $"{filePath} has been removed from favorites.", "OK");
            }
            else
            {
                await _favoriteManager.AddFavoriteAsync(filePath);
                await DisplayAlert("Favorite Added", $"{filePath} has been added to favorites.", "OK");
            }
        }
    }
}