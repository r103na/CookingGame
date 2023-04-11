using CookingGame.Enum;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class StationItem : ClickableSprite
{
    private Ingredient ingredient;

    public StationItem(Ingredient ingredient, Texture2D texture, Vector2 position)
    {
        this.ingredient = ingredient;
        _position = position;
        var str = nameof(ingredient);
        _texture = texture;
    }
}