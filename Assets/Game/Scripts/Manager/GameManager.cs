using System.Collections.Generic;
using Game.ScriptableObjects.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private int level;
    [SerializeField] private TextLevel textLevel;
    [SerializeField] private Prefab prefab;
    [SerializeField] private GameObject player;
    [SerializeField] private TextAsset loadedLevelTextAsset;
    [SerializeField] private List<GameObject> loadedPrefab;

    // For locate object
    [SerializeField] private Transform wallContainer;
    [SerializeField] private Transform brickContainer;
    [SerializeField] private Transform roadContainer;

    private Dictionary<int, Transform> _containerValue;

    public int[][] GeneratedMatrix { get; private set; }

    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        _containerValue = new Dictionary<int, Transform>
        {
            { (int)ObjectType.Wall, wallContainer },
            { (int)ObjectType.Brick, brickContainer },
            { (int)ObjectType.RoadNeedBrick, roadContainer }
        };
        loadedLevelTextAsset = textLevel.levelText[level];
        loadedPrefab = prefab.prefab;
        GeneratedMatrix = GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private int[][] GenerateMap()
    {
        var row = loadedLevelTextAsset.text.Split('\n');
        var matrix = new int[row.Length][]; // Declare the local 2D array to store the Vector3 values.
        for (var i = 0; i < row.Length; i++)
        {
            var colValues = row[i].Trim().Split(' ');
            matrix[i] = new int[colValues.Length];

            for (var j = 0; j < colValues.Length; j++)
            {
                if (!int.TryParse(colValues[j], out var cellValue)) continue;
                switch (cellValue)
                {
                    case (int) ObjectType.StartPoint:
                        Instantiate(loadedPrefab[(int) ObjectType.Brick], 
                                new Vector3(j, 0, i), Quaternion.identity)
                            .transform.SetParent(brickContainer);
                        Instantiate(player, 
                            new Vector3(j, 0, i), Quaternion.identity);
                        break;
                    case (int) ObjectType.EndPoint:
                        Instantiate(loadedPrefab[(int) ObjectType.Brick], 
                                new Vector3(j, 1, i), Quaternion.identity)
                            .transform.SetParent(brickContainer);
                        break;
                    case (int) ObjectType.Wall:
                    case (int) ObjectType.Brick:
                    case (int) ObjectType.RoadNeedBrick:
                        Instantiate(loadedPrefab[cellValue], 
                                new Vector3(j, 0, i), Quaternion.identity)
                            .transform.SetParent(_containerValue[cellValue]);
                        // ObjectPool.Instance.Spawn(loadedPrefab[cellValue].tag);
                        break;
                }
                matrix[i][j] = cellValue;
            }
        }

        return matrix;
    }

}
