using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoSingleton<CameraFollow>
{
    public Transform target;
    public float yTargetPos;
    public Vector3 initOffset = new (0, 15 , -8.5f);
    public float speed = 1000f;
    
    // Moving camera when game done
    // If can using Cinema-chine, remove all below variables
    [FormerlySerializedAs("isGameComplete")] public bool isChangeCamera; // Change to GameManager when done testing with camera
    public bool isMovingCameraDone;
    [SerializeField] private Vector3 initRotation = new(60, 0, 0);
    [SerializeField] private Vector3 gameCompleteOffset = new(-5, 5, -10);
    [SerializeField] private Vector3 gameCompleteRotation = new(25, 25, 0);
    // current offset and rotation
    [FormerlySerializedAs("currentOffset")] [SerializeField] private Vector3 currentPositionOffset;
    [SerializeField] private Vector3 currentRotation;
    // time for changing camera
    [SerializeField] private float cameraTimeChange = 1f;

    private Vector3 _directionBetweenInitAndComplete;
    
    // Start is called before the first frame update

    private void Start()
    {
        // OnInit(GameManager.Instance.Player);
    }

    private void Update()
    {
        // if (target == null) return;
        var targetPos = target.position;
        // Logic: currentOffset and currentRotation will be changed by time when isGameComplete = true
        // Below code is not using this logic, just directly change from init to complete offset and rotation
        
        // if (!isChangeCamera)
        // {
        //     transform.position = Vector3.Lerp(transform.position, 
        //         new Vector3(targetPos.x, yTargetPos, targetPos.z) + currentOffset, 
        //         Time.deltaTime * speed);
        //     transform.rotation = Quaternion.Euler(currentRotation);
        // }
        // else {
        //     if (!isMovingCameraDone)
        //     {
        //         MovingCamera();
        //         return;
        //     }
        //     transform.position = Vector3.Lerp(transform.position, 
        //     new Vector3(targetPos.x, yTargetPos, targetPos.z) + gameCompleteOffset, 
        //     Time.deltaTime * speed);
        //     transform.rotation = Quaternion.Euler(gameCompleteRotation);
        // }
        transform.position = Vector3.Lerp(transform.position, 
            new Vector3(targetPos.x, yTargetPos, targetPos.z) + currentPositionOffset, 
            Time.deltaTime * speed);
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    private void FixedUpdate()
    {
        if (isChangeCamera) StartCoroutine(MovingCameraTest());
    }

    public void OnInit(PlayerN playerN)
    {
        currentPositionOffset = initOffset;
        currentRotation = initRotation;
        cameraTimeChange = 1f;
        isChangeCamera = false;
        isMovingCameraDone = false;
        target = playerN.CameraTarget;
        yTargetPos = playerN.CameraTarget.position.y;
    }
    
    private void MovingCamera()
    {
        isMovingCameraDone = true;
    }

    private IEnumerator MovingCameraTest()
    {
        isChangeCamera = false;
        var changePos = (gameCompleteOffset - initOffset) * Time.fixedDeltaTime;
        var changeRot = (gameCompleteRotation - initRotation) * Time.fixedDeltaTime;
        while (cameraTimeChange >= 0f)
        {
            cameraTimeChange -= Time.fixedDeltaTime;
            currentPositionOffset = new Vector3(currentPositionOffset.x + changePos.x, 
                currentPositionOffset.y + changePos.y, 
                currentPositionOffset.z + changePos.z);
            currentRotation = new Vector3(currentRotation.x + changeRot.x,
                currentRotation.y + changeRot.y, 
                currentRotation.z + changeRot.z);
            yield return new WaitForSeconds(Time.fixedDeltaTime);   
        }
    }
}
