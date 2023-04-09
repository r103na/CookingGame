using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CookingGame.States;
using CookingGame.Enum;
using System;

namespace CookingGame
{
    public class CookingGame : Game
    {
        private BaseState _currentGameState;

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private RenderTarget2D _renderTarget;
        private Rectangle _renderScaleRectangle;
        private const int ResolutionWidth = 1280;
        private const int ResolutionHeight = 720;

        private const float ResolutionAspectRatio =
            ResolutionWidth / (float)ResolutionHeight;

        RenderTarget2D _nativeRenderTarget;
        Rectangle nativeWindowRectangle;
        Rectangle windowBoxingRect;
        float WindowAspect => Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
        float nativeAspect;

        public CookingGame()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += UpdateWindowBoxingRect;
            //Window.ClientSizeChanged += UpdateMousePosition;

            nativeWindowRectangle = new Rectangle(0, 0, ResolutionWidth, ResolutionHeight); // x y w h
            nativeAspect = nativeWindowRectangle.Width / (float)nativeWindowRectangle.Height;
            
            _graphics.PreferredBackBufferWidth = nativeWindowRectangle.Width;
            _graphics.PreferredBackBufferHeight = nativeWindowRectangle.Height;
            _graphics.SynchronizeWithVerticalRetrace = true; // Vsync, prevents screen tearing
            _graphics.HardwareModeSwitch = true; // false is borderless window fullscreen, true is true fullscreen
            //_graphics.IsFullScreen = true;
            
            _graphics.ApplyChanges(); // Makes the magic happen

            _nativeRenderTarget = new RenderTarget2D(GraphicsDevice, nativeWindowRectangle.Width, nativeWindowRectangle.Height);
            windowBoxingRect = nativeWindowRectangle;

            _renderTarget = new RenderTarget2D(_graphics.GraphicsDevice,
                ResolutionWidth, ResolutionHeight,
                false,
                SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);

            _renderScaleRectangle = GetScaleRectangle();

            base.Initialize();
        }

        private Rectangle GetScaleRectangle()
        {
            const double variance = 0.5;
            var actualAspectRatio = Window.ClientBounds.Width / (float)
                Window.ClientBounds.Height;
            Rectangle scaleRectangle;

            if (actualAspectRatio <= ResolutionAspectRatio)
            {
                var presentHeight = (int)(Window.ClientBounds.Width /
                    ResolutionAspectRatio + variance);
                var barHeight = (Window.ClientBounds.Height -
                                 presentHeight) / 1;
                scaleRectangle = new Rectangle(0, barHeight, Window.
                    ClientBounds.Width, presentHeight);
            }
            else
            {
                var presentWidth = (int)(Window.ClientBounds.Height *
                    ResolutionAspectRatio + variance);
                var barWidth = (Window.ClientBounds.Width -
                                presentWidth) / 1;
                scaleRectangle = new Rectangle(barWidth, 0,
                    presentWidth, Window.ClientBounds.Height);
            }
            return scaleRectangle;
        }

        private void UpdateWindowBoxingRect(object sender, EventArgs e) // Updates windowBoxingRect
        {
            // Calculates dimensions of black bars on sides of screen
            const float variance = 0.5f;
            int windowWidth, windowHeight;
            if (_graphics.IsFullScreen)
            {
                windowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                windowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                windowWidth = Window.ClientBounds.Width;
                windowHeight = Window.ClientBounds.Height;
            }

            if (WindowAspect <= nativeAspect)
            {
                //Smaller output means taller than native, meaning top bars
                int presentHeight = (int)((windowWidth / nativeAspect) + variance);
                int barHeight = (windowHeight - presentHeight) / 1;
                windowBoxingRect = new Rectangle(0, barHeight, windowWidth, presentHeight);
            }
            else
            {
                //Larger output means wider than native, meaning side bars
                int presentWidth = (int)((windowHeight * nativeAspect) + variance);
                int barWidth = (windowWidth - presentWidth) / 2;
                windowBoxingRect = new Rectangle(barWidth, 0, presentWidth, windowHeight);
            }
        }

        public Matrix GetResolutionMatrix() // Fixes the mouse
        {
            float xRatio = (float)nativeWindowRectangle.Width / Window.ClientBounds.Width;
            float yRatio = (float)nativeWindowRectangle.Height / Window.ClientBounds.Height;
            return Matrix.CreateTranslation(windowBoxingRect.X, windowBoxingRect.Y, 0) *
                   Matrix.CreateScale(xRatio, yRatio, 1f);
        }

        public void UpdateMousePosition()
        {
            _currentGameState.TransformMatrix = GetResolutionMatrix();
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
            UpdateMousePosition();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
            GraphicsDevice.Clear(Color.WhiteSmoke);

            _spriteBatch.Begin();

            _currentGameState.Render(_spriteBatch);
            
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null); // Sets target to back buffer
            GraphicsDevice.Clear(Color.Black); // Clears for black windowboxing

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null);
            _spriteBatch.Draw(_nativeRenderTarget, windowBoxingRect, Color.White); // Draw the _nativeRenderTarget as a texture
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}