using Plugin.Maui.Audio;
using CalmSpace.Views;
using System.Diagnostics;

namespace CalmSpace.Pages.WinterPage
{
    public partial class WinterPage : BasicSoundPage
    {
        public WinterPage(IAudioManager audioManager) : base(audioManager)
        {
            InitializeComponent();

            var timerView = this.FindByName<TimerView>("TimerViewControl");
            if (timerView != null)
            {
                SetTimerViewControl(timerView);
            }
            else
            {
                Debug.WriteLine("TimerViewControl is null after initialization.");
            }

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
