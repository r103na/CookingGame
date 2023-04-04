using CookingGame.Enum;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookingGame.Objects;

namespace CookingGame.States
{
    public class BackstoryState : BaseState
    {
        private Queue<SplashImage> _backstoryImages = new();
        private SplashImage currentImage;
        private bool IsBackstoryOver => _backstoryImages.Count == 0;
        public override void LoadContent()
        {
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
