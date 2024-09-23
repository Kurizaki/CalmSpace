using Plugin.Maui.Audio;
using CalmSpace.ViewModels;
using Microsoft.Maui.Controls;
using System.Diagnostics; 
using CalmSpace.Views;

namespace CalmSpace.Pages.WinterPage
{
    public partial class WinterPage : ContentPage
    {
        private readonly BaseSoundViewModel _viewModel;

        public WinterPage(IAudioManager audioManager)
        {
            InitializeComponent();
            _viewModel = new BaseSoundViewModel(audioManager);
            this.BindingContext = _viewModel;
            LoadSounds();
            Debug.WriteLine("BindingContext set to ViewModel");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SetPlayPauseButton();
            SetTimerViewControl();
        }

        private void SetPlayPauseButton()
        {
            var playPauseButton = this.FindByName<Button>("PlayPauseButton");
            if (playPauseButton != null)
            {
                _viewModel.SetPlayPauseButton(playPauseButton);
                Debug.WriteLine("PlayPauseButton set successfully.");
            }
            else
            {
                Debug.WriteLine("PlayPauseButton is null. Check if it is correctly named in XAML.");
            }
        }

        private void SetTimerViewControl()
        {
            var timerViewControl = this.FindByName<TimerView>("TimerViewControl");
            if (timerViewControl != null)
            {
                _viewModel.SetTimerView(this.FindByName<TimerView>("TimerViewControl")); // Assuming SetTimerView is a method in ViewModel
                Debug.WriteLine("TimerViewControl set successfully.");
            }
            else
            {
                Debug.WriteLine("TimerViewControl is null. Check the XAML declaration.");
            }
        }

        private async void LoadSounds()
        {
            var winterSoundMappings = new Dictionary<string, string>
        {
            { "icicle_drips.mp3", "Icicle Drips" },
            { "crackling_fireside.mp3", "Crackling Fireside" },
            { "footsteps_on_frozen_ice.mp3", "Footsteps on Frozen Ice" },
            { "winter_woods_whisper.mp3", "Winter Woods Whisper" },
            { "snowstorm_whirl.mp3", "Snowstorm Whirl" }
        };

            await Task.Run(async () =>
            {
                await _viewModel.SoundManager.LoadSoundsAsync("Winter", winterSoundMappings);
            });
        }
    }

}