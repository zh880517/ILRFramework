public struct GridRect
{
    public GridPoint min;
    public GridPoint max;

    public bool Contains(GridPoint point)
    {
        return point.x >= min.x && point.x < max.x && point.y >= min.y && point.y < max.y;
    }

    public bool Overlaps(GridRect other)
    {
        return other.max.x > min.x && other.min.x < max.x && other.max.y > min.y && other.min.y < max.y;
    }
}
