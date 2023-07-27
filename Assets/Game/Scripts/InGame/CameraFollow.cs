using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoSingleton<CameraFollow>
{
    public Transform target;
    public float yTargetPos;
    public Vector3 initOffset = new (11, 15 , 0);
    public float speed = 1000f;
    
    // Moving camera when game done
    // If can using Cinema-chine, remove all below variables
    public bool isGameComplete; // Change to GameManager when done testing with camera
    public bool isMovingCameraDone;
    [SerializeField] private Vector3 initRotation = new(45, -90, 0);
    [SerializeField] private Vector3 gameCompleteOffset = new(4.75f, 3.5f, 8f);
    [SerializeField] private Vector3 gameCompleteRotation = new(20f, -150f, 0f);
    // current offset and rotation
    [SerializeField] private Vector3 currentOffset;
    [SerializeField] private Vector3 currentRotation;
    // time for changing camera
    [SerializeField] private float cameraTimeChange = 1f;

    private Vector3 _directionBetweenInitAndComplete;
    
    // Start is called before the first frame update

    private void Start()
    {
        OnInit();
    }

    private void Update()
    {
        var targetPos = target.position;
        // Logic: currentOffset and currentRotation will be changed by time when isGameComplete = true
        // Below code is not using this logic, just directly change from init to complete offset and rotation
        if (!isGameComplete)
        {
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(targetPos.x, yTargetPos, targetPos.z) + currentOffset, 
                Time.deltaTime * speed);
            transform.rotation = Quaternion.Euler(currentRotation);
        }
        else {
            if (!isMovingCameraDone)
            {
                MovingCamera();
                return;
            }
            transform.position = Vector3.Lerp(transform.position, 
            new Vector3(targetPos.x, yTargetPos, targetPos.z) + gameCompleteOffset, 
            Time.deltaTime * speed);
            transform.rotation = Quaternion.Euler(gameCompleteRotation);
        }
    }

    private void OnInit()
    {
        currentOffset = initOffset;
        currentRotation = initRotation;
        cameraTimeChange = 1f;
    }
    
    private void MovingCamera()
    {
        isMovingCameraDone = true;
    }

    IEnumerator MovingCameraTest()
    {
        if (cameraTimeChange >= 0f)
        {
            cameraTimeChange -= Time.deltaTime;
            
        }
        yield return new WaitForSeconds(Time.deltaTime);    
    }
}
