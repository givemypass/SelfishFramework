using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Unity.Identifiers;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SelfishFramework.Src.Unity.Features.InputFeature.Components
{
    [Serializable]
    public struct InputActionsComponent : IComponent
    {
        [SerializeField]
        private InputActionAsset _actions;
        
        [SerializeField, ListDrawerSettings]
        private List<InputActionSettings> _inputActionSettings;

        public InputActionAsset Actions => _actions;

        public bool TryGetInputAction(string name, out InputAction inputAction)
        {
            foreach (var a in _actions.actionMaps)
            {
                foreach (var action in a.actions)
                {
                    if (action.name == name)
                    {
                        inputAction = action;
                        return true;
                    }
                }
            }

            inputAction = null;
            return false;
        }

        public bool TryGetInputAction(int index, out InputAction inputAction)
        {
            var actionSetting = _inputActionSettings.FirstOrDefault(x => x.Identifier.Id == index);
            if (actionSetting == null)
            {
                inputAction = null;
                return false;
            }

            foreach (var a in _actions.actionMaps)
            {
                foreach (var action in a.actions)
                {
                    if (action.name == actionSetting.ActionName)
                    {
                        inputAction = action;
                        return true;
                    }
                }
            }

            inputAction = null;
            return false;
        }
        public int GetInputActionIndex(string name)
        {
            var actionSetting = _inputActionSettings.FirstOrDefault(x => x.ActionName == name);
            if (actionSetting == null)
                return -1;

            return actionSetting.Identifier.Id;
        }

        #region UnityEditor
#if UNITY_EDITOR
        private string SavePath => "Assets/" + "/Blueprints/" + "Identifiers/" + "InputIdentifiers/";

        [Button]
        private void FillInputActions()
        {
            var hashTest = new HashSet<string>();

            foreach (var map in _actions.actionMaps)
            {
                foreach (var action in map.actions)
                {
                    var check = _inputActionSettings.FirstOrDefault(x => x.ActionName == action.name);

                    if (check == null)
                        _inputActionSettings.Add(new InputActionSettings { ActionName = action.name });

                    hashTest.Add(action.name);
                }
            }

            foreach (var inputAction in _inputActionSettings.ToArray())
            {
                if (hashTest.Contains(inputAction.ActionName))
                    continue;
                _inputActionSettings.Remove(inputAction);
            }

            CreateAndFillIdentifiers();
        }

        private void CreateAndFillIdentifiers()
        {
            var inputIdentifiers = AssetDatabase.FindAssets("t:InputIdentifier")
               .Select(AssetDatabase.GUIDToAssetPath)
               .Select(AssetDatabase.LoadAssetAtPath<InputIdentifier>).ToList();

            CheckFolder(SavePath);

            foreach (var inputAction in _inputActionSettings)
            {
                if (inputAction.Identifier == null || inputAction.Identifier.name != inputAction.ActionName)
                {
                    var neededIdentifier = inputIdentifiers.FirstOrDefault(x => x.name == inputAction.ActionName);

                    if (neededIdentifier != null)
                    {
                        inputAction.Identifier = neededIdentifier;
                        continue;
                    }

                    inputAction.Identifier = CreateIdentifier(inputAction.ActionName);
                }
            }

            AssetDatabase.SaveAssets();
        }

        private InputIdentifier CreateIdentifier(string name)
        {
            var identifier = ScriptableObject.CreateInstance<InputIdentifier>();
            identifier.name = name;

            SaveIdentifier(identifier);
            return identifier;
        }

        private void SaveIdentifier(InputIdentifier inputIdentifier)
        {
            AssetDatabase.CreateAsset(inputIdentifier, SavePath + $"{inputIdentifier.name}.asset");
        }

        private static void CheckFolder(string path)
        {
            var folder = new DirectoryInfo(path);

            if (folder is not { Exists: true })
                Directory.CreateDirectory(path);
        }

#endif
        #endregion
    }

    [Serializable]
    public class InputActionSettings
    {
        [ReadOnly]
        public string ActionName;

        [ReadOnly]
        public InputIdentifier Identifier;
    }
}