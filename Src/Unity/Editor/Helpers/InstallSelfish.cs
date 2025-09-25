#if UNITY_EDITOR
using System.IO;
using UnityEditor;
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
        public const string SCRIPTS = "Scripts";
        public const string CORE = "Core";
        public const string COMMON_ACTORS = "CommonActors";
        public const string COMMON_COMMANDS = "CommonCommands";
        public const string COMMON_COMPONENTS = "CommonComponents";
        public const string COMMON_SYSTEMS = "CommonSystems";
        public const string FEATURES = "Features";
        public const string SERVICES = "Services";
        public const string CONTENT = "Content";
        
        public static string DataPath => Application.dataPath;

        [MenuItem("Selfish/Install Selfish Framework", priority = 1)]
        public static void Install()
        {
            CheckFolder(Path.Combine(DataPath, SCRIPTS, CORE));
            CheckFolder(Path.Combine(DataPath, SCRIPTS, CORE, COMMON_ACTORS));
            CheckFolder(Path.Combine(DataPath, SCRIPTS, CORE, COMMON_COMMANDS));
            CheckFolder(Path.Combine(DataPath, SCRIPTS, CORE, COMMON_COMPONENTS));
            CheckFolder(Path.Combine(DataPath, SCRIPTS, CORE, COMMON_SYSTEMS));
            CheckFolder(Path.Combine(DataPath, SCRIPTS, CORE, FEATURES));
            CheckFolder(Path.Combine(DataPath, SCRIPTS, CORE, SERVICES));
            CheckFolder(Path.Combine(DataPath, CONTENT));
            CheckFolder(Path.Combine(DataPath, BLUE_PRINTS));
            CheckFolder(Path.Combine(DataPath, BLUE_PRINTS, IDENTIFIERS));
            CheckFolder(Path.Combine(DataPath, BLUE_PRINTS, UI_BLUE_PRINTS));
        }
        
        public static void CheckFolder(string path)
        {
            var folder = new DirectoryInfo(path);

            if (!folder.Exists)
                Directory.CreateDirectory(path);
        }
    }
}
#endif