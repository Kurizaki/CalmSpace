using Plugin.Maui.Audio;
using CalmSpace.ViewModels;
using Microsoft.Maui.Controls;
using System.Diagnostics;

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
            SetPlayPauseButton(this.FindByName<Button>("PlayPauseButton"));
            LoadSounds();
            Debug.WriteLine("BindingContext set to ViewModel");
        }

        private void SetPlayPauseButton(Button button)
        {
            _viewModel.SetPlayPauseButton(button);
            Debug.WriteLine("PlayPauseButton set successfully.");
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
