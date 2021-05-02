public struct DynamicCircle
{
    public FP Radius;
    public TSVector2 Center;

    public FP SDValue(TSVector2 pos)
    {
        return SDFUtils.SDCircle(pos, Center, Radius);
    }
}
