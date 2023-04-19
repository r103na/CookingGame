using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects.Base
{
    public class MovableSprite : ClickableSprite
    {
        protected Rectangle acceptableBounds;
        protected bool idk;
        public MovableSprite(Texture2D texture, Vector2 position)
        {
            Position = position;
            Texture = texture;
            BoundX = (int)Center.X * 2;
            BoundY = (int)Center.Y * 2;
            Released += (_, _) =>
            {
                idk = false;
            };
            Clicked += (_, _) => idk = true;

        }
        public override void HandleHold(Point clickPosition)
        {
            if (!CanClick) return;
            if (idk || IsInBounds(clickPosition))
            {
                Position.X = clickPosition.X - Center.X;
                Position.Y = clickPosition.Y - Center.Y;
            }
            
        }
    }
}
