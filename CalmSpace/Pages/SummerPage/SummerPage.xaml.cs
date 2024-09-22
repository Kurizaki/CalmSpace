using Plugin.Maui.Audio;
using CalmSpace.ViewModels;
using Microsoft.Maui.Controls;

namespace CalmSpace.Pages.SummerPage
{
    public partial class SummerPage : ContentPage
    {
        private readonly BaseSoundViewModel _viewModel;

        public SummerPage(IAudioManager audioManager)
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
            var summerSoundMappings = new Dictionary<string, string>
            {
                { "footsteps_in_water.mp3", "Footsteps in Water" },
                { "torrential_summer_rain.mp3", "Torrential Summer Rain" },
                { "mountain_breeze_and_summer_birds.mp3", "Mountain Breeze & Summer Birds" },
                { "jungle_awakening.mp3", "Jungle Awakening" },
                { "summer_night_symphony.mp3", "Summer Night Symphony" }
            };

            await Task.Run(async () =>
            {
                await _viewModel.SoundManager.LoadSoundsAsync("Summer", summerSoundMappings);
            });
        }
    }
}
