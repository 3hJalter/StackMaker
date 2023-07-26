using UnityEngine;
using UnityEditor;
using System.IO;
using Game.ScriptableObjects.Editor;

[CustomEditor(typeof(TextLevel))]
public class TextAssetListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var textAssetList = (TextLevel)target;

        if (!GUILayout.Button("Load Text Files")) return;
        var folderPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");

        if (string.IsNullOrEmpty(folderPath)) return;
        textAssetList.levelText.Clear();

        var filePaths = Directory.GetFiles(folderPath, "*.txt");

        foreach (var filePath in filePaths)
        {
            var relativePath = GetRelativeAssetPath(filePath);
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(relativePath);
            if (textAsset != null)
            {
                textAssetList.levelText.Add(textAsset);
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