using System;
using System.Linq;
using CookingGame.Enum;
using CookingGame.Objects;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class MenuState : BaseState
{
    private SplashImage _background;
    public override void LoadContent()
    {
        SoundManager.LoadBackgroundMusic("music/menu");
        SoundManager.LoadSoundEffects();
        
        _background = new SplashImage(LoadTexture("backgrounds/Menu"));
        AddGameObject(_background);

        var playbtn = new Button(LoadTexture("gui/playbtn"), new Vector2(500, 340));
        var settingsButton = new Button(LoadTexture("gui/settingsbtn"), new Vector2(500, 440));
        var exitbtn = new Button(LoadTexture("gui/exitbtn"), new Vector2(550, 540));

        settingsButton.Clicked += SwitchToSettings;
        playbtn.Clicked += SwitchToGameplay;
        exitbtn.Clicked += (_, _) =>
        {
            NotifyEvent(Events.GAME_QUIT);
        };

        AddGameObject(playbtn);
        AddGameObject(settingsButton);
        AddGameObject(exitbtn);

        foreach (var button in GameObjects.OfType<ClickableSprite>())
        {
            button.Clicked += SoundManager.PlayButtonClick;
        }
    }

    public override void Update(GameTime gameTime)
    {
        UpdateTime(gameTime);
        OnUpdated(null, EventArgs.Empty);
    }


    private void SwitchToGameplay(object sender, EventArgs e)
    {
        SwitchState(new TutorialState());
    }

    private void SwitchToSettings(object sender, EventArgs e)
    {
        SwitchState(new SettingState());
    }
}