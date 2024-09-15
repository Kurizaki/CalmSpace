using CalmSpace.Helpers;
using CalmSpace.Views;
using Plugin.Maui.Audio;

namespace CalmSpace.Pages.SummerPage;

public partial class SummerPage : ContentPage
{
    private readonly SoundManager _soundManager;
    private readonly FavoriteManager _favoriteManager;

    public SummerPage(IAudioManager audioManager)
    {
        InitializeComponent();
        _soundManager = new SoundManager(audioManager);
        LoadSummerSounds();
        BindingContext = _soundManager;

        _soundManager.OnPlayStateChanged += UpdatePlayPauseButtonIcon;
        _soundManager.OnRemainingTimeUpdated += UpdateRemainingTime;

        TimerViewControl.TimerSet += OnTimerSet;
    }

    private async void LoadSummerSounds()
    {
        var SummerSoundMappings = new Dictionary<string, string>
        {
            { "footsteps_in_water.mp3", "Footsteps in Water" },
            { "torrential_summer_rain.mp3", "Torrential Summer Rain" },
            { "mountain_breeze_and_summer_birds.mp3", "Mountain Breeze & Summer Birds" },
            { "jungle_awakening.mp3", "Jungle Awakening" },
            { "sumer_night_symphony.mp3", "Summer Night Symphony" }
        };

        await _soundManager.LoadSoundsAsync("Summer", SummerSoundMappings);
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