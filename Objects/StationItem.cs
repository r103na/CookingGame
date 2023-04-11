using CookingGame.Enum;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class StationItem : ClickableSprite
{
    private Ingredient ingredient;
    private bool isDragging = false;

    public StationItem(Ingredient ingredient, Texture2D texture)
    {
        this.ingredient = ingredient;
        var str = nameof(ingredient);
        _texture = texture;
        BoundX = 30;
        BoundY = 30;
    }
}