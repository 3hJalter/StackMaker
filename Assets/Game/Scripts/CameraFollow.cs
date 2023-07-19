using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float yTargetPos;
    public Vector3 offset;
    public float speed = 1000f;
    // Start is called before the first frame update

    private void Update()
    {
        var targetPos = target.position;
        transform.position = Vector3.Lerp(transform.position, 
            new Vector3(targetPos.x, yTargetPos, targetPos.z) + offset, 
            Time.deltaTime * speed);
    }
}
