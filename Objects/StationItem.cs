using CookingGame.Enum;
using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class StationItem : ClickableSprite
{
    public Ingredient Ingredient { get; }

    public StationItem(Ingredient ingredient, Texture2D texture, Vector2 position)
    {
        Ingredient = ingredient;
        Position = position;
        Texture = texture;
    }
}