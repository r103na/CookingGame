using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using CookingGame.Enum;
using CookingGame.Managers;
using CookingGame.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CookingGame.States
{
    public class MenuState : BaseState
    {
        private SpriteFont _spriteFont;

        private string[] menuItems =
        {
            "Start", "Exit"
        };

        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("backgrounds/Menu")));
        }

        public override void Update()
        {
            
        }

        public override void HandleInput()
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Q))
            {
                NotifyEvent(Events.GAME_QUIT);
            }

            if (keyboardState.IsKeyDown(Keys.G))
            {
                SwitchState(new GameplayState());
            }

            if (keyboardState.IsKeyDown(Keys.B))
            {
                SwitchState(new BackstoryState());
            }
        }
    }
}
