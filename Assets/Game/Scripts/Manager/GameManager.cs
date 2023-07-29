using System.Collections.Generic;
using Game.ScriptableObjects.Editor;
using UnityEngine;
using UnityEngine.Serialization;
public  enum  GameState{ MainMenu = 0, GamePlay = 1, Finish = 3}
public class GameManager : MonoSingleton<GameManager>
{
    public bool isInGameRunning;
    public bool isEndGame;
    // Win condition
    public bool isWin;
    public bool isReset;
    
    [SerializeField] private int level;

    public int Level
    {
        get => level;
        set => level = value;
    }

    [SerializeField] private TextLevel textLevel;

    public TextLevel TextLevel => textLevel;

    [SerializeField] private Prefab prefab;
    [SerializeField] private GameObject player;
    [SerializeField] private TextAsset loadedLevelTextAsset;
    [SerializeField] public List<GameObject> loadedPrefab;

    // For locate object
    [SerializeField] private Transform wallContainer;
    [SerializeField] private Transform brickContainer;
    [SerializeField] private Transform roadContainer;

    private Dictionary<int, Transform> _containerValue;

    private readonly List<Color> _colors = new()
    {
        new Color(1,0,0),
        new Color(1, 0.5f, 0),
        new Color(1,1,0),
        new Color(0.25f,1,0),
        new Color(0,1, 1),
        new Color(0,0,1),
        new Color(0.5f,0,1),
        new Color(1,0,1),
    };
    // if want constant color for each level, set _countColor = 0 on init
    private int _countColor;
    
    public int[][] GeneratedMatrix { get; private set; }

    // Game data

    private PlayerN Player { get; set; }

    private ChessOpen _chess;
    private EndPoint _endPoint;
    private List<RoadN> _roadNs;
    private List<BrickN> _brickNs;
    private List<GameObject> _walls;
    
