using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects.Base
{
    public class MovableSprite : ClickableSprite
    {
        public MovableSprite(Texture2D texture, Vector2 position)
        {
            Position = position;
            _texture = texture;
            BoundX = 50;
            BoundY = 50;
        }
        public override void HandleHold(Point clickPosition)
        {
            if (!IsInBounds(clickPosition) || !canClick) return;
            Position.X = clickPosition.X;
            Position.Y = clickPosition.Y;
        }
    }
}
