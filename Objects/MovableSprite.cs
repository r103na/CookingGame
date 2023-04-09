using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
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

        public override void HandleClick(Point clickPosition)
        {
            if (IsInBounds(clickPosition))
            {
                OnClicked();
                _position.X = clickPosition.X;
                _position.Y = clickPosition.Y;
            }
        }

        public override void HandleRelease(Point clickPosition)
        {
            throw new NotImplementedException();
        }
    }
}
