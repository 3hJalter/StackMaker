using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    public Vector3 RightDirection { get; private set; }
    public Vector3 ForwardDirection { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        var thisTransform = transform;
        ForwardDirection = thisTransform.forward;
        RightDirection = thisTransform.right;
    }

}
