using System.IO;

namespace SelfishFramework.Src.Core.Features.Serialization
{
    public class JsonSave
    {
        public static bool TryLoadJson(string path, out string json)
        {
            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
                return true;
            }
            json = default;
            return false;
        }

        public static void SaveJson(string path, string json)
        {
            File.WriteAllText(path, json);
        }
    }
}