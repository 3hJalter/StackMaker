using UnityEngine;

public class RoadN : MonoBehaviour
{
    [SerializeField]
    private bool isGoThrough;

    private void SetNewColor()
    {
        var render = GetComponentInChildren<Renderer>(true);
        if (render != null) render.material.color = Color.cyan;
    }

    private void DetachBrickFromPlayer(PlayerN playerN)
    {
        if (!playerN.DetachBrick()) return;
        isGoThrough = true;
        SetNewColor();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isGoThrough) return;
        if (!other.CompareTag("Player")) return;
        
        DetachBrickFromPlayer(other.GetComponent<PlayerN>());
        
    }
}
