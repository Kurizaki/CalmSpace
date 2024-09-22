using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CalmSpace.Models;
using CalmSpace.Views;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;

namespace CalmSpace.ViewModels
{
    public class BaseSoundViewModel : BindableObject
    {
        private readonly SoundManager _soundManager;
        private readonly FavoriteManager _favoriteManager;
        private readonly ShakeDetector _shakeDetector;
        private TimerView? _timerViewControl;
        private Button? PlayPauseButton;

        public ObservableCollection<SoundItem> SoundItems => _soundManager.SoundItems;
        public ICommand PlayPauseCommand { get; }
        public ICommand SkipCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand FavoriteCommand { get; }
        public ICommand TimerCommand { get; }

        public BaseSoundViewModel(IAudioManager audioManager)
        {
            _favoriteManager = new FavoriteManager();
            _soundManager = new SoundManager(audioManager);

            _soundManager.OnPlayStateChanged += UpdatePlayPauseButtonIcon;
            _soundManager.OnRemainingTimeUpdated += UpdateRemainingTime;

            _shakeDetector = new ShakeDetector(OnShakeDetected);
            _shakeDetector.Start();

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

        protected void LoadSounds() { }

        private async void OnShakeDetected()
        {
            if (_soundManager.SoundItems.Count > 0)
            {
                await _soundManager.PlayNextSoundAsync();
            }
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            var shell = Application.Current?.MainPage as AppShell;
            if (shell != null)
            {
                SwipeHandler.OnSwiped(shell, e);
            }
        }

        protected void OnDisappearing()
        {
            _shakeDetector.Stop();
        }

        protected void OnTimerButtonClicked(object parameter)
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

        private async void OnPlayPauseButtonClicked(object parameter)
        {
            var soundItem = parameter as SoundItem;
            if (soundItem != null && !string.IsNullOrEmpty(soundItem.SoundFilePath))
            {
                Debug.WriteLine($"PlayPause button clicked for sound: {soundItem.SoundName}");

                if (_soundManager.IsPlaying && _soundManager._currentSoundFilePath == soundItem.SoundFilePath)
                {
                    _soundManager.PauseSound();
                }
                else
                {
                    await _soundManager.PlaySoundAsync(soundItem.SoundFilePath);
                }
            }
        }

        private async void OnSoundButtonClicked(object parameter)
        {
            var button = parameter as Button;
            var soundItem = button?.BindingContext as SoundItem;

            if (soundItem != null && !string.IsNullOrEmpty(soundItem.SoundFilePath))
            {
                await _soundManager.PlaySoundAsync(soundItem.SoundFilePath);
            }
        }

        protected async void OnSkipButtonClicked(object parameter)
        {
            if (_soundManager.SoundItems.Count > 0)
            {
                await _soundManager.PlayNextSoundAsync();
            }
        }

        protected async void OnPreviousButtonClicked(object parameter)
        {
            if (_soundManager.SoundItems.Count > 0)
            {
                await _soundManager.PlayPreviousSoundAsync();
            }
        }

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

        private void UpdatePlayPauseButtonIcon(bool isPlaying)
        {
            Dispatcher.Dispatch(() =>
            {
                if (PlayPauseButton != null)
                {
                    PlayPauseButton.ImageSource = isPlaying
                        ? "Icons/Player/stop_icon.svg"
                        : "Icons/Player/play_icon.svg";
                }
                else
                {
                    Console.WriteLine("PlayPauseButton is null, cannot update icon.");
                }
            });
        }
    }
}
