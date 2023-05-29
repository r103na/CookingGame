using System;
using System.Linq;
using CookingGame.Objects;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.States;

public class SettingState : BaseState
{
    #region VARIABLES
    private Button _returnButton;

    private Button _soundVolumeDownButton;
    private Button _soundVolumeUpButton;
    private Button _musicVolumeDownButton;
    private Button _musicVolumeUpButton;

    private Button _yesButton;
    private Button _noButton;

    private Text _soundVolumeText;
    private Text _musicVolumeText;
    private SpriteFont _font;
    #endregion

    public override void LoadContent()
    {
        InitializeObjects();
        SubscribeButtons();
        AddGameObjects();

        SoundManager.LoadBackgroundMusic("music/menu");

        foreach (var button in GameObjects.OfType<ClickableSprite>())
        {
            button.Clicked += SoundManager.PlayButtonClick;
        }
    }

    private void InitializeObjects()
    {
        _font = ContentManager.Load<SpriteFont>("Fonts/MyFont");
        _returnButton = new Button(LoadTexture("gui/backbtn"), new Vector2(560, 600));

        _musicVolumeDownButton = new Button(LoadTexture("gui/arrowdown"), new Vector2(560, 240));
        _musicVolumeUpButton = new Button(LoadTexture("gui/arrowup"), new Vector2(700, 240));
        _musicVolumeText = new Text(_font, SettingsManager.Settings.MusicVolume.ToString(), new Vector2(630, 250));

        _soundVolumeDownButton = new Button(LoadTexture("gui/arrowdown"), new Vector2(560, 360));
        _soundVolumeUpButton = new Button(LoadTexture("gui/arrowup"), new Vector2(700, 360));
        _soundVolumeText = new Text(_font, SettingsManager.Settings.SoundVolume.ToString(), new Vector2(630, 370));

        _yesButton = new Button(LoadTexture("gui/yesbtn"), new Vector2(400, 520));
        _noButton = new Button(LoadTexture("gui/nobtn"), new Vector2(680, 520));
    }

    private void SubscribeButtons()
    {
        _returnButton.Clicked += SwitchToMenu;
        _soundVolumeDownButton.Clicked += (_, _) =>
        {
            if (SettingsManager.Settings.SoundVolume <= 0) return;
            SettingsManager.UpdateSoundVolume(-5);
            _soundVolumeText.StringText = SettingsManager.Settings.SoundVolume.ToString();
        };
        _soundVolumeUpButton.Clicked += (_, _) =>
        {
            if (SettingsManager.Settings.SoundVolume >= 100 ) return;
            SettingsManager.UpdateSoundVolume(5);
            _soundVolumeText.StringText = SettingsManager.Settings.SoundVolume.ToString();
        };

        _musicVolumeDownButton.Clicked += (_, _) =>
        {
            if (SettingsManager.Settings.MusicVolume <= 0) return;
            SettingsManager.UpdateMusicVolume(-5);
            _musicVolumeText.StringText = SettingsManager.Settings.MusicVolume.ToString();
        };
        _musicVolumeUpButton.Clicked += (_, _) =>
        {
            if (SettingsManager.Settings.MusicVolume >= 100) return;
            SettingsManager.UpdateMusicVolume(5);
            _musicVolumeText.StringText = SettingsManager.Settings.MusicVolume.ToString();
        };

        _yesButton.Clicked += _yesButton_Clicked;
        _noButton.Clicked += _noButton_Clicked;
    }

    private void _noButton_Clicked(object sender, EventArgs e)
    {
        SettingsManager.UpdateFullscreen(false);
        SettingsManager.UpdateSettings();
    }

    private void _yesButton_Clicked(object sender, EventArgs e)
    {
        SettingsManager.UpdateFullscreen(true);
        SettingsManager.UpdateSettings();
    }

    private void AddGameObjects()
    {
        AddGameObject(new SplashImage(LoadTexture("backgrounds/settingsbg")));
        AddGameObject(_returnButton);
        AddGameObject(_soundVolumeDownButton);
        AddGameObject(_soundVolumeUpButton);
        AddGameObject(_musicVolumeDownButton);
        AddGameObject(_musicVolumeUpButton);
        AddGameObject(_yesButton);
        AddGameObject(_noButton);
        AddText(_soundVolumeText);
        AddText(_musicVolumeText);
    }

    public override void Update(GameTime gameTime)
    {
        OnUpdated(null, EventArgs.Empty);
    }

    private void SwitchToMenu(object sender, EventArgs e)
    {
        SettingsManager.SaveSettings();
        SwitchState(new MenuState());
    }
}