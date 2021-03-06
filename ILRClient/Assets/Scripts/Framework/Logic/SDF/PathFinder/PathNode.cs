using System;
using UnityEngine;

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
    public const int Line = 10;
    public const int Tilted = 14;

    public int g; // 起点到节点代价
    public int h; // 节点到终点代价 估值
    public int f;
    public Vector2Int pos;
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

    public int CompareTo(PathNode refrence)
    {
        return f.CompareTo(refrence.f);
    }
    public static int ComputeH(PathNode ori, PathNode dest)
    {
        int xDelta = dest.pos.x > ori.pos.x ? dest.pos.x - ori.pos.x : ori.pos.x - dest.pos.x;
        int yDelta = dest.pos.y > ori.pos.y ? dest.pos.y - ori.pos.y : ori.pos.y - dest.pos.y;
        return (xDelta + yDelta) * 10;
    }
}
