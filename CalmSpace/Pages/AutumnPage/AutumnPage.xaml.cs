using Plugin.Maui.Audio;
using CalmSpace.ViewModels;
using Microsoft.Maui.Controls;

namespace CalmSpace.Pages.AutumnPage
{
    public partial class AutumnPage : ContentPage
    {
        private readonly BaseSoundViewModel _viewModel;

        public AutumnPage(IAudioManager audioManager)
        {
            InitializeComponent();
            _viewModel = new BaseSoundViewModel(audioManager);
            this.BindingContext = _viewModel;
            SetPlayPauseButton(this.FindByName<Button>("PlayPauseButton"));
            LoadSounds();
        }

        private void SetPlayPauseButton(Button button)
        {
            _viewModel.SetPlayPauseButton(button);
        }

        private async void LoadSounds()
        {
            var autumnSoundMappings = new Dictionary<string, string>
            {
                { "riverbank_morning.mp3", "Riverbank Morning" },
                { "hurricanes_edge.mp3", "Hurricane’s Edge" },
                { "thunderous_rainstorm.mp3", "Thunderous Rainstorm" },
                { "raindrops_on_car_roof.mp3", "Raindrops on Car Roof" },
                { "crunching_autumn_leaves.mp3", "Crunching Autumn Leaves" }
            };

            await Task.Run(async () =>
            {
                await _viewModel.SoundManager.LoadSoundsAsync("autumn", autumnSoundMappings);
            });
        }
    }
}
