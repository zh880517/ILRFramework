using System;
public enum NodeStatus
{
    Untest,
    Open,
    Close,
}
public enum DirectionType
{
    None = 0,
    Top = 1,
    Left = 2,
    Bottom = 4,
    Right = 8,
    TopLeft = 16,
    TopRight = 32,
    BottomLeft = 64,
    BottomRight = 128,
    All = 255,
}

public class PathNode: IComparable<PathNode>
{
    const int Line = 10;
    const int Tilted = 14;

    public int g; // 起点到节点代价
    public int h; // 节点到终点代价 估值
    public int f;

    public short x;
    public short y;
    public Neighbor neighbor;
    public NodeStatus status;
    public PathNode parent;
    public DirectionType dir; // 用于跳点搜索 跳点速度方向(父给的方向 + 自身带的方向)

    public void Clear()
    {
        parent = null;
        g = 0;
        h = 0;
        f = 0;
        status = NodeStatus.Untest;
        dir = DirectionType.None;
    }

    //待修改接口，需要传递sdf和碰撞半径
    public bool Walkable()
    {
        return true;
    }

    public int CompareTo(PathNode refrence)
    {
        return f.CompareTo(refrence.f);
    }
    public static int ComputeH(PathNode ori, PathNode dest)
    {
        int xDelta = dest.x > ori.x ? dest.x - ori.x : ori.x - dest.x;
        int yDelta = dest.y > ori.y ? dest.y - ori.y : ori.y - dest.y;
        return (xDelta + yDelta) * 10;
    }

    public static int ComputeGForAStar(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.Bottom:
            case DirectionType.Top:
            case DirectionType.Left:
            case DirectionType.Right:
                return Line;
            default:
                return Tilted;
        }
    }

    public static int ComputeGForJPS(DirectionType direction, PathNode ori, PathNode dest)
    {
        int xDelta, yDelta;
        switch (direction)
        {
            case DirectionType.Bottom:
            case DirectionType.Top:
                yDelta = dest.y > ori.y ? dest.y - ori.y : ori.y - dest.y;
                return yDelta * Line;
            case DirectionType.Left:
            case DirectionType.Right:
                xDelta = dest.x > ori.x ? dest.x - ori.x : ori.x - dest.x;
                return xDelta * Line;
            default:
                xDelta = dest.x > ori.x ? dest.x - ori.x : ori.x - dest.x;
                return xDelta * Tilted;
        }
    }

}
