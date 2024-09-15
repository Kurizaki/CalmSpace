using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace CalmSpace.Views;

public partial class TimerView : ContentView
{
    public ICommand SetTimerCommand { get; set; }

    private int _totalSeconds;
    private int _remainingSeconds;
    private bool _timerRunning;

    public event Action<int>? TimerSet;

    public TimerView()
    {
        InitializeComponent();
        SetTimerCommand = new Command(SetTimer);
        BindingContext = this;
    }

    private void SetTimer()
    {
        Debug.WriteLine("Set Timer button clicked");

        int hours = HoursPicker.SelectedIndex > -1 ? (int)HoursPicker.SelectedItem : 0;
        int minutes = MinutesPicker.SelectedIndex > -1 ? (int)MinutesPicker.SelectedItem : 0;

        _totalSeconds = (hours * 3600) + (minutes * 60);
        _remainingSeconds = _totalSeconds;

        if (_totalSeconds > 0)
        {
            _timerRunning = true;
            TimerSet?.Invoke(_totalSeconds);
            UpdateRemainingTime(_remainingSeconds);
            IsVisible = false;
        }
        else
        {
            Debug.WriteLine("No time set, totalSeconds is 0");
        }
    }

    public void UpdateRemainingTime(int remainingSeconds)
    {
        _remainingSeconds = remainingSeconds;

        var timeSpan = TimeSpan.FromSeconds(_remainingSeconds);
        RemainingTimeLabel.Text = $"Time remaining: {timeSpan:hh\\:mm\\:ss}";
        RemainingTimeLabel.IsVisible = _timerRunning && _remainingSeconds > 0;
    }

    public void ShowTimerView()
    {
        if (_timerRunning && _remainingSeconds > 0)
        {
            UpdateRemainingTime(_remainingSeconds);
        }

        IsVisible = true;
    }

    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        IsVisible = false;
    }
}