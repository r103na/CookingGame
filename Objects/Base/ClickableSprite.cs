using System;
using Microsoft.Xna.Framework;

namespace CookingGame.Objects.Base
{
    public abstract class ClickableSprite : BaseSprite
    {
        public event EventHandler Clicked;
        public event EventHandler Held;
        public event EventHandler Released;
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

        public virtual void HandleClick(Point clickPosition)
        {
            if (IsInBounds(clickPosition))
            {
                OnClick();
            }
        }

        public virtual void HandleHover(Point clickPosition)
        {
            if (IsInBounds(clickPosition))
            {
                OnHover();
            }
        }

        public virtual void HandleHold(Point clickPosition)
        {
            if (IsInBounds(clickPosition))
            {
                OnHeld();
            }
        }

        public virtual void HandleRelease(Point clickPosition)
        {
            if (IsInBounds(clickPosition))
            {
                OnReleased();
            }
        }

        protected virtual void OnClick()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnHeld()
        {
            Held?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnReleased()
        {
            Released?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnHover()
        {
            Hovered?.Invoke(this, EventArgs.Empty);
        }
    }
}
