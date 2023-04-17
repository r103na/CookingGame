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
            var image = new BackstoryImage(LoadTexture("backgrounds/Menu"));
            image.Clicked += SwitchToGameplay;
            AddGameObject(image);
        }

        public override void Update()
        {
            
        }

        public override void HandleInput()
        {
            InputManager.HandleInput(TransformMatrix, GameObjects.OfType<ClickableSprite>().ToList());
        }

        private void SwitchToGameplay(object sender, EventArgs e)
        {
            SwitchState(new GameplayState());
        }
    }
}
