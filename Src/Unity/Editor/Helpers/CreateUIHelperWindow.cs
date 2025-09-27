#if UNITY_EDITOR
using System;
using System.IO;
using SelfishFramework.Src.Unity.UI;
using SelfishFramework.Src.Unity.UI.Systems;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SelfishFramework.Src.Unity.Editor.Helpers
{
    [Serializable]
    public class CreateUIHelperWindow : EditorWindow
    {
        private SO _data;
        private UnityEditor.Editor _editor;

        [MenuItem("Selfish/Helpers/Create UI Window")]
        public static void GetWindow()
        {
            var window = GetWindow<CreateUIHelperWindow>("UI Helper");
            window.Show();
        }

        private void OnEnable()
        {
            if (_data == null)
            {
                _data = CreateInstance<SO>();
                _data.EditorWindow = this;
            }

            _editor = UnityEditor.Editor.CreateEditor(_data);
        }
        
        private void OnGUI()
        {
            if (_data == null)
                return;

            _editor.OnInspectorGUI();
        }


        private class SO : ScriptableObject
        {
            private const string UI_ACTORS = "UI_Actors";
            private const string UI_BLUE_PRINTS = "UI_BluePrints";

            public string IdentifierName;

            [AssetsOnly]
            [OnValueChanged(nameof(SetNameOfIdentifierByPrfbName))]
            public GameObject Prefab;

            public CreateUIHelperWindow EditorWindow { get; set; }

            [Button]
            public void CreateUI()
            {
                var pathBluePrints = Path.Combine(InstallSelfish.DataPath, InstallSelfish.BLUE_PRINTS, InstallSelfish.UI_BLUE_PRINTS);
                var pathUIIdentifiers = Path.Combine(InstallSelfish.DataPath, InstallSelfish.BLUE_PRINTS, InstallSelfish.IDENTIFIERS, InstallSelfish.UI_IDENTIFIERS);
                
                InstallSelfish.CheckFolder(pathBluePrints);
                InstallSelfish.CheckFolder(pathUIIdentifiers);

                //check all fields
                if (string.IsNullOrEmpty(IdentifierName))
                {
                    EditorWindow.ShowNotification(new GUIContent("Identifier name not set properly"));
                    return;
                }

                if (File.Exists(pathUIIdentifiers + $"{IdentifierName}.asset"))
                {
                    EditorWindow.ShowNotification(new GUIContent("We already have identifier like this"));
                    return;
                }

                if (Prefab == null)
                {
                    EditorWindow.ShowNotification(new GUIContent("Set ui prefab in field"));
                    return;
                }

                //Crete objects of blueprints
                var identifier = CreateInstance<UIIdentifier>();
                var bluePrint = CreateInstance<UIBluePrint>();

                identifier.name = IdentifierName;
                bluePrint.name = $"{Prefab.name}_UIBluePrint";

                AssetDatabase.CreateAsset(bluePrint, Path.Combine(
                    InstallSelfish.ASSETS, InstallSelfish.BLUE_PRINTS, InstallSelfish.UI_BLUE_PRINTS,
                    $"{bluePrint.name}.asset"));
                
                AssetDatabase.CreateAsset(identifier, Path.Combine(
                    InstallSelfish.ASSETS, InstallSelfish.BLUE_PRINTS, InstallSelfish.IDENTIFIERS,
                    InstallSelfish.UI_IDENTIFIERS, $"{identifier.name}.asset"));

                var assetEntry = AddressablesHelpers.SetAddressableGroup(Prefab, UI_ACTORS);

                var uiBluePrintEntry = AddressablesHelpers.SetAddressableGroup(bluePrint, UI_BLUE_PRINTS);
                AddressablesHelpers.AddLabel(uiBluePrintEntry, UIService.UI_BLUE_PRINT);

                //assign fields of blueprints
                bluePrint.UIType = identifier;
                bluePrint.UIActor = new AssetReference(assetEntry.guid);

                EditorUtility.SetDirty(bluePrint);
                EditorUtility.SetDirty(Prefab);
                EditorUtility.SetDirty(identifier);

                AssetDatabase.SaveAssets();
            }

            private void SetNameOfIdentifierByPrfbName()
            {
                if (string.IsNullOrEmpty(IdentifierName) && Prefab != null)
                    IdentifierName = Prefab.name + "_UIIdentifier";
            }
        }
    }
}
#endif