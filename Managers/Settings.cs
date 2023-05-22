using System.IO;
using System.Text.Json;

namespace CookingGame.Managers;

public class SettingsManager
{
    public Settings Settings;

    public SettingsManager()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        var relativePath = Path.Combine("Data", "settings", "settings.json");
        var filePath = Path.Combine(PathManager.GetPath(), relativePath);
        var jsonString = File.ReadAllText(filePath);
        Settings = JsonSerializer.Deserialize<Settings>(jsonString);
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