using System.Collections.Generic;
using UnityEngine;

public class PlayerN : MonoBehaviour
{
    // Contain Brick
    [SerializeField] private Transform brickContainer;
    [SerializeField] private Transform model;
    
    // [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 7f;
    [SerializeField] private bool isMoving;

    [SerializeField] private Vector3 destination;

    // Camera
    [SerializeField] private Transform cameraTarget;
    private Stack<BrickN> _bricks;

    private Dictionary<Direction, Vector3> _direction;

    private Vector2 _mouseDirection;

    private Vector2 _mouseInputDown;

    private Vector2 _mouseInputUp;

    // Start is called before the first frame update
    private void Awake()
    {
        OnInit();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, destination) <= 0.01f)
            {
                isMoving = false;
                return;
            }
            OnMoving();
            return;
        }
        if (Input.GetMouseButtonDown(0)) _mouseInputUp = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (!Input.GetMouseButtonUp(0)) return;
        _mouseInputDown = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        _mouseDirection = _mouseInputUp - _mouseInputDown;
        if (_mouseDirection == Vector2.zero) return;
        var angle = Mathf.Atan2(-_mouseDirection.y, _mouseDirection.x);
        _mouseDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        GetDestination();
    }

    private void OnInit()
    {
        CameraFollow.Instance.isGameComplete = false; // Change to GameManager when done testing with camera
        CameraFollow.Instance.isMovingCameraDone = false;
        CameraFollow.Instance.target = cameraTarget;
        CameraFollow.Instance.yTargetPos = cameraTarget.position.y;
        _direction = new Dictionary<Direction, Vector3>
        {
            { Direction.Right, Vector3.right },
            { Direction.Left, Vector3.left },
            { Direction.Up, Vector3.forward },
            { Direction.Down, Vector3.back },
            { Direction.None, Vector3.zero }
        };
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
            if (mapValue is (int)ObjectType.Brick or (int)ObjectType.RoadNeedBrick
                or (int)ObjectType.StartPoint or (int)ObjectType.EndPoint)
                continue;
            destination -= direction;
            return;
        } while (true);
    }

    public void AttachBrick()
    {
        var offset = Vector3.up * 0.5f;
        Debug.Log("Attach Brick");
        var brick = Instantiate(GameManager.Instance.loadedPrefab[(int) ObjectType.Brick], 
            brickContainer.position - offset * (_bricks.Count + 1),
            brickContainer.rotation);
        brick.transform.SetParent(brickContainer);
        _bricks.Push(brick.GetComponent<BrickN>());
        ChangeModelPosition(offset);
    }

    public bool DetachBrick()
    {
        var offset = Vector3.down * 0.5f;
        if (_bricks.Count == 0)
        {
            var thisTransform = transform;
            var position = thisTransform.position;
            position = new Vector3(Mathf.Round(position.x), position.y, Mathf.Round(position.z));
            thisTransform.position = position;
            destination = position;
            isMoving = false;
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
}