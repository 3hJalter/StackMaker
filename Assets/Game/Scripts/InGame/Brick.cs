using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private BelowBrickCube belowBrickCube;
    public bool holdByPlayer; // Temporary fix for double collision with player, remove when fixed
    public void DetachBelowBrickCube()
    {
        if (belowBrickCube == null) return;
        belowBrickCube.transform.parent = null;
        belowBrickCube.IsThrough = true;
    }
}