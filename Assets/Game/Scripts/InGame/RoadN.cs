using UnityEngine;

public class RoadN : MonoBehaviour
{
    [SerializeField]
    private bool isGoThrough;
    [SerializeField] private GameObject yellowRoad;
    [SerializeField] private Renderer render;

    public void OnInit()
    {
        isGoThrough = false;
        SetNewColor(new Color(0.7f, 0.6f, 1f, 1f));
    }
    
    protected virtual void SetNewColor(Color color)
    {
        if (render != null) render.material.color = color;
    }

    private void DetachBrickFromPlayer(PlayerN playerN)
    {
        if (!playerN.DetachBrick()) return;
        isGoThrough = true;
        SetNewColor(Color.yellow);
        yellowRoad.SetActive(true);
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (isGoThrough) return;
        if (!other.CompareTag("Player")) return;
        
        DetachBrickFromPlayer(other.GetComponent<PlayerN>());
        
    }
}
