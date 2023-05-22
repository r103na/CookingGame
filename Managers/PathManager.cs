using System.IO;

namespace CookingGame.Managers;

public class PathManager
{
    public static string GetPath() => Directory.GetCurrentDirectory();
}

