using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework.Input;

namespace CookingGame.Managers
{
    public class InputManager
    {
        private List<ClickableSprite> _clickableSprites = new List<ClickableSprite>();
        private MouseState _mouseState;
        private MouseState _lastMouseState;
        private KeyboardState _keyboardState;

        public Vector2 scaledMouse;

        public InputManager(List<ClickableSprite> gameObjects)
        {
            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
            _clickableSprites = gameObjects;
        }

        public void UpdateGameObjects(List<ClickableSprite> gameObjects)
        {
            _clickableSprites = gameObjects;
        }

        public void UpdateStates()
        {
            _keyboardState = Keyboard.GetState();
            _lastMouseState = _mouseState;
            _mouseState = Mouse.GetState();
        }

        public void UpdateMouseScale(Vector2 clientMouse, Matrix transform)
        {
            scaledMouse = Vector2.Transform(clientMouse, transform);
        }

        public void HandleLeftClick()
        {
            if (LeftMouseButton(true))
            {
                var clickPosition = new Point(_mouseState.X, _mouseState.Y);
                _clickableSprites.ToList().ForEach(x => x.HandleClick(clickPosition));
            }
        }

        public void HandleHold()
        {
            if (LeftMouseButton())
            {
                var clickPosition = new Point(_mouseState.X, _mouseState.Y);
                _clickableSprites.ToList().ForEach(x => x.HandleHold(clickPosition));
            }
        }
        public void HandleKey()
        {

        }
        public bool LeftMouseButton(bool single = false)
        {
            if (single) return _mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released;
            else return (_mouseState.LeftButton == ButtonState.Pressed);
        }
        public bool RightMouseButton(bool single = false)
        {
            if (single) return _mouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released;
            else return (_mouseState.RightButton == ButtonState.Pressed);
        }
    }

}
