using CookingGame.Enum;
using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class StationItem : ClickableSprite
{
    private Ingredient _ingredient;

    public StationItem(Ingredient ingredient, Texture2D texture, Vector2 position)
    {
        _ingredient = ingredient;
        Position = position;
        _texture = texture;
    }
}