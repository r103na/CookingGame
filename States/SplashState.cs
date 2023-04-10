using CookingGame.Objects;

using Microsoft.Xna.Framework.Input;

namespace CookingGame.States
{
    public class SplashState : BaseState
    {
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("backgrounds/win")));
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
