#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SelfishFramework.Src.Unity.Editor.Helpers
{
    public class SelfishEditorWindow<SO> : EditorWindow
        where SO : ScriptableObject
    {
        protected SO Data;
        protected UnityEditor.Editor Editor;

        protected virtual void OnEnable()
        {
            if (Data == null)
            {
                Data = CreateInstance<SO>();
            }

            Editor = UnityEditor.Editor.CreateEditor(Data);
        }
        
        protected virtual void OnGUI()
        {
            if (Data == null)
                return;

            Editor.OnInspectorGUI();
        }
    }
}
#endif