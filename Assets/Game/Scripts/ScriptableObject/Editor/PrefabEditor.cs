using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.ScriptableObjects.Editor
{
    [CustomEditor(typeof(Prefab))]
    public class PrefabEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var prefabList = (Prefab)target;

            if (!GUILayout.Button("Load Text Files")) return;
            var folderPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");

            if (string.IsNullOrEmpty(folderPath)) return;
            prefabList.prefab.Clear();

            var filePaths = Directory.GetFiles(folderPath, "*.prefab");

            foreach (var filePath in filePaths)
            {
                var relativePath = GetRelativeAssetPath(filePath);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);
                if (prefab != null)
                {
                    prefabList.prefab.Add(prefab);
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