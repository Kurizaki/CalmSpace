using System;
using System.Timers;

namespace CalmSpace.Models
{
    public class TimerManager
    {
        private System.Timers.Timer _timer;
        private int _remainingSeconds;
        private bool _timerRunning;

        public event Action<int>? OnRemainingTimeUpdated;
        public event Action? OnTimerFinished;

        public TimerManager()
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
        }

        public void StartTimer(int totalSeconds)
        {
            _remainingSeconds = totalSeconds;
            _timerRunning = true;
            _timer.Start();
        }

        public void StopTimer()
        {
            _timerRunning = false;
            _timer.Stop();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_remainingSeconds <= 0)
            {
                _timerRunning = false;
                _timer.Stop();
                OnTimerFinished?.Invoke();
            }
            else
            {
                _remainingSeconds--;
                OnRemainingTimeUpdated?.Invoke(_remainingSeconds);
            }
        }

        public int RemainingSeconds => _remainingSeconds;
        public bool IsRunning => _timerRunning;
    }
}
