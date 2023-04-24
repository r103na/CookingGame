using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects.Base
{
    public class MovableSprite : ClickableSprite
    {
        protected Rectangle acceptableBounds;
        protected bool IsSpriteTaken;
        public MovableSprite(Texture2D texture, Vector2 position)
        {
            Position = position;
            Texture = texture;
            BoundX = (int)Center.X * 2;
            BoundY = (int)Center.Y * 2;
            Released += (_, _) =>
            {
                IsSpriteTaken = false;
            };
            Clicked += (_, _) => IsSpriteTaken = true;

        }
        public override void HandleHold(Point clickPosition)
        {
            if (!CanClick) return;
            if (IsSpriteTaken || IsInBounds(clickPosition))
            {
                Position.X = clickPosition.X - Center.X;
                Position.Y = clickPosition.Y - Center.Y;
            }
            
        }
    }
}
