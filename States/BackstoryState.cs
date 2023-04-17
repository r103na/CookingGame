using CookingGame.Enum;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

using CookingGame.Objects;
using CookingGame.Managers;
using CookingGame.Objects.Base;

namespace CookingGame.States
{
    public class BackstoryState : BaseState
    {
        private Queue<BackstoryImage> _backstoryImages = new();
        private BackstoryImage currentImage;
        private bool IsBackstoryOver => _backstoryImages.Count == 0;

        public override void LoadContent()
        {
            InputManager = new InputManager();
            //var image = new BackstoryImage();
            //LoadImage();
        }

        public override void Update()
        {
            
        }

        public void LoadImage()
        {
            currentImage = _backstoryImages.Dequeue();
            AddGameObject(currentImage);
        }

        public void SwitchImage()
        {
            if (!IsBackstoryOver)
            {
                RemoveGameObject(currentImage);
                LoadImage();
            }
            else
            {
                SwitchState(new GameplayState());
            }
        }

        public override void HandleInput()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.F))
            {
                SwitchImage();
            }
        }
    }
}
