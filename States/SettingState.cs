using System;
using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class SettingState : BaseState
{
    private Button fullscreenButton;
    private Button resetButton;
    private Button applyButton;
    private Button returnButton;
    public override void LoadContent()
    {
        fullscreenButton = new Button(LoadTexture("gui/stats"), new Vector2(640, 360));
        fullscreenButton.Clicked += FullscreenButton_Clicked;
        
        returnButton = new Button(LoadTexture("gui/stats"), new Vector2(640, 560));
        returnButton.Clicked += SwitchToMenu;

        AddGameObject(fullscreenButton);
        AddGameObject(returnButton);
    }

    private void FullscreenButton_Clicked(object sender, EventArgs e)
    {
        SettingsManager.UpdateFullscreen(true);
        SettingsManager.UpdateSettings();
    }

    public override void Update(GameTime gameTime)
    {
        OnUpdated(null, EventArgs.Empty);
    }

    private void SwitchToMenu(object sender, EventArgs e)
    {
        SwitchState(new MenuState());
    }
}