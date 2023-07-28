using UnityEngine;

public class RoadExpand : RoadN
{
    [SerializeField] private Renderer lineLeft;
    [SerializeField] private Renderer lineRight;
    public Color lineColor;
    // Start is called before the first frame update

    protected override void SetNewColor(Color color)
    {
        base.SetNewColor(color);
        lineLeft.material.color = lineColor;
        lineRight.material.color = lineColor;
    }
}
