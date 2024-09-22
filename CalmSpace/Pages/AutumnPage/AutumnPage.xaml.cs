using Plugin.Maui.Audio;
using CalmSpace.Views;

namespace CalmSpace.Pages.AutumnPage
{
    public partial class AutumnPage : BasicSoundPage
    {
        public AutumnPage(IAudioManager audioManager) : base(audioManager)
        {
            InitializeComponent();
            SetPlayPauseButton(this.FindByName<Button>("PlayPauseButton"));
            LoadSounds();
        }

        protected override async void LoadSounds()
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
                await _soundManager.LoadSoundsAsync("Autumn", autumnSoundMappings);
            });
        }
    }
}
