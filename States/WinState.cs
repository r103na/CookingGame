using System;
using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class WinState : BaseState
{
    private ClickableImage _bg;
    public override void LoadContent()
    {
        _bg = new ClickableImage(LoadTexture("backgrounds/win"));
        _bg.Clicked += SwitchToMenu;
        AddGameObject(_bg);
        SoundManager.PlaySound("win");
    }

    public override void Update(GameTime gameTime)
    {
            
    }

    private void SwitchToMenu(object sender, EventArgs e)
    {
        SwitchState(new MenuState());
    }

}