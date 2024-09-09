using System.Collections.ObjectModel;
using System.IO;
using CalmSpace;
using CalmSpace.Helpers;
using CalmSpace.Pages.WinterPage;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;
namespace CalmSpace.Pages.WinterPage;



public partial class WinterPage: ContentPage
{
    private readonly SoundManager _soundManager;

    public WinterPage()
    {
        InitializeComponent();
        _soundManager = new SoundManager();
        LoadWinterSounds();
        BindingContext = _soundManager;
    }

    private async void LoadWinterSounds()
    {
        var winterSoundMappings = new Dictionary<string, string>
        {
            { "icicle_drips.mp3", "Icicle Drips" },
            { "crackling_fireside.mp3", "Crackling Fireside" },
            { "footsteps_on_frozen_ice.mp3", "Footsteps on Frozen Ice" },
            { "winter_woods_whisper.mp3", "Winter Woods Whisper" },
            { "snowstorm_whirl.mp3", "Snowstorm Whirl" }
        };

        await _soundManager.LoadSoundsAsync("Winter", winterSoundMappings);
    }

    private void OnSoundButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var soundItem = button.BindingContext as SoundItem;

        if (soundItem != null)
        {
            _soundManager.PlaySound(soundItem.SoundFilePath);
        }
    }

    private void OnSwiped(object sender, SwipedEventArgs e)
    {
        var shell = (AppShell)Application.Current.MainPage;
        SwipeHandler.OnSwiped(shell, e);
    }

    private void OnPlayPauseButtonClicked(object sender, EventArgs e)
    {
        // Implement play/pause functionality
    }

    private void OnTimerButtonClicked(object sender, EventArgs e)
    {
        // Implement timer functionality
    }

    private void OnFavoriteButtonClicked(object sender, EventArgs e)
    {
        // Implement favorite functionality
    }
}
