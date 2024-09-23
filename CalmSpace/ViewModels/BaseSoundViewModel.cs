using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CalmSpace.Models;
using CalmSpace.Views;
using System.Diagnostics;

public class BaseSoundViewModel : BindableObject
{
    private readonly SoundManager _soundManager;
    private readonly FavoriteManager _favoriteManager;
    private readonly ShakeDetector _shakeDetector;
    private TimerView? _timerViewControl;
    private Button? PlayPauseButton;

    private IAudioPlayer? _currentPlayer;
    private string? _currentSoundFilePath;
    private IAudioManager _audioManager;
    private bool _isPlaying;

    public ObservableCollection<SoundItem> SoundItems => _soundManager.SoundItems;
    public ICommand PlayPauseCommand { get; }
    public ICommand SkipCommand { get; }
    public ICommand PlaySpecificSoundCommand { get; }
    public ICommand PreviousCommand { get; }
    public ICommand FavoriteCommand { get; }
    public ICommand TimerCommand { get; }

    public bool IsPlaying
    {
        get => _isPlaying;
        set
        {
            if (_isPlaying != value)
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }
    }

    public BaseSoundViewModel(IAudioManager audioManager)
    {
        _favoriteManager = new FavoriteManager();
        _soundManager = new SoundManager(audioManager);
        _audioManager = audioManager;

        _soundManager.OnPlayStateChanged += UpdatePlayPauseButtonIcon;
        _soundManager.OnRemainingTimeUpdated += UpdateRemainingTime;

        _shakeDetector = new ShakeDetector(OnShakeDetected);
        _shakeDetector.Start();

        PlaySpecificSoundCommand = new Command<string>(async (soundFilePath) => await PlaySpecificSoundAsync(soundFilePath));
        PlayPauseCommand = new Command(OnPlayPauseButtonClicked);
        SkipCommand = new Command(OnSkipButtonClicked);
        PreviousCommand = new Command(OnPreviousButtonClicked);
        FavoriteCommand = new Command(OnFavoriteButtonClicked);
        TimerCommand = new Command(OnTimerButtonClicked);
    }

    public SoundManager SoundManager => _soundManager;

    public void SetPlayPauseButton(Button button)
    {
        PlayPauseButton = button;
    }
    public void SetTimerView(TimerView timerView)
    {
        _timerViewControl = timerView;
    }

    private async Task PlaySpecificSoundAsync(string soundFilePath)
{
    Debug.WriteLine($"PlaySpecificSoundAsync called with: {soundFilePath}");
    
    if (string.IsNullOrEmpty(soundFilePath))
    {
        Debug.WriteLine("Sound file path is null or empty.");
        return;
    }

    try
    {
        if (_currentPlayer != null && IsPlaying)
        {
            Debug.WriteLine("Stopping current player.");
            _currentPlayer.Stop();
            _currentPlayer.Dispose();
        }

        Debug.WriteLine("Creating new audio player.");
        _currentPlayer = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(soundFilePath));

        if (_currentPlayer == null)
        {
            Debug.WriteLine("Failed to create audio player.");
            return;
        }

        _currentPlayer.Play();
        _currentSoundFilePath = soundFilePath;
        IsPlaying = true;

        Debug.WriteLine("Sound started playing.");
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Error playing sound: {ex.Message}");
    }

    UpdatePlayPauseButtonIcon(IsPlaying);
}



    private void OnPlayPauseButtonClicked()
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


    private async void OnSkipButtonClicked(object parameter)
    {
        if (_soundManager.SoundItems.Count > 0)
        {
            await _soundManager.PlayNextSoundAsync();
            IsPlaying = true;
        }
    }

    private async void OnPreviousButtonClicked(object parameter)
    {
        if (_soundManager.SoundItems.Count > 0)
        {
            await _soundManager.PlayPreviousSoundAsync();
            IsPlaying = true;
        }
    }

    private void UpdatePlayPauseButtonIcon(bool isPlaying)
    {
        Dispatcher.Dispatch(() =>
        {
            if (PlayPauseButton != null)
            {
                PlayPauseButton.ImageSource = isPlaying
                    ? "Icons/Player/stop_icon.svg"  // Show stop/pause icon if playing
                    : "Icons/Player/play_icon.svg"; // Show play icon if paused
            }
        });
    }

    // Handle Favorite button clicks
    private async void OnFavoriteButtonClicked(object parameter)
    {
        var button = parameter as Button;
        var soundItem = button?.BindingContext as SoundItem;

        if (soundItem != null && !string.IsNullOrEmpty(soundItem.SoundFilePath))
        {
            var mainPage = Application.Current?.MainPage;
            if (mainPage != null)
            {
                if (_favoriteManager.IsFavorite(soundItem.SoundFilePath))
                {
                    await _favoriteManager.RemoveFavoriteAsync(soundItem.SoundFilePath);
                    await mainPage.DisplayAlert("Favorite Removed", $"{soundItem.SoundName} has been removed from favorites.", "OK");
                }
                else
                {
                    await _favoriteManager.AddFavoriteAsync(soundItem.SoundFilePath);
                    await mainPage.DisplayAlert("Favorite Added", $"{soundItem.SoundName} has been added to favorites.", "OK");
                }
            }
        }
    }

    private void OnTimerButtonClicked(object parameter)
    {
        _timerViewControl?.UpdateRemainingTime(_soundManager.RemainingSeconds);
        _timerViewControl?.ShowTimerView();
    }

    protected async void OnTimerSet(int totalSeconds)
    {
        if (!_soundManager.IsPlaying)
        {
            if (_soundManager.SoundItems.Count > 0)
            {
                await _soundManager.PlaySoundAsync(_soundManager.SoundItems[0].SoundFilePath);
            }
        }

        _soundManager.StartTimer(totalSeconds);
        if (_timerViewControl != null)
        {
            _timerViewControl.IsVisible = false;
        }
    }

    private void UpdateRemainingTime(int remainingSeconds)
    {
        _timerViewControl?.UpdateRemainingTime(remainingSeconds);
    }

    private async void OnShakeDetected()
    {
        if (_soundManager.SoundItems.Count > 0)
        {
            await _soundManager.PlayNextSoundAsync();
        }
    }

    protected void OnDisappearing()
    {
        _shakeDetector.Stop();
    }
}
