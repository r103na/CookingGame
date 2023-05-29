using System;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace CookingGame.Managers;

public class SettingsManager
{
    public Settings Settings;
    private readonly GraphicsDeviceManager _gDeviceManager;

    public SettingsManager(GraphicsDeviceManager graphicsDeviceManager)
    {
        LoadSavedSettings();
        _gDeviceManager = graphicsDeviceManager;
        UpdateSettings();
    }

    private static string GetSettingsPath()
    {
        var relativePath = Path.Combine("Data", "settings", "settings.json");
        return Path.Combine(PathManager.GetPath(), relativePath);
    }

    public void LoadSavedSettings()
    {
        var jsonString = File.ReadAllText(GetSettingsPath());
        Settings = JsonSerializer.Deserialize<Settings>(jsonString);
    }

    public void SaveSettings()
    {
        var settingsJson = JsonSerializer.Serialize(Settings);
        File.WriteAllText(GetSettingsPath(), settingsJson);
    }

    public void UpdateSettings()
    {
        _gDeviceManager.IsFullScreen = Settings.IsFullscreen;
        _gDeviceManager.ApplyChanges();
    }

    public void UpdateSoundVolume(int volume)
    {
        Settings.SoundVolume += volume;
    }
    public void UpdateMusicVolume(int volume)
    {
        Settings.MusicVolume += volume;
    }

    public void UpdateFullscreen(bool value)
    {
        Settings.IsFullscreen = value;
    }
}

[Serializable]
public class Settings
{
    public int SoundVolume { get; set; }
    public int MusicVolume { get; set; }
    public bool IsFullscreen { get; set; }
}