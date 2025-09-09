#if UNITY_EDITOR
using System.IO;
using UnityEngine;

namespace SelfishFramework.Src.Unity.Editor.Helpers
{
    public class InstallSelfish : UnityEditor.Editor
    {
        public const string ASSETS = "Assets";
        public const string UI_IDENTIFIERS = "UIIdentifiers";
        public const string IDENTIFIERS = "Identifiers";
        public const string UI_BLUE_PRINTS = "UIBluePrints";
        public const string BLUE_PRINTS = "BluePrints";
        public static string DataPath => Application.dataPath;
        
        public static void CheckFolder(string path)
        {
            var folder = new DirectoryInfo(path);

            if (!folder.Exists)
                Directory.CreateDirectory(path);
        }
    }
}
#endif