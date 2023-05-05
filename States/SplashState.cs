using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class SplashState : BaseState
{
    public override void LoadContent()
    {
        AddGameObject(new SplashImage(LoadTexture("backgrounds/win")));
    }

    public override void Update(GameTime gameTime)
    {
            
    }

}