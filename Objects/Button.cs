using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class Button : ClickableSprite
{
    public Button(Texture2D texture, Vector2 position)
    {
        Texture = texture;
        Position = position;

        Hovered += (_, _) =>
        {
            Color = Color.Brown;
        };
        Released += (_, _) =>
        {
            Color = Color.White;
        };
    }
}