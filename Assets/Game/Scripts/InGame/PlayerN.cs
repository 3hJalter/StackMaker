using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerN : MonoBehaviour
{
    // Brick container and Model
    [SerializeField] private Transform brickContainer;
    [SerializeField] private Transform model;

    [SerializeField] private Vector3 initPos;
    public Vector3 InitPos => initPos;

    // Animator
    [SerializeField] private Animator animator;
    private string _currentAnim;
    
    // Movement logic
    [SerializeField] private float speed = 7f;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector3 destination;
    // Movement input
    private Dictionary<Direction, Vector3> _direction;
    private Vector2 _mouseDirection;
    private Vector2 _mouseInputDown;
    private Vector2 _mouseInputUp;
    
    // Camera logic
    [SerializeField] private Transform cameraTarget;

    public Transform CameraTarget => cameraTarget;

    // Brick stack
    private Stack<BrickN> _bricks;

    // Start is called before the first frame update
    private void Awake()
    {
        OnInit();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.Instance.isInGameRunning) return;
        if (_currentAnim == Enum.GetName(PlayerState.Victory.GetType(), PlayerState.Victory)) return;
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, destination) <= 0.01f)
            {
                ChangeAnim(PlayerState.Idle);
                isMoving = false;
                return;
            }
            OnMoving();
            return;
        }

        if (GameManager.Instance.isEndGame) return;
        if (Input.GetMouseButtonDown(0)) _mouseInputUp = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (!Input.GetMouseButtonUp(0)) return;
        _mouseInputDown = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        _mouseDirection = _mouseInputUp - _mouseInputDown;
        if (_mouseDirection == Vector2.zero) return;
        var angle = Mathf.Atan2(-_mouseDirection.y, _mouseDirection.x);
        _mouseDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        GetDestination();

    }

    public void OnInit()
    {
        initPos = transform.position;
        model.localPosition = Vector3.up * 0.5f;
        ChangeAnim(PlayerState.Idle);
        _direction = new Dictionary<Direction, Vector3>
        {
            { Direction.Right, Vector3.right },
            { Direction.Left, Vector3.left },
            { Direction.Up, Vector3.forward },
            { Direction.Down, Vector3.back },
            { Direction.None, Vector3.zero }
        };
        if (_bricks is { Count: > 0 })
        {
            while (_bricks.Count > 0)
            {
                var brick = _bricks.Pop();
                Destroy(brick.gameObject);
            }
            _bricks.Clear();
        }
        _bricks = new Stack<BrickN>();
    }

    private void OnMoving()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, destination, speed * Time.deltaTime);
    }

    private Direction GetDirection()
    {
        if (Mathf.Abs(_mouseDirection.x) > Mathf.Abs(_mouseDirection.y))
            return _mouseDirection.x > 0 ? Direction.Left : Direction.Right;
        return _mouseDirection.y > 0 ? Direction.Up : Direction.Down;
    }

    private void GetDestination()
    {
        var position = transform.position;
        destination = new Vector3(Mathf.RoundToInt(position.x), 0, Mathf.RoundToInt(position.z));
        var direction = _direction[GetDirection()];
        if (direction == Vector3.zero)
        {
            isMoving = false;
            return;
        }
        isMoving = true;
        var map = GameManager.Instance.GeneratedMatrix;
        var rowLength = map.Length;
        var colLength = map[0].Length;
        do
        {
            destination += direction;
            var row = Mathf.RoundToInt(destination.z);
            var column = Mathf.RoundToInt(destination.x);
            if (row >= rowLength || column >= colLength)
            {
                destination -= direction;
                return;
            }
            var mapValue = map[row][column];
            // Need lose coupling 
            if (mapValue is (int)ObjectType.PivotBrick or (int)ObjectType.RoadNeedBrick or (int) ObjectType.Pivot
                or (int) ObjectType.RoadNeedBrickRotate or (int)ObjectType.StartPoint or (int)ObjectType.PivotEndPoint
                or (int) ObjectType.PivotCaro or (int) ObjectType.RoadNeedBrickNone or (int) ObjectType.RoadNeedBrickExpand 
                or (int) ObjectType.RoadNeedBrickExpandRotate )
                continue;
            destination -= direction;
            return;
        } while (true);
    }

    public void AttachBrick()
    {
        var offset = Vector3.up * 0.3f;
        Debug.Log("Attach Brick");
        var brick = Instantiate(GameManager.Instance.loadedPrefab[(int) ObjectType.Brick], 
            brickContainer.position - offset * (_bricks.Count + 1),
            brickContainer.rotation);
        brick.transform.SetParent(brickContainer);
        _bricks.Push(brick.GetComponent<BrickN>());
        ChangeModelPosition(offset);
        if (isMoving)
            ChangeAnim(PlayerState.Collect);
    }

    public bool DetachBrick()
    {
        var offset = Vector3.down * 0.3f;
        if (_bricks.Count == 0)
        {
            GameManager.Instance.isEndGame = true;
            var thisTransform = transform;
            var position = thisTransform.position;
            position = new Vector3(Mathf.Round(position.x), position.y, Mathf.Round(position.z));
            thisTransform.position = position;
            destination = position;
            isMoving = false;
            if (GameManager.Instance.isWin)
            {
                ChangeAnim(PlayerState.Victory);
                Debug.Log("Win");
                StartCoroutine(ShowVictoryScreen());
            }
            else
            {
                GameManager.Instance.OnLose();
            }
            return false;
        }
        var brick = _bricks.Pop();
        Debug.Log("Detach Brick");
        // Change to Polling if need optimize
        Destroy(brick.gameObject);
        ChangeModelPosition(offset);
        return true;
    }

    private void ChangeModelPosition(Vector3 offset)
    {
        var modelTransform = model.transform;
        var pos = modelTransform.position;
        modelTransform.position = pos + offset;
    }

    public void ChangeAnim(PlayerState state)
    {
        // var animName = Enum.GetName(state.GetType(), state);
        var animName = state.ToString();
        // if (_currentAnim == animName) return;
        animator.ResetTrigger(animName);
        _currentAnim = animName;
        animator.SetTrigger(_currentAnim);
    }

    private IEnumerator ShowVictoryScreen()
    {
        yield return new WaitForSeconds(2.5f);
        UIManager.Instance.currentScreen.ChangeScreen(Screen.VictoryScreen);
    }
}