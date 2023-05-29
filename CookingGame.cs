using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CookingGame.States;
using CookingGame.Enum;
using System;
using CookingGame.Managers;
using Microsoft.Xna.Framework.Input;

namespace CookingGame;

public class CookingGame : Game
{
    private BaseState _currentGameState;

    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private const int ResolutionWidth = 1280;
    private const int ResolutionHeight = 720;

    private RenderTarget2D _nativeRenderTarget;
    private Rectangle _nativeWindowRectangle;
    private Rectangle _windowBoxingRect;
    private float WindowAspect => Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
    private float _nativeAspect;

    private SettingsManager _settingsManager;

    public CookingGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _settingsManager = new SettingsManager(_graphics);

        Content.RootDirectory = "Content";

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += UpdateWindowBoxingRect;
        Window.Title = "Моя шаурма";

        _graphics.IsFullScreen = _settingsManager.Settings.IsFullscreen;

        _nativeWindowRectangle = new Rectangle(0, 0, ResolutionWidth, ResolutionHeight); 
        _nativeAspect = _nativeWindowRectangle.Width / (float)_nativeWindowRectangle.Height;
            
        _graphics.PreferredBackBufferWidth = _nativeWindowRectangle.Width;
        _graphics.PreferredBackBufferHeight = _nativeWindowRectangle.Height;
        _graphics.SynchronizeWithVerticalRetrace = true; // Vsync, prevents screen tearing
        _graphics.HardwareModeSwitch = true;

        _graphics.ApplyChanges(); 

        _nativeRenderTarget = new RenderTarget2D(GraphicsDevice, _nativeWindowRectangle.Width, _nativeWindowRectangle.Height);
        _windowBoxingRect = _nativeWindowRectangle;

        base.Initialize();
    }

    private void UpdateWindowBoxingRect(object sender, EventArgs e) // Updates windowBoxingRect
    {
        // Calculates dimensions of black bars on sides of screen
        const float variance = 0f;
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

        if (WindowAspect <= _nativeAspect)
        {
            //Smaller output means taller than native, meaning top bars
            var presentHeight = (int)((windowWidth / _nativeAspect) + variance);
            var barHeight = (windowHeight - presentHeight) / 1;
            _windowBoxingRect = new Rectangle(0, barHeight, windowWidth, presentHeight);
        }
        else
        {
            //Larger output means wider than native, meaning side bars
            var presentWidth = (int)((windowHeight * _nativeAspect) + variance);
            var barWidth = (windowWidth - presentWidth) / 2;
            _currentGameState.InputManager.ChangeOffsetX(barWidth);
            _windowBoxingRect = new Rectangle(barWidth, 0, presentWidth, windowHeight);
        }
    }

    public Matrix GetResolutionMatrix() // Fixes the mouse
    {
        var xRatio = (float)_nativeWindowRectangle.Width / Window.ClientBounds.Width;
        var yRatio = (float)_nativeWindowRectangle.Height / Window.ClientBounds.Height;
        return Matrix.CreateTranslation(_windowBoxingRect.X, _windowBoxingRect.Y, 0) *
               Matrix.CreateScale(xRatio, yRatio, 0f);
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
        _currentGameState.SetSettings(_settingsManager);
        _currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
        _currentGameState.OnEventNotification += CurrentGameState_OnEventNotification;
    }

    private void CurrentGameState_OnEventNotification(object sender, Events e)
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
        base.Update(gameTime);
        _currentGameState.HandleInput();

        _currentGameState.Update(gameTime);
        UpdateMousePosition();

        if (Keyboard.GetState().IsKeyDown(Keys.E))
        {
            Exit();
        }

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
        GraphicsDevice.Clear(Color.WhiteSmoke);

        _spriteBatch.Begin(transformMatrix: _currentGameState.CameraManager.TransformMatrix);

        _currentGameState.Render(_spriteBatch);
            
        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null); // Sets target to back buffer
        GraphicsDevice.Clear(Color.Black); // Clears for black windowboxing

        _spriteBatch.Begin(SpriteSortMode.Immediate,
            null,
            SamplerState.PointClamp,
            DepthStencilState.Default,
            RasterizerState.CullNone);
        _spriteBatch.Draw(_nativeRenderTarget, _windowBoxingRect, Color.White); // Draw the _nativeRenderTarget as a texture
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}