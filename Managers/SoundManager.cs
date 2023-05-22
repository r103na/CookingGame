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

    public SoundManager(ContentManager contentManager)
    {
        _contentManager = contentManager;
        LoadSoundEffects();
    }

    public void LoadBackgroundMusic(string backgroundMusicName)
    {
        BackgroundSong = _contentManager.Load<Song>(backgroundMusicName);
        MediaPlayer.Play(BackgroundSong);
        MediaPlayer.IsRepeating = true;
    }

    public void LoadSoundEffects()
    {
        SoundEffects = new Dictionary<string, SoundEffect>
        {
            { "select", _contentManager.Load<SoundEffect>("SFX/buttonClick") },
            { "newCustomer", _contentManager.Load<SoundEffect>("SFX/newcustomer") },
            {"grill", _contentManager.Load<SoundEffect>("SFX/grill") }, 
            {"win", _contentManager.Load<SoundEffect>("SFX/win")},
            {"lose", _contentManager.Load<SoundEffect>("SFX/lose")}
        };
    }

    public void PlayButtonClick(object sender, EventArgs e)
    {
        LoadSoundEffects();
        SoundEffects?["select"]?.Play();
    }
}