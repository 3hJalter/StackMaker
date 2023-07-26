using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Init brick and brick container
    [SerializeField] private GameObject brickContainer;
    [SerializeField] private Brick firstBrick;

    // Brick stack
    private Stack<Brick> _bricks2;
    
    // Moving
    // [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 3f;
    [SerializeField] private bool isMoving;
    [SerializeField] private float horizontal;

    [SerializeField] private float vertical;

    // Check Moving
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private bool turnLeft;
    [SerializeField] private bool turnRight;
    [SerializeField] private bool turnUp;
    [SerializeField] private bool turnDown;
      
    // Handle with change direction object
    [SerializeField] private ChangeDirection changeDirection;
    
    // Layer
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask belowBrickCubeLayer;

    // Camera
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Transform cameraTarget;
    
    // Start is called before the first frame update
    private void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (isMoving) return;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Math.Abs(horizontal) > 0.01f || Math.Abs(vertical) > 0.01f)
            CheckMoving();
        else turnDown = turnRight = turnLeft = turnUp = false;
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;
        OnMoving();
        var pos = transform.position;
        if (pos == lastPosition) ResetMoving();
        lastPosition = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var tagName = collision.transform.tag;
        if (tagName != "Brick") return;
        var brick = collision.transform.GetComponent<Brick>();
        if (brick.holdByPlayer) return;
        brick.holdByPlayer = true;
        OnCollectBrick(collision.transform.GetComponent<Brick>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ChangeDirection")) return;
        if (changeDirection != null) return;
        changeDirection = other.GetComponent<ChangeDirection>();
    }

    private void OnInit()
    {
        // cameraFollow.isGameComplete = false; // Change to GameManager when done testing with camera
        // cameraFollow.isMovingCameraDone = false;
        // cameraFollow.target = cameraTarget;
        // cameraFollow.yTargetPos = cameraTarget.position.y;
        lastPosition = transform.position;
        firstBrick.DetachBelowBrickCube();
        _bricks2 = new Stack<Brick>();
        _bricks2.Push(firstBrick);
    }

    private void ResetMoving()
    {
        isMoving = false;
        turnRight = false;
        turnLeft = false;
        turnUp = false;
        turnDown = false;
    }

    private void ChangeMovingDirection(Vector3 direction)
    {
        changeDirection = null;
        isMoving = true;
        turnRight = false;
        turnLeft = false;
        turnUp = false;
        turnDown = false;
        if (direction == Vector3.forward) turnRight = true;
        else if (direction == Vector3.back) turnLeft = true;
        else if (direction == Vector3.left) turnUp = true;
        else if (direction == Vector3.right) turnDown = true;
        else isMoving = false;
    }

    private void CheckMoving()
    {
        isMoving = true;
        turnRight = horizontal > 0;
        turnLeft = horizontal < 0;
        turnUp = vertical > 0;
        turnDown = vertical < 0;
    }

    private void OnMoving()
    {
        Vector3 direction;
        if (turnRight) direction = Vector3.forward;
        else if (turnLeft) direction = Vector3.back;
        else if (turnUp) direction = Vector3.left;
        else if (turnDown) direction = Vector3.right;
        else direction = Vector3.zero;
        transform.Translate(direction * (speed * Time.deltaTime));
        // rb.velocity = direction * speed;
        HandleGoThroughBelowBrickCube();
        if (!IsCollisionWithWall(direction)) return;
        OnStopMoving();
        if (changeDirection != null)
        {
            ChangeMovingDirection(direction == -changeDirection.ForwardDirection
                ? changeDirection.RightDirection
                : changeDirection.ForwardDirection);
        }
            
        
    }

    private void OnStopMoving()
    {
        isMoving = false;
        // rb.velocity = Vector3.zero;
    }

    private void OnCollectBrick(Brick collectedBrick)
    {
        // Implement
        _bricks2.Push(collectedBrick);
        collectedBrick.transform.SetParent(brickContainer.transform);
        collectedBrick.DetachBelowBrickCube();
        // Move the collected brick below first brick
        var pos = firstBrick.transform.localPosition;
        const float offsetY = 0.3f; // height of a brick, need to modify by config
        collectedBrick.transform.localPosition = new Vector3(
            pos.x, pos.y - offsetY, pos.z);
        // Then make it as first brick
        firstBrick = collectedBrick;
        // Finally set new position for player += brick pos.y
        var playerTransform = transform;
        var pPos = playerTransform.localPosition;
        pPos = new Vector3(
            pPos.x, pPos.y + offsetY, pPos.z);
        playerTransform.localPosition = pPos;
    }

    private void OnDetachBrick()
    {
        // Remove the first brick from container;
        var detachBrick = _bricks2.Pop();
        // Change first brick to next brick;
        firstBrick = _bricks2.Peek();
        // Hide the detached brick
        detachBrick.gameObject.SetActive(false);
        // Set new position for player -= brick.pos.y
        const float offsetY = 0.3f; // height of a brick, need to modify by config
        var playerTransform = transform;
        var pPos = playerTransform.localPosition;
        pPos = new Vector3(
            pPos.x, pPos.y - offsetY, pPos.z);
        playerTransform.localPosition = pPos;
    }

    private bool IsCollisionWithWall(Vector3 direction)
    {
        // 0.5f = 1/2 length in world space (scale = 0.5) of a wall cube (or belowBrickCube)
        // need to modify (or get from a config) when changing their values
        var hit = Physics.Raycast(firstBrick.transform.position, direction, 
            0.5f, wallLayer);
        return hit;
    }

    private void HandleGoThroughBelowBrickCube()
    {
        // 0.2f > 1/2 length in world space (scale = 0.3) of a brick cube
        // need to modify (or get from a config) when changing their values
        Physics.Raycast(firstBrick.transform.position, Vector3.down, out var hit,
            0.2f, belowBrickCubeLayer);
        if (!hit.transform) return;
        var belowBrickCube = hit.transform.GetComponent<BelowBrickCube>();
        if (belowBrickCube == null) return;
        if (belowBrickCube.IsThrough || belowBrickCube.HasInitBrickParent || belowBrickCube.isSetColor) return;
        belowBrickCube.SetNewColor();
        OnDetachBrick();
    }
}