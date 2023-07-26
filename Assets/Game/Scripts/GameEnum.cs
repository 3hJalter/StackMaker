public enum LayerMaskEnum
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Wall = 3,
    Water = 4,
    UI = 5,
    BelowBrickCube = 6,
    Brick = 7,
    Player = 8
}

public enum Direction
{
    Left = 0,
    Right = 1,
    Up = 2,
    Down = 3,
    None = 4
}

public enum ObjectType
{
    Wall = 0,
    Brick = 1,
    RoadNeedBrick = 2,
    StartPoint = 3,
    EndPoint = 4,
}