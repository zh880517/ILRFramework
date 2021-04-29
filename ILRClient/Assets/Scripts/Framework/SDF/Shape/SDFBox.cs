public class SDFBox : SDFShape
{

    public TSVector2 Rotation { get; private set; }

    public SDFBox(TSVector2 center, TSVector2 rotation, RectInt bound) : base(center, bound)
    {
        Rotation = rotation;
    }

    public override FP SDValue(TSVector2 pos)
    {
        return SDFUtils.SDBox(pos, Center, Rotation);
    }
}
