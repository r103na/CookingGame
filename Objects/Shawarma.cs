using System.Collections.Generic;

using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class Shawarma : ClickableSprite
{
    public List<IngredientItem> IngredientList = new();
    public bool IsWrapped;
    public bool IsGrilled;
    public int FlatbreadType;

    public Shawarma(Texture2D texture, int flatbreadType)
    {
        Texture = texture;
        Position = new Vector2(920, 430);
        FlatbreadType = flatbreadType;
    }

    public void ChangePosition()
    {
        Position = new Vector2(1540, 460);
    }

    public void Grill()
    {
        if (IsWrapped) IsGrilled = true;
    }

    public void Wrap() => IsWrapped = true;
}