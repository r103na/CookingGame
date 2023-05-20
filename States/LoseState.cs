using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class LoseState : BaseState
{
    public override void LoadContent() 
    {
        var background = new SplashImage(LoadTexture("backgrounds/loss"));
        background.Clicked += (_, _) =>
        {
            SwitchState(new GameplayState());
        };
        AddGameObject(background);
        SoundManager.SoundEffects["lose"].Play();

    }

    public override void Update(GameTime gameTime)
    {
    }
}