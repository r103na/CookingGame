using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects.Base
{
    public class MovableSprite : ClickableSprite
    {
        public MovableSprite(Texture2D texture, Vector2 position)
        {
            _position = position;
            _texture = texture;
            BoundX = 50;
            BoundY = 50;
        }

        public override void HandleHold(Point clickPosition)
        {
            if (IsInBounds(clickPosition))
            {
                OnClicked();
                _position.X = clickPosition.X - 20;
                _position.Y = clickPosition.Y - 50;
            }
        }

        public override void HandleClick(Point clickPosition)
        {
            
        }

        public override void HandleRelease(Point clickPosition)
        {
            
        }
    }
}
