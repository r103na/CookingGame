using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace CookingGame.Managers;

public class SoundManager
{
    public Song BackgroundSong;
    public Dictionary<string, SoundEffect> SoundEffects = new();
    private readonly ContentManager _contentManager;
    private readonly SettingsManager _settingsManager;
    public float SoundVolume = 1;
    public float MusicVolume = 1;

    public SoundManager(ContentManager contentManager, SettingsManager settingsManager)
    {
        _contentManager = contentManager;
        _settingsManager = settingsManager;
        LoadSoundEffects();
        LoadSettings();
    }

    public void LoadBackgroundMusic(string backgroundMusicName)
    {
        BackgroundSong = _contentManager.Load<Song>(backgroundMusicName);
        MediaPlayer.Play(BackgroundSong);
        MediaPlayer.IsRepeating = true;
    }

    public void LoadSettings()
    {
        SoundVolume = _settingsManager.Settings.SoundVolume / 100f;
        MusicVolume = _settingsManager.Settings.MusicVolume / 100f;
        MediaPlayer.Volume = MusicVolume;
    }

    public void LoadSoundEffects()
    {
        SoundEffects = new Dictionary<string, SoundEffect>
        {
            { "select", _contentManager.Load<SoundEffect>("SFX/buttonClick") },
            { "newCustomer", _contentManager.Load<SoundEffect>("SFX/newcustomer") },
            {"grill", _contentManager.Load<SoundEffect>("SFX/grill") },
            {"sauce", _contentManager.Load<SoundEffect>("SFX/sauce")},
            {"win", _contentManager.Load<SoundEffect>("SFX/win")},
            {"lose", _contentManager.Load<SoundEffect>("SFX/lose")}
        };
    }

    public void PlayButtonClick(object sender, EventArgs e)
    {
        LoadSoundEffects();
        PlaySound("select");
    }

    public void PlaySound(string soundName)
    {
        SoundEffects?[soundName]?.Play(volume: SoundVolume, pitch: 0.0f, pan: 0.0f);
    }
}