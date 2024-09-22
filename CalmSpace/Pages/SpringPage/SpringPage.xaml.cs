using Plugin.Maui.Audio;

namespace CalmSpace.Pages.SpringPage
{
    public partial class SpringPage : BasicSoundPage
    {
        public SpringPage(IAudioManager audioManager) : base(audioManager)
        {
            InitializeComponent();
            SetPlayPauseButton(this.FindByName<Button>("PlayPauseButton"));
            LoadSounds();
        }

        protected override async void LoadSounds()
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
                await _soundManager.LoadSoundsAsync("Spring", springSoundMappings);
            });
        }
    }
}
