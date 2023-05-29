using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Managers;

public class SettingsManager
{
    public Settings Settings;
    private GraphicsDeviceManager _gDeviceManager;

    public SettingsManager(GraphicsDeviceManager graphicsDeviceManager)
    {
        LoadSavedSettings();
        _gDeviceManager = graphicsDeviceManager;
        UpdateSettings();
    }

    private void LoadSavedSettings()
    {
        var relativePath = Path.Combine("Data", "settings", "settings.json");
        var filePath = Path.Combine(PathManager.GetPath(), relativePath);
        var jsonString = File.ReadAllText(filePath);
        Settings = JsonSerializer.Deserialize<Settings>(jsonString);
    }

    public void UpdateSettings()
    {
        _gDeviceManager.IsFullScreen = Settings.IsFullscreen;
        _gDeviceManager.ApplyChanges();
    }

    public void UpdateSoundVolume(int volume)
    {
        Settings.SoundVolume = volume;
    }
    public void UpdateMusicVolume(int volume)
    {
        Settings.MusicVolume = volume;
    }

    public void UpdateFullscreen(bool fullscreen)
    {
        Settings.IsFullscreen = fullscreen;
    }
}

public class Settings
{
    public int SoundVolume = 100;
    public int MusicVolume = 100;
    public bool IsFullscreen = false;
}