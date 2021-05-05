public class SDCircle : SDFShape
{
    public FP Radius { get; private set; }

    public SDCircle(TSVector2 center, FP radius, RectInt bound) : base(center, bound)
    {
        Radius = radius;
    }

    public override FP SDValue(TSVector2 pos)
    {
        return SDFUtils.SDCircle(pos, Center, Radius);
    }
}
