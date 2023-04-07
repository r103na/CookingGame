﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class Button : ClickableSprite
{
    public Button(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        _position = position;
    }

    public override void HandleClick(Point clickPosition)
    {
        if (IsInBounds(clickPosition))
        {
            OnClicked();
        }
    }

    public override void HandleRelease(Point clickPosition)
    {

    }
}