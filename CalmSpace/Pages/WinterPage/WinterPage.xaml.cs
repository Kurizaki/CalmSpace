using Plugin.Maui.Audio;

namespace CalmSpace.Pages.WinterPage
{
    public partial class WinterPage : BasicSoundPage
    {
        public WinterPage(IAudioManager audioManager) : base(audioManager)
        {
            InitializeComponent();
            SetPlayPauseButton(this.FindByName<Button>("PlayPauseButton"));
            LoadSounds();
        }

        protected override async void LoadSounds()
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
                await _soundManager.LoadSoundsAsync("Winter", winterSoundMappings);
            });
        }
    }
}
