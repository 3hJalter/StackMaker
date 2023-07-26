using UnityEngine;
using System.IO;

public class MatrixGenerator : MonoBehaviour
{
    public TextAsset matrixTextFile;
    // Other variables for object generation, if needed
    
    void Start()
    {
        if (matrixTextFile != null)
        {
            GenerateObjectsFromMatrix(matrixTextFile);
        }
        else
        {
            Debug.LogError("Matrix text file is not assigned!");
        }
    }

    private void GenerateObjectsFromMatrix(TextAsset textAsset)
    {
        string[] lines = textAsset.text.Split('\n');

        int startX = 0;
        int startY = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("StartX="))
            {
                int.TryParse(lines[i].Substring(7), out startX);
            }
            else if (lines[i].StartsWith("StartY="))
            {
                int.TryParse(lines[i].Substring(7), out startY);
            }
            else
            {
                string[] rowValues = lines[i].Trim().Split(' ');

                for (int j = 0; j < rowValues.Length; j++)
                {
                    int cellValue;
                    if (int.TryParse(rowValues[j], out cellValue))
                    {
                        // Create object based on the cellValue at position (startX + j, startY + i)
                        // For example:
                        // if (cellValue == 1)
                        //     Instantiate(object1, new Vector3(startX + j, startY + i, 0), Quaternion.identity);
                        // else if (cellValue == 2)
                        //     Instantiate(object2, new Vector3(startX + j, startY + i, 0), Quaternion.identity);
                        // ... and so on for other values
                    }
                }
            }
        }
    }
}