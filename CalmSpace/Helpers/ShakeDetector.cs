using System;
using Microsoft.Maui.Devices.Sensors;
using System.Timers;

public class ShakeDetector : IDisposable
{
    private const double ShakeThreshold = 2.0;
    private readonly Action _onShakeDetected;
    private System.Timers.Timer _shakeCooldownTimer;
    private bool _canDetectShake = true;

    public ShakeDetector(Action onShakeDetected)
    {
        _onShakeDetected = onShakeDetected;
        Accelerometer.ReadingChanged += OnAccelerometerReadingChanged;
        _shakeCooldownTimer = new System.Timers.Timer(1000);
        _shakeCooldownTimer.AutoReset = false;
    }

    public void Start()
    {
        if (!Accelerometer.IsMonitoring)
        {
            Accelerometer.Start(SensorSpeed.Fastest);
        }
    }

    public void Stop()
    {
        if (Accelerometer.IsMonitoring)
        {
            Accelerometer.Stop();
        }
    }

    private void OnAccelerometerReadingChanged(object sender, AccelerometerChangedEventArgs e)
    {
        if (!_canDetectShake) return;

        double acceleration = Math.Sqrt(
            Math.Pow(e.Reading.Acceleration.X, 2) +
            Math.Pow(e.Reading.Acceleration.Y, 2));

        if (acceleration > ShakeThreshold)
        {
            _onShakeDetected?.Invoke();
            _canDetectShake = false;

            _shakeCooldownTimer.Elapsed += (s, args) => { _canDetectShake = true; };
            _shakeCooldownTimer.Start();
        }
    }

    public void Dispose()
    {
        Accelerometer.ReadingChanged -= OnAccelerometerReadingChanged;
        Stop();
        _shakeCooldownTimer.Dispose();
    }
}
