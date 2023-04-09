using System.Collections.Generic;
using CookingGame.Enum;
using CookingGame.Objects.Base;
using CookingGame.States;
using Microsoft.Xna.Framework;

namespace CookingGame.Objects;

public class Shawarma : ClickableSprite
{
    private List<Ingredient> ingredientList = new List<Ingredient>();
    private readonly Vector2 shawarmaPosition = new Vector2(100, 200);
    private int timeToGrill = 10;
    public int burntMeter = 0;

    // TODO circle bounds

    public Shawarma()
    {
        _texture = _texture;
        _position = shawarmaPosition;
    }

    public override void HandleClick(Point clickPosition)
    {
        if (Bounds.Contains(clickPosition))
        {
            // notify that shawarma has been cooked
        }
    }

    public override void HandleHold(Point clickPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleRelease(Point clickPosition)
    {
        
    }
}