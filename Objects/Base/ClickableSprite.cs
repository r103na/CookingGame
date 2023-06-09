﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CookingGame.Objects.Base;

public abstract class ClickableSprite : BaseSprite
{
    #region VARIABLES
    public bool CanClick = true;

    public event EventHandler Clicked;
    public event EventHandler Held;
    public event EventHandler Released;
    public event EventHandler Hovered;
    public event EventHandler Unhovered;
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
            Mouse.SetCursor(MouseCursor.Hand);
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

    public virtual void HandleUnhover(Point clickPosition)
    {
        if (!IsInBounds(clickPosition))
        {
            OnUnhovered();
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

    protected virtual void OnUnhovered()
    {
        Unhovered?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}