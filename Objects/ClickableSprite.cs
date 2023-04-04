using System;
using Microsoft.Xna.Framework;

namespace CookingGame.Objects
{
    public abstract class ClickableSprite : BaseSprite
    {
        public event EventHandler Clicked;
        protected int BoundX = 0;
        protected int BoundY = 0;

        protected Rectangle Bounds => 
            new Rectangle(
                (int)_position.X - BoundX,
                (int)_position.Y - BoundY,
                _texture.Width + BoundX,
                _texture.Height + BoundY);

        protected bool IsInBounds(Point mousePosition) => Bounds.Contains(mousePosition);

        public abstract void HandleClick(Point clickPosition);
        public abstract void HandleRelease(Point clickPosition);


        protected void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
