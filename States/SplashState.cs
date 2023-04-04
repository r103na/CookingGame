using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookingGame.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CookingGame.States
{
    public class SplashState : BaseState
    {
        private const string SplashTextureName = "Splash";

        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture(SplashTextureName)));
        }

        public override void Update()
        {
            
        }


        public override void HandleInput()
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter))
            {
                SwitchState(new MenuState());
            }
        }
    }
}
