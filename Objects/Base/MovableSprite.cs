using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects.Base
{
    public class MovableSprite : ClickableSprite
    {
        protected Rectangle acceptableBounds;
        public MovableSprite(Texture2D texture, Vector2 position)
        {
            Position = position;
            Texture = texture;
            BoundX = (int)Center.X * 2;
            BoundY = (int)Center.Y * 2;
            
        }
        public override void HandleHold(Point clickPosition)
        {
            if (!IsInBounds(clickPosition) || !CanClick) return;
            Position.X = clickPosition.X - Center.X;
            Position.Y = clickPosition.Y - Center.Y;
        }
    }
}
