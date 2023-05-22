using System;
using System.Linq;

namespace CookingGame.Managers;

    public class PathManager
    {
        public static string GetPath()
        {
            var list = AppDomain.CurrentDomain.BaseDirectory.Split("\\").ToList();
            list.RemoveRange(list.Count - 4, 4);
            var result = string.Join("\\", list);
            return result;
        }
    }

