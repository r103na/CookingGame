using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects.Base
{
    public abstract class ClickableSprite : BaseSprite
    {
        public bool canClick = true;

        public event EventHandler Clicked;
        public event EventHandler Held;
        public event EventHandler Released;
        public event EventHandler Hovered;

        protected int BoundX = 0;
        protected int BoundY = 0;

        protected Rectangle Bounds =>
            new(
                (int)Position.X - BoundX,
                (int)Position.Y - BoundY,
                _texture.Width + BoundX,
                _texture.Height + BoundY);

        public bool IsInBounds(Point mousePosition) => Bounds.Contains(mousePosition);

        public virtual void HandleClick(Point clickPosition)
        {
            if (IsInBounds(clickPosition) && canClick)
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
            if (IsInBounds(clickPosition) && canClick)
            {
                OnHeld();
            }
        }

        public virtual void HandleRelease(Point clickPosition)
        {
            if (IsInBounds(clickPosition) && canClick)
            {
                OnReleased();
            }
        }

        #region EVENT INVOCATORS
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

        #endregion
    }
}
