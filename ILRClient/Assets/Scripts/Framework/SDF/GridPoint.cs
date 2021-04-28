public struct GridPoint
{
    public ushort x;
    public ushort y;

    public GridPoint(ushort x, ushort y)
    {
        this.x = x;
        this.y = y;
    }

    public int ToInt()
    {
        int i = x;
        return i << 16 | y;
    }
}
