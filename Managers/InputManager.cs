﻿using System.Collections.Generic;
using System.Linq;

using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CookingGame.Managers
{
    public class InputManager
    {
        #region VARIABLES
        private List<ClickableSprite> _clickableSprites;
        public MouseState MouseState { get; private set; }
        private MouseState _lastMouseState;
        private KeyboardState _keyboardState;

        public Point ScaledMousePosition;
        #endregion

        #region UPDATE
        public void UpdateGameObjects(List<ClickableSprite> gameObjects)
        {
            _clickableSprites = gameObjects;
        }

        public void UpdateStates()
        {
            _keyboardState = Keyboard.GetState();
            _lastMouseState = MouseState;
            MouseState = Mouse.GetState();
        }

        public void UpdateMouseScale(Matrix transform)
        {
            var clientMouse = new Vector2(MouseState.X, MouseState.Y);
            var scaledMouseVector = Vector2.Transform(clientMouse, transform);
            ScaledMousePosition = new Point((int)scaledMouseVector.X, (int)scaledMouseVector.Y);
        }


        #endregion

        #region HANDLE INPUT
        public void HandleInput(Matrix transform, List<ClickableSprite> gameObjects)
        {
            UpdateMouseScale(transform);
            UpdateStates();
            UpdateGameObjects(gameObjects);
            HandleLeftClick();
            HandleHover();
            HandleHold();
            HandleReleased();
        }

        public void HandleLeftClick()
        {
            if (LeftMouseButton(true))
            {
                _clickableSprites.ToList().ForEach(x => x.HandleClick(ScaledMousePosition));
            }
        }

        public void HandleHold()
        {
            if (LeftMouseHeld())
            {
                _clickableSprites.ToList().ForEach(x => x.HandleHold(ScaledMousePosition));
            }
        }
        public void HandleHover()
        {
            //var mouse = new Point(MouseState.X, MouseState.Y);
            _clickableSprites.ToList().ForEach(x => x.HandleHover(ScaledMousePosition));
        }

        public void HandleReleased()
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

}
