using System;
using System.Linq;

using CookingGame.Managers;
using CookingGame.Objects;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;

namespace CookingGame.States
{
    public class MenuState : BaseState
    {
        public override void LoadContent()
        {
            var image = new ImageObject(LoadTexture("backgrounds/Menu"));
            image.Clicked += SwitchToGameplay;
            AddGameObject(image);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        private void SwitchToGameplay(object sender, EventArgs e)
        {
            SwitchState(new GameplayState());
        }
    }
}
