using System.Collections.Generic;

using CookingGame.Enum;
using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class Shawarma : ClickableSprite
{
    private List<Ingredient> ingredientList = new List<Ingredient>();
    public int BurntMeter = 0;

    public Shawarma(Texture2D texture)
    {
        _texture = texture;
        Position = new Vector2(920, 410);
        BoundX = -20;
        BoundY = -20;
    }
}