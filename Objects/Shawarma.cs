using System.Collections.Generic;
using CookingGame.Enum;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;

namespace CookingGame.Objects;

public class Shawarma : ClickableSprite
{
    private List<Ingredient> ingredientList = new List<Ingredient>();
    private readonly Vector2 _shawarmaPosition = new Vector2(100, 200);
    private int _timeToGrill = 10;
    public int BurntMeter = 0;

    public Shawarma()
    {
        _texture = _texture;
        _position = _shawarmaPosition;
    }
}