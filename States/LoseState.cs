using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class LoseState : BaseState
{
    public override void LoadContent() 
    {
        var background = new ClickableImage(LoadTexture("backgrounds/loss"));
        background.Clicked += (_, _) =>
        {
            SwitchState(new GameplayState());
        };
        AddGameObject(background);
        SoundManager.PlaySound("lose");

    }

    public override void Update(GameTime gameTime)
    {
    }
}