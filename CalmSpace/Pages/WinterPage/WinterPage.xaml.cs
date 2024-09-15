using CalmSpace.Helpers;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;

namespace CalmSpace.Pages.WinterPage;
public partial class WinterPage : ContentPage
{
    private readonly SoundManager _soundManager;
    private readonly FavoriteManager _favoriteManager;

    public WinterPage(IAudioManager audioManager)
    {
        InitializeComponent();
        _soundManager = new SoundManager(audioManager);
        LoadWinterSounds();
        BindingContext = _soundManager;

        _soundManager.OnPlayStateChanged += UpdatePlayPauseButtonIcon;
        _soundManager.OnRemainingTimeUpdated += UpdateRemainingTime;

        TimerViewControl.TimerSet += OnTimerSet;
    }

    private async void LoadWinterSounds()
    {
        var winterSoundMappings = new Dictionary<string, string>
        {
            { "icicle_drips.mp3", "Icicle Drips" },
            { "crackling_fireside.mp3", "Crackling Fireside" },
            { "footsteps_on_frozen_ice.mp3", "Footsteps on Frozen Ice" },
            { "winter_woods_whisper.mp3", "Winter Woods Whisper" },
            { "snowstorm_whirl.mp3", "Snowstorm Whirl" }
        };

        await _soundManager.LoadSoundsAsync("Winter", winterSoundMappings);
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

