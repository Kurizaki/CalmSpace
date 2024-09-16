using CalmSpace.Helpers;
using CalmSpace.Views;
using Plugin.Maui.Audio;

namespace CalmSpace.Pages.AutumnPage;

public partial class AutumnPage : ContentPage
{
    private readonly SoundManager _soundManager;
    private readonly FavoriteManager _favoriteManager;
    private ShakeDetector _shakeDetector;

    public AutumnPage(IAudioManager audioManager)
    {
        InitializeComponent();
        _soundManager = new SoundManager(audioManager);
        LoadAutumnSounds();
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

    private async void LoadAutumnSounds()
    {
        var AutumnSoundMappings = new Dictionary<string, string>
        {
            { "riverbank_morning.mp3", "Riverbank Morning" },
            { "hurricanes_edge.mp3", "Hurricane’s Edge" },
            { "thunderous_rainstorm.mp3", "Thunderous Rainstorm" },
            { "raindrops_on_car_roof.mp3", "Raindrops on Car Roof" },
            { "crunching_autumn_leaves.mp3", "Crunching Autumn Leaves" }
        };

        await _soundManager.LoadSoundsAsync("Autumn", AutumnSoundMappings);
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