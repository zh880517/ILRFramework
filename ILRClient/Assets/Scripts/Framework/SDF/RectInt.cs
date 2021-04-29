using UnityEngine;
public struct RectInt
{
    public Vector2Int min;
    public Vector2Int max;

    public bool Contains(Vector2Int point)
    {
        return point.x >= min.x && point.x < max.x && point.y >= min.y && point.y < max.y;
    }

    public bool Overlaps(RectInt other)
    {
        return other.max.x > min.x && other.min.x < max.x && other.max.y > min.y && other.min.y < max.y;
    }
}
