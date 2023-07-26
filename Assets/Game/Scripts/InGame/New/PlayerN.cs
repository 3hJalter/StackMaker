using System.Collections.Generic;
using UnityEngine;

public class PlayerN : MonoBehaviour
{
    // Contain Brick
    [SerializeField] private GameObject brickContainer;
    private Stack<BrickN> _bricks;
    
    // [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 3f;
    [SerializeField] private bool isMoving;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;

    [SerializeField] private Vector3 destination;

    private Dictionary<Direction, Vector3> _direction;
    // Start is called before the first frame update
    private void Start()
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

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
            GetDestination();

    }

    private void OnInit()
    {
        _direction = new Dictionary<Direction, Vector3>{
            { Direction.Right, Vector3.right },
            { Direction.Left, Vector3.left },
            { Direction.Up, Vector3.forward },
            { Direction.Down, Vector3.back },
            { Direction.None, Vector3.zero}
        };
    }

    private void OnMoving()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, destination, speed * Time.deltaTime);
    }

    private Direction GetDirection()
    {
        // Temporary, need to change to control horizontal and vertical from mouse input
        return horizontal switch
        {
            > 0 => Direction.Right,
            < 0 => Direction.Left,
            _ => vertical switch
            {
                > 0 => Direction.Up,
                < 0 => Direction.Down,
                _ => Direction.None
            }
        };
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
            {
                continue;
            }
            destination -= direction;
            return;
        } while (true);
    }
}
