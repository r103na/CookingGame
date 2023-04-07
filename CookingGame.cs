using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using CookingGame.States;
using CookingGame.Enum;

namespace CookingGame
{
    public class CookingGame : Game
    {
        private BaseState _currentGameState;

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private RenderTarget2D _renderTarget;
        private Rectangle _renderScaleRectangle;
        private const int RESOLUTION_WIDTH = 1280;
        private const int RESOLUTION_HEIGHT = 720;

        private const float RESOLUTION_ASPECT_RATIO =
            RESOLUTION_WIDTH / (float)RESOLUTION_HEIGHT;


        public event EventHandler<Events> OnEventNotification;

        public CookingGame()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            
            _graphics.IsFullScreen = true;
            
            _graphics.ApplyChanges();

            _renderTarget = new RenderTarget2D(_graphics.GraphicsDevice,
                RESOLUTION_WIDTH, RESOLUTION_HEIGHT,
                false,
                SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);

            _renderScaleRectangle = GetScaleRectangle();

            base.Initialize();
        }

        private Rectangle GetScaleRectangle()
        {
            const double variance = 1.5;
            var actualAspectRatio = Window.ClientBounds.Width / (float)
                Window.ClientBounds.Height;
            Rectangle scaleRectangle;

            if (actualAspectRatio <= RESOLUTION_ASPECT_RATIO)
            {
                var presentHeight = (int)(Window.ClientBounds.Width /
                    RESOLUTION_ASPECT_RATIO + variance);
                var barHeight = (Window.ClientBounds.Height -
                                 presentHeight) / 2;
                scaleRectangle = new Rectangle(0, barHeight, Window.
                    ClientBounds.Width, presentHeight);
            }
            else
            {
                var presentWidth = (int)(Window.ClientBounds.Height *
                    RESOLUTION_ASPECT_RATIO + variance);
                var barWidth = (Window.ClientBounds.Width -
                                presentWidth) / 2;
                scaleRectangle = new Rectangle(barWidth, 0,
                    presentWidth, Window.ClientBounds.Height);
            }
            return scaleRectangle;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SwitchGameState(new MenuState());
        }
        private void CurrentGameState_OnStateSwitched(object sender, BaseState e)
        {
            SwitchGameState(e);
        }

        private void SwitchGameState(BaseState gameState)
        {
            _currentGameState?.UnloadContent();
            _currentGameState = gameState;
            _currentGameState.Initialize(Content);
            _currentGameState.LoadContent();
            _currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
            _currentGameState.OnEventNotification += _currentGameState_OnEventNotification;

        }

        private void _currentGameState_OnEventNotification(object sender, Enum.Events e)
        {
            switch (e)
            {
                case Events.GAME_QUIT:
                    Exit();
                    break;
            }
        }

        protected override void UnloadContent()
        {
            _currentGameState?.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _currentGameState.HandleInput();

            _currentGameState.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _currentGameState.Render(_spriteBatch);

            _spriteBatch.End();

            _graphics.GraphicsDevice.SetRenderTarget(null);

            _graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

            _spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}