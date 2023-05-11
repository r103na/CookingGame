using System;

using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class MenuState : BaseState
{
    private float wait = 0.2f;
    private ImageObject background;
    public override void LoadContent()
    {
        background = new ImageObject(LoadTexture("backgrounds/Menu"));
        background.Clicked += SwitchToGameplay;
        background.CanClick = false;
        AddGameObject(background);

        Updated += WaitForStart;
    }

    public override void Update(GameTime gameTime)
    {
        UpdateTime(gameTime);
        OnUpdated(null, EventArgs.Empty);
    }

    private void WaitForStart(object sender, EventArgs e)
    {
        if (wait > 0)
        {
            wait -= ElapsedTime;
        }
        else
        {
            background.CanClick = true;
            Updated -= WaitForStart;
        }
    }

    private void SwitchToGameplay(object sender, EventArgs e)
    {
        SwitchState(new GameplayState());
    }
}