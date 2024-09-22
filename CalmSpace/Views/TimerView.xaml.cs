using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CalmSpace.Models;

namespace CalmSpace.Views
{
    public partial class TimerView : ContentView
    {
        public ICommand SetTimerCommand { get; set; }
        private readonly TimerManager _TimerManager;

        public event Action<int>? TimerSet;

        public TimerView()
        {
            InitializeComponent();
            _TimerManager = new TimerManager();
            _TimerManager.OnRemainingTimeUpdated += UpdateRemainingTime;
            SetTimerCommand = new Command(SetTimer);
            BindingContext = this;
        }

        private void SetTimer()
        {
            Debug.WriteLine("Set Timer button clicked");

            int hours = HoursPicker.SelectedIndex > -1 ? (int)HoursPicker.SelectedItem : 0;
            int minutes = MinutesPicker.SelectedIndex > -1 ? (int)MinutesPicker.SelectedItem : 0;

            int totalSeconds = (hours * 3600) + (minutes * 60);

            if (totalSeconds > 0)
            {
                TimerSet?.Invoke(totalSeconds);
                _TimerManager.StartTimer(totalSeconds);
                IsVisible = false;
            }
            else
            {
                Debug.WriteLine("No time set, totalSeconds is 0");
            }
        }

        public void UpdateRemainingTime(int remainingSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(remainingSeconds);
            RemainingTimeLabel.Text = $"Time remaining: {timeSpan:hh\\:mm\\:ss}";
            RemainingTimeLabel.IsVisible = _TimerManager.IsRunning && remainingSeconds > 0;
        }

        public void ShowTimerView()
        {
            if (_TimerManager.IsRunning && _TimerManager.RemainingSeconds > 0)
            {
                UpdateRemainingTime(_TimerManager.RemainingSeconds);
            }

            IsVisible = true;
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            IsVisible = false;
        }
    }
}
