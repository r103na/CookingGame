﻿using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class ClickableImage : ClickableSprite
{
    public ClickableImage(Texture2D texture)
    {
        Texture = texture;
    }

    public ClickableImage(Texture2D texture, Vector2 position)
    {
        Texture = texture;
        Position = position;
    }
}