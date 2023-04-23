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
        private Queue<ImageObject> _backstoryImages = new();
        private ImageObject _currentImageObject;
        private bool IsBackstoryOver => _backstoryImages.Count == 0;

        public override void LoadContent()
        {
            InputManager = new InputManager();
            //var image = new ImageObject();
            //LoadImage();
        }

        public override void Update()
        {
            
        }

        public void LoadImage()
        {
            _currentImageObject = _backstoryImages.Dequeue();
            AddGameObject(_currentImageObject);
        }

        public void SwitchImage()
        {
            if (!IsBackstoryOver)
            {
                RemoveGameObject(_currentImageObject);
                LoadImage();
            }
            else
            {
                SwitchState(new GameplayState());
            }
        }
    }
}
