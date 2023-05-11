﻿using System.Collections.Generic;

using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class Shawarma : ClickableSprite
{
    public List<IngredientItem> IngredientList = new();
    public bool IsWrapped;
    public bool IsGrilled;

    public Shawarma(Texture2D texture)
    {
        Texture = texture;
        Position = new Vector2(920, 430);
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