    // Start is called before the first frame update

    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        base.Awake();
        level = PlayerPrefs.GetInt("level");
        OnInit();
    }

    public void OnInit()
    {
        // Reset bool value
        isEndGame = false;
        isWin = false;
        _containerValue = new Dictionary<int, Transform>
        {
            { (int)ObjectType.PivotWall, wallContainer },
            { (int)ObjectType.PivotCaro, wallContainer },
            { (int)ObjectType.Pivot, wallContainer },
            { (int)ObjectType.PivotEndPoint, wallContainer },
            { (int)ObjectType.PivotChess, wallContainer },
            { (int)ObjectType.PivotBrick, brickContainer },
            { (int)ObjectType.RoadNeedBrick, roadContainer },
            { (int)ObjectType.RoadNeedBrickRotate, roadContainer},
            { (int)ObjectType.RoadNeedBrickNone, roadContainer},
            { (int)ObjectType.RoadNeedBrickExpand, roadContainer},
            { (int)ObjectType.RoadNeedBrickExpandRotate, roadContainer},
        };
        // Setup map
        if (isReset) OnResetLevel();
        else OnLoadNewLevel();
        UIManager.Instance.OnInit();
        StartCoroutine(UIManager.Instance.ScaleUpWaitBg());
        CameraFollow.Instance.OnInit(Player);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // isReset = true;
            StartCoroutine(UIManager.Instance.ScaleDownWaitBg());
            // OnInit();
        }
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
                        var brickStart = Instantiate(loadedPrefab[(int) ObjectType.PivotBrick], 
                                new Vector3(j, 0, i), Quaternion.identity);
                        brickStart.transform.SetParent(_containerValue[(int) ObjectType.PivotBrick]);
                        _brickNs.Add(brickStart.GetComponentInChildren<BrickN>());
                        Player = Instantiate(player, 
                            new Vector3(j, 0, i), Quaternion.identity).GetComponent<PlayerN>();
                        break;
                    case (int) ObjectType.PivotEndPoint:
                        _endPoint = Instantiate(loadedPrefab[(int)ObjectType.PivotEndPoint],
                            new Vector3(j, 0, i), Quaternion.identity).GetComponent<EndPoint>();
                        break;
                    case (int) ObjectType.Pivot:
                    case (int) ObjectType.PivotCaro:
                    case (int) ObjectType.PivotWall:
                        var wall = Instantiate(loadedPrefab[cellValue],
                            new Vector3(j, 0, i), Quaternion.identity);
                        wall.transform.SetParent(_containerValue[cellValue]);
                        _walls.Add(wall);
                        // ObjectPool.Instance.Spawn(loadedPrefab[cellValue].tag);
                        break;
                    case (int) ObjectType.PivotBrick:
                        var brick = Instantiate(loadedPrefab[cellValue], 
                                new Vector3(j, 0, i), Quaternion.identity);
                        brick.transform.SetParent(_containerValue[cellValue]);
                        _brickNs.Add(brick.GetComponentInChildren<BrickN>());
                        // ObjectPool.Instance.Spawn(loadedPrefab[cellValue].tag);
                        break;
                    case (int) ObjectType.RoadNeedBrick:
                    case (int) ObjectType.RoadNeedBrickRotate:
                    case (int) ObjectType.RoadNeedBrickNone:
                        var road = Instantiate(loadedPrefab[cellValue],
                            new Vector3(j, 0, i), Quaternion.identity);
                        road.transform.SetParent(_containerValue[cellValue]);
                        var roadN = road.GetComponent<RoadN>();
                        _roadNs.Add(roadN);
                        // ObjectPool.Instance.Spawn(loadedPrefab[cellValue].tag);
                        break;
                    case (int) ObjectType.RoadNeedBrickExpand:
                    case (int) ObjectType.RoadNeedBrickExpandRotate:
                        var roadExpand = Instantiate(loadedPrefab[cellValue],
                            new Vector3(j, 0, i), Quaternion.identity);
                        roadExpand.transform.SetParent(_containerValue[cellValue]);
                        var roadExpandN = roadExpand.GetComponent<RoadExpand>();
                        if (_countColor >= _colors.Count) _countColor = 0;
                        roadExpandN.lineColor = _colors[_countColor];
                        _countColor++;
                        _roadNs.Add(roadExpandN);
                        // ObjectPool.Instance.Spawn(loadedPrefab[cellValue].tag);
                        break;
                    case (int) ObjectType.PivotChess:
                        // Temp Fix
                        var chest = Instantiate(loadedPrefab[cellValue],
                            new Vector3(j, 0, i), Quaternion.Euler(new Vector3(0, -90, 0)));
                        chest.transform.SetParent(_containerValue[cellValue]);
                        _chess = chest.GetComponent<ChessOpen>();
                        // ObjectPool.Instance.Spawn(loadedPrefab[cellValue].tag);
                        break;
                }
                matrix[i][j] = cellValue;
            }
        }
        Debug.Log(_brickNs.Count);
        Debug.Log(_roadNs.Count);
        return matrix;
    }

    public void OnWin()
    {
        isWin = true;
        UIManager.Instance.currentScreen.HideComponents();
        CameraFollow.Instance.isChangeCamera = true;
        level++;
        if (level >= textLevel.levelText.Count) level = 0;
        PlayerPrefs.SetInt("level", level);
        // OnInit();
    }

    public void OnLose()
    {
        isReset = true;
        UIManager.Instance.currentScreen.HideComponents();
        // Reset game logic
        Debug.Log("Lose");
        StartCoroutine(UIManager.Instance.ScaleDownWaitBg());
    }

    private void RemoveData()
    {
        foreach (var wall in _walls)
        {
            Destroy(wall);
        }
        _walls.Clear();
        // Wrong logic, root is brick container and road container
        foreach (var brick in _brickNs)
        {
            Destroy(brick.transform.gameObject);
        }
        _brickNs.Clear();
        foreach (var road in _roadNs)
        {
            Destroy(road.transform.gameObject);
        }
        _roadNs.Clear();
        Destroy(Player.gameObject);
        Destroy(_endPoint.gameObject);
        Destroy(_chess.gameObject);
    }
    
    private void OnLoadNewLevel()
    {
        // Remove Old Data
        if ((_roadNs != null && _roadNs.Count != 0) || 
            (_brickNs != null && _brickNs.Count != 0) ||
            (_walls != null && _walls.Count != 0)) RemoveData();
        // Setting new Data
        _brickNs = new List<BrickN>();
        _roadNs = new List<RoadN>();
        _walls = new List<GameObject>();
        loadedLevelTextAsset = textLevel.levelText[level];
        loadedPrefab = prefab.prefab;
        GeneratedMatrix = GenerateMap();
        Debug.Log("Load New Level");
    }

    private void OnResetLevel()
    {
        isReset = false;
        Player.transform.position = Player.InitPos;
        Player.OnInit();
        _endPoint.OnInit();
        _chess.OnInit();
        foreach (var road in _roadNs)
            road.OnInit();
        foreach (var brick in _brickNs)
            brick.OnInit();
        Debug.Log("Restart Level");
    }
}
