using Plugin.Maui.Audio;
using System.Diagnostics;
using CalmSpace.Views;

namespace CalmSpace.Pages.SummerPage
{
    public partial class SummerPage : BasicSoundPage
    {
        public SummerPage(IAudioManager audioManager) : base(audioManager)
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
            var summerSoundMappings = new Dictionary<string, string>
               {
                { "footsteps_in_water.mp3", "Footsteps in Water" },
                { "torrential_summer_rain.mp3", "Torrential Summer Rain" },
                { "mountain_breeze_and_summer_birds.mp3", "Mountain Breeze & Summer Birds" },
                { "jungle_awakening.mp3", "Jungle Awakening" },
                { "sumer_night_symphony.mp3", "Summer Night Symphony" }
            };

            await Task.Run(async () =>
            {
                await _soundManager.LoadSoundsAsync("Summer", summerSoundMappings);
            });
        }
    }
}
