using System;

using Microsoft.Xna.Framework;

namespace CookingGame.Objects.Base
{
    public abstract class ClickableSprite : BaseSprite
    {
        #region VARIABLES
        public bool CanClick = true;

        public event EventHandler Clicked;
        public event EventHandler Held;
        public event EventHandler Released;
        public event EventHandler Hovered;
        #endregion

        #region BOUNDS
        protected int BoundX = 0;
        protected int BoundY = 0;

        protected Rectangle Bounds =>
            new(
                (int)Position.X - BoundX,
                (int)Position.Y - BoundY,
                Texture.Width + BoundX,
                Texture.Height + BoundY);
        public bool IsInBounds(Point mousePosition) => Bounds.Contains(mousePosition);
        #endregion

        #region INPUT HANDLERS
        public virtual void HandleClick(Point clickPosition)
        {
            if (IsInBounds(clickPosition) && CanClick)
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
            if (IsInBounds(clickPosition) && CanClick)
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
        #endregion

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
