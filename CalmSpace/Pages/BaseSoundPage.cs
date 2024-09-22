using CalmSpace.Helpers;
using CalmSpace.Views;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;
using System.Diagnostics;

namespace CalmSpace.Pages
{
    public abstract class BasicSoundPage : ContentPage
    {
        protected readonly SoundManager _soundManager;
        protected readonly FavoriteManager _favoriteManager;
        protected Button PlayPauseButton;
        protected ShakeDetector _shakeDetector;
        protected TimerView TimerViewControl;

        public BasicSoundPage(IAudioManager audioManager)
        {
            _favoriteManager = new FavoriteManager();
            _soundManager = new SoundManager(audioManager);
            BindingContext = _soundManager;

            _soundManager.OnPlayStateChanged += UpdatePlayPauseButtonIcon;
            _soundManager.OnRemainingTimeUpdated += UpdateRemainingTime;

            _shakeDetector = new ShakeDetector(OnShakeDetected);
            _shakeDetector.Start();
        }
        protected void SetTimerViewControl(TimerView timerView)
        {
            TimerViewControl = timerView;
        }
        protected void SetPlayPauseButton(Button button)
        {
            PlayPauseButton = button;
        }

        protected abstract void LoadSounds();

        private async void OnShakeDetected()
        {
            if (_soundManager.SoundItems.Count > 0)
            {
                await _soundManager.PlayNextSoundAsync();
            }
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            var shell = (AppShell)Application.Current.MainPage;
            SwipeHandler.OnSwiped(shell, e);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _shakeDetector.Stop();
        }

        protected void OnTimerButtonClicked(object sender, EventArgs e)
        {
            if (TimerViewControl != null)
            {
                TimerViewControl.UpdateRemainingTime(_soundManager.RemainingSeconds);
                TimerViewControl.ShowTimerView();
            }
            else
            {
                Debug.WriteLine("TimerViewControl is null");
            }
        }


        protected void OnTimerSet(int totalSeconds)
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

        private async void OnSoundButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var soundItem = button?.BindingContext as SoundItem;

            if (soundItem != null && !string.IsNullOrEmpty(soundItem.SoundFilePath))
            {
                await _soundManager.PlaySoundAsync(soundItem.SoundFilePath);
            }
        }

        protected async void OnSkipButtonClicked(object sender, EventArgs e)
        {
            if (_soundManager.SoundItems.Count > 0)
            {
                await _soundManager.PlayNextSoundAsync();
            }
        }

        protected async void OnPreviousButtonClicked(object sender, EventArgs e)
        {
            if (_soundManager.SoundItems.Count > 0)
            {
                await _soundManager.PlayPreviousSoundAsync();
            }
        }

        protected async void OnFavoriteButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var soundItem = button?.BindingContext as SoundItem;

            if (soundItem != null && !string.IsNullOrEmpty(soundItem.SoundFilePath))
            {
                if (_favoriteManager.IsFavorite(soundItem.SoundFilePath))
                {
                    await _favoriteManager.RemoveFavoriteAsync(soundItem.SoundFilePath);
                    await DisplayAlert("Favorite Removed", $"{soundItem.SoundName} has been removed from favorites.", "OK");
                }
                else
                {
                    await _favoriteManager.AddFavoriteAsync(soundItem.SoundFilePath);
                    await DisplayAlert("Favorite Added", $"{soundItem.SoundName} has been added to favorites.", "OK");
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
