using System;
using Microsoft.Xna.Framework;

namespace CookingGame.Objects.Base
{
    public abstract class ClickableSprite : BaseSprite
    {
        public event EventHandler Clicked;
        public event EventHandler Hovered;
        protected int BoundX = 0;
        protected int BoundY = 0;

        protected Rectangle Bounds =>
            new(
                (int)_position.X - BoundX,
                (int)_position.Y - BoundY,
                _texture.Width + BoundX,
                _texture.Height + BoundY);

        protected bool IsInBounds(Point mousePosition) => Bounds.Contains(mousePosition);

        public abstract void OnHover(Point clickPosition);
        public abstract void HandleClick(Point clickPosition);
        public abstract void HandleHold(Point clickPosition);
        public abstract void HandleRelease(Point clickPosition);


        protected void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnHovered()
        {
            Hovered?.Invoke(this, EventArgs.Empty);
        }
    }
}
