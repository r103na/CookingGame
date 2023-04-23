using System;
using System.Linq;

using CookingGame.Managers;
using CookingGame.Objects;
using CookingGame.Objects.Base;

namespace CookingGame.States
{
    public class MenuState : BaseState
    {
        public override void LoadContent()
        {
            InputManager = new InputManager();
            var image = new ImageObject(LoadTexture("backgrounds/Menu"));
            image.Clicked += SwitchToGameplay;
            AddGameObject(image);
        }

        public override void Update()
        {
            
        }

        private void SwitchToGameplay(object sender, EventArgs e)
        {
            SwitchState(new GameplayState());
        }
    }
}
