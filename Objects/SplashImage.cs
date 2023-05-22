using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CookingGame.Objects.Base;
using System;
using Microsoft.Xna.Framework.Input;

namespace CookingGame.Objects;

public class SplashImage : BaseSprite
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

    public virtual void HandleHover(Point clickPosition)
    {
        if (IsInBounds(clickPosition))
        {
            Mouse.SetCursor(MouseCursor.Arrow);
        }
    }
}
