using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States
{
    public class TutorialState : BaseState
    {
        private Button _nextButton;
        private SplashImage _bg;
        public override void LoadContent()
        {
            SoundManager.LoadBackgroundMusic("music/menu");
            _bg = new SplashImage(LoadTexture("backgrounds/tutorial"));
            _nextButton = new Button(LoadTexture("gui/alertbutton"), new Vector2(1060, 660));
            _nextButton.Clicked += (_, _) =>
            {
                SoundManager.PlaySound("select");
                SwitchState(new GameplayState());
            };
            AddGameObject(_bg);
            AddGameObject(_nextButton);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
