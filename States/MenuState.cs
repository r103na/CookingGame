using System;

using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class MenuState : BaseState
{
    private float _wait = 0.2f;
    private ClickableImage _background;
    public override void LoadContent()
    {
        SoundManager.LoadBackgroundMusic("music/menu");
        SoundManager.LoadSoundEffects();
        
        _background = new ClickableImage(LoadTexture("backgrounds/Menu"));
        _background.Clicked += SwitchToGameplay;
        _background.CanClick = false;
        AddGameObject(_background);

        Updated += WaitForStart;
    }

    public override void Update(GameTime gameTime)
    {
        UpdateTime(gameTime);
        OnUpdated(null, EventArgs.Empty);
    }

    private void WaitForStart(object sender, EventArgs e)
    {
        if (_wait > 0)
        {
            _wait -= ElapsedTime;
        }
        else
        {
            _background.CanClick = true;
            Updated -= WaitForStart;
        }
    }

    private void SwitchToGameplay(object sender, EventArgs e)
    {
        SwitchState(new GameplayState());
    }
}