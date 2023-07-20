using UnityEngine;

public class BelowBrickCube : MonoBehaviour
{
    [SerializeField] private bool isThrough;
    [SerializeField] private bool hasInitBrickParent;
    [SerializeField] private new Renderer renderer;
    public bool isSetColor;

    public bool IsThrough
    {
        get => isThrough;
        set => isThrough = value;
    }

    public bool HasInitBrickParent => hasInitBrickParent;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        renderer = GetComponent<Renderer>();
        var parent = transform.parent;
        if (parent != null) hasInitBrickParent = parent.CompareTag("Brick");
    }

    public void SetNewColor()
    {
        isSetColor = true;
        renderer.material.color = Color.cyan;
    }
}