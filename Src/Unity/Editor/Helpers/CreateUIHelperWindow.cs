using System.IO;
using SelfishFramework.Src.Unity.UI;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace SelfishFramework.Src.Unity.Editor.Helpers
{
    public class CreateUIHelperWindow : EditorWindow
    {
        private const string UIActors = "UI_Actors";
        private const string UIBluePrints = "UI_BluePrints";
        
        public string IdentifierName;
        
        [AssetsOnly]
        [OnValueChanged(nameof(SetNameOfIdentifierByPrfbName))]
        public GameObject UIprfb;
        
        public static void GetWindow()
        {
            var window = GetWindow<CreateUIHelperWindow>();
            window.titleContent = new GUIContent("UI Helper");
            window.Show();
        }

        [Button]
        public void CreateUI()
        {
            var pathBluePrints = InstallSelfish.DataPath + InstallSelfish.BLUE_PRINTS + InstallSelfish.UI_BLUE_PRINTS;
            var pathUIIdentifiers = InstallSelfish.DataPath + InstallSelfish.BLUE_PRINTS + InstallSelfish.IDENTIFIERS + InstallSelfish.UI_IDENTIFIERS;
            InstallSelfish.CheckFolder(pathBluePrints);
            InstallSelfish.CheckFolder(pathUIIdentifiers);
            
            //check all fields
            if (string.IsNullOrEmpty(IdentifierName))
            {
                ShowNotification(new GUIContent("Identifier name not set properly"));
                return;
            }

            if (File.Exists(pathUIIdentifiers + $"{IdentifierName}.asset"))
            {
                ShowNotification(new GUIContent("We already have identifier like this"));
                return;
            }

            if (UIprfb == null)
            {
                ShowNotification(new GUIContent("Set ui prefab in field"));
                return;
            }
            
            //Create objects of blueprints
            var uiidentifier = CreateInstance<UIIdentifier>();
            var uibluePrint = CreateInstance<UIBluePrint>();

            uiidentifier.name = IdentifierName;
            uibluePrint.name = $"{UIprfb.name}_UIBluePrint";
            
            //save SO to project
            AssetDatabase.CreateAsset(uibluePrint, InstallSelfish.ASSETS + InstallSelfish.BLUE_PRINTS + InstallSelfish.UI_BLUE_PRINTS + $"{uibluePrint.name}.asset");
            AssetDatabase.CreateAsset(uiidentifier, InstallSelfish.ASSETS + InstallSelfish.BLUE_PRINTS + InstallSelfish.IDENTIFIERS+InstallSelfish.UI_IDENTIFIERS + $"{uiidentifier.name}.asset");

        }
        
        private void SetNameOfIdentifierByPrfbName()
        {
            if (string.IsNullOrEmpty(IdentifierName) && UIprfb != null)
                IdentifierName = UIprfb.name + "_UIIdentifier";
        }
        
    }
}