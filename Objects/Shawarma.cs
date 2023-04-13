using System.Collections.Generic;

using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class Shawarma : ClickableSprite
{
    public List<IngredientItem> IngredientList = new List<IngredientItem>();
    public int BurntMeter = 0;

    public Shawarma(Texture2D texture)
    {
        _texture = texture;
        Position = new Vector2(920, 410);
        BoundX = -20;
        BoundY = -20;
    }
}