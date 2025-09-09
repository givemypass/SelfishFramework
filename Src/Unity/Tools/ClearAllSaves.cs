#if UNITY_EDITOR
using UnityEditor;

namespace SelfishFramework.Src.Unity.Tools
{
    public static class ClearAllSaves
    {
        [MenuItem("Selfish/Tools/ClearAllSaves")]
        public static void Clear()
        {
            var di = new System.IO.DirectoryInfo(UnityEngine.Application.persistentDataPath);
            foreach (var file in di.GetFiles())
            {
                file.Delete(); 
            }
            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true); 
            }
        } 
    }
}
#endif