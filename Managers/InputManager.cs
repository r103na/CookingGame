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
        private List<ClickableSprite> clickableSprites = new List<ClickableSprite>();
        private MouseState mouseState;
        private MouseState lastMouseState;
        private KeyboardState keyboardState;

        public InputManager(List<ClickableSprite> GameObjects)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            clickableSprites = GameObjects;
        }

        public void UpdateGameObjects(List<ClickableSprite> GameObjects)
        {
            clickableSprites = GameObjects;
        }

        public void UpdateStates()
        {
            keyboardState = Keyboard.GetState();
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
        }

        public void HandleLeftClick()
        {
            if (LeftMouseButton(true))
            {
                var clickPosition = new Point(mouseState.X, mouseState.Y);
                clickableSprites.ToList().ForEach(x => x.HandleClick(clickPosition));
            }
        }

        public void HandleKey()
        {

        }
        public bool LeftMouseButton(bool single = false)
        {
            if (single) return mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released;
            else return (mouseState.LeftButton == ButtonState.Pressed);
        }
        public bool RightMouseButton(bool single = false)
        {
            if (single) return mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released;
            else return (mouseState.RightButton == ButtonState.Pressed);
        }
    }

}
