using Plugin.Maui.Audio;
using CalmSpace.ViewModels;
using Microsoft.Maui.Controls;

namespace CalmSpace.Pages.SpringPage
{
    public partial class SpringPage : ContentPage
    {
        private readonly BaseSoundViewModel _viewModel;

        public SpringPage(IAudioManager audioManager)
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
            var springSoundMappings = new Dictionary<string, string>
            {
                { "midnight_rainfall.mp3", "Midnight Rainfall" },
                { "tranquil_stream.mp3", "Tranquil Stream" },
                { "gentle_creek_splash.mp3", "Gentle Creek Splash" },
                { "evening_birdsong.mp3", "Evening Birdsong" },
                { "umbrella_rain_comfort.mp3", "Umbrella Rain Comfort" }
            };

            await Task.Run(async () =>
            {
                await _viewModel.SoundManager.LoadSoundsAsync("Spring", springSoundMappings);
            });
        }
    }
}
