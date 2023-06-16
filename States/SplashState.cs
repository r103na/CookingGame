using System;
using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class SplashState : BaseState
{
    private ClickableImage bg;
    public override void LoadContent()
    {
        bg = new ClickableImage(LoadTexture("backgrounds/win"));
        bg.Clicked += SwitchToMenu;
        AddGameObject(bg);
        SoundManager.SoundEffects["win"].Play();
    }

    public override void Update(GameTime gameTime)
    {
            
    }

    private void SwitchToMenu(object sender, EventArgs e)
    {
        SwitchState(new MenuState());
    }

}