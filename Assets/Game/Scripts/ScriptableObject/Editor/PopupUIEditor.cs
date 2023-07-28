using System.IO;
using Game.ScriptableObjects.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.ScriptableObject.Editor
{
    [CustomEditor(typeof(PopupUI))]
    public class PopupUIEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var popupList = (PopupUI)target;

            if (!GUILayout.Button("Load Text Files")) return;
            var folderPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");

            if (string.IsNullOrEmpty(folderPath)) return;
            popupList.popupList.Clear();

            var filePaths = Directory.GetFiles(folderPath, "*.prefab");

            foreach (var filePath in filePaths)
            {
                var relativePath = GetRelativeAssetPath(filePath);
                var popup = AssetDatabase.LoadAssetAtPath<BasePopup>(relativePath);
                if (popup != null)
                {
                    popupList.popupList.Add(popup);
                }
            }
        }

        private static string GetRelativeAssetPath(string absolutePath)
        {
            var applicationPath = Application.dataPath;
            if (absolutePath.StartsWith(applicationPath))
            {
                return "Assets" + absolutePath[applicationPath.Length..];
            }
            return null;
        }
    }
}