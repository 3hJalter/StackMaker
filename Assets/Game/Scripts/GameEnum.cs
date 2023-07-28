public enum Screen
{
    MainScreen = 0,
    InGameScreen = 1,
    VictoryScreen = 2,
}

public enum Popup
{
    SettingPopup = 0,
}

public enum PlayerState
{
    Idle = 0,
    Collect = 1,
    Victory = 2
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
    StartPoint = -2,
    None = -1,
    PivotWall = 0,
    PivotBrick = 1,
    RoadNeedBrick = 2,
    RoadNeedBrickRotate = 3,
    Brick = 4,
    Pivot = 5,
    PivotCaro = 6,
    PivotChess = 7,
    PivotEndPoint = 8,
    RoadNeedBrickNone = 9,
    RoadNeedBrickExpand = 10,
    RoadNeedBrickExpandRotate = 11,
}