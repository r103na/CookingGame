using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CookingGame.Objects.Base;

namespace CookingGame.Objects;

public class SplashImage : ClickableSprite
{
    public SplashImage(Texture2D texture)
    {
        Texture = texture;
    }
    public SplashImage(Texture2D texture, Vector2 position)
    {
        Texture = texture;
        Position = position;
    }
}