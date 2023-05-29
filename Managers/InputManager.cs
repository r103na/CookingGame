using System.Collections.Generic;
using System.Linq;
using CookingGame.Objects;
using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CookingGame.Managers;

public class InputManager
{
    #region VARIABLES
    private List<ClickableSprite> _clickableSprites;
    private List<SplashImage> _splashImages;
    public MouseState MouseState { get; private set; }
    public Vector2 MousePosition { get; private set; }
    private MouseState _lastMouseState;
    private Point _offset;
    private int _offsetResized;

    public Point ScaledMousePosition;
    #endregion

    #region UPDATE
    public void UpdateGameObjects(List<BaseSprite> gameObjects)
    {
        _clickableSprites = gameObjects.OfType<ClickableSprite>().ToList();
        _splashImages = gameObjects.OfType<SplashImage>().ToList();
    }

    public void UpdateStates()
    {
        _lastMouseState = MouseState;
        MouseState = Mouse.GetState();
        MousePosition = new Vector2(MouseState.X, MouseState.Y);
    }

    public void UpdateMouseScale(Matrix transform)
    {
        var clientMouse = new Vector2(MouseState.X, MouseState.Y);
        var scaledMouseVector = Vector2.Transform(clientMouse, transform);
        ScaledMousePosition = new Point((int)scaledMouseVector.X - _offsetResized / 2, (int)scaledMouseVector.Y) - _offset;
    }

    public void ChangeOffset(Point offsetPoint)
    {
        _offset = offsetPoint;
    }

    public void ChangeOffsetX(int offset)
    {
        _offsetResized = offset;
    }

    #endregion

    #region HANDLE INPUT
    public void HandleInput(Matrix transform, List<BaseSprite> gameObjects)
    {
        UpdateMouseScale(transform);
        UpdateStates();
        UpdateGameObjects(gameObjects);
        HandleLeftClick();
        HandleHover();
        HandleUnhover();
        HandleHold();
        HandleReleased();
    }

    private void HandleLeftClick()
    {
        if (LeftMouseButton(true))
        {
            _clickableSprites.ToList().ForEach(x => x.HandleClick(ScaledMousePosition));
        }
    }

    private void HandleHold()
    {
        if (LeftMouseHeld())
        {
            _clickableSprites.ToList().ForEach(x => x.HandleHold(ScaledMousePosition));
        }
    }
    private void HandleHover()
    {
        _splashImages.ToList().ForEach(x => x.HandleHover(ScaledMousePosition));
        _clickableSprites.ToList().ForEach(x => x.HandleHover(ScaledMousePosition));
    }

    private void HandleUnhover()
    {
        _clickableSprites.ToList().ForEach(x => x.HandleUnhover(ScaledMousePosition));
    }

    private void HandleReleased()
    {
        if (LeftMouseButtonReleased(true))
            _clickableSprites.ToList().ForEach(x => x.HandleRelease(ScaledMousePosition));
    }
    #endregion

    #region MOUSE STATES
    public bool LeftMouseButtonWas()
    {
        return _lastMouseState.LeftButton == ButtonState.Pressed;
    }

    public bool LeftMouseButton(bool single = false)
    {
        if (single) return MouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released;
        return (MouseState.LeftButton == ButtonState.Pressed);
    }
    public bool LeftMouseButtonReleased(bool single = false)
    {
        if (single) return MouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed;
        return (MouseState.LeftButton == ButtonState.Released);
    }
    public bool RightMouseButton(bool single = false)
    {
        if (single) return MouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released;
        return (MouseState.RightButton == ButtonState.Pressed);
    }

    public bool LeftMouseHeld()
    {
        return LeftMouseButton() && LeftMouseButtonWas();
    }
    #endregion
}