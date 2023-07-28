using System.IO;
using Game.ScriptableObjects.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.ScriptableObject.Editor
{
    [CustomEditor(typeof(ScreenUI))]
    public class ScreenUIEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var screenList = (ScreenUI)target;

            if (!GUILayout.Button("Load Text Files")) return;
            var folderPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");

            if (string.IsNullOrEmpty(folderPath)) return;
            screenList.screenList.Clear();

            var filePaths = Directory.GetFiles(folderPath, "*.prefab");

            foreach (var filePath in filePaths)
            {
                var relativePath = GetRelativeAssetPath(filePath);
                var screen = AssetDatabase.LoadAssetAtPath<BaseScreen>(relativePath);
                if (screen != null)
                {
                    screenList.screenList.Add(screen);
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