public abstract class SDFShape
{
    public GridRect Bound { get; private set; }
    public TSVector2 Center { get; private set; }//相对于SDF地图原点的位置
    public int LayerMask { get; private set; }

    public SDFShape(TSVector2 center, GridRect bound)
    {
        Bound = bound;
        Center = center;
    }

    public void SetLayerMask(int layer)
    {
        LayerMask = layer;
    }

    public abstract FP SDValue(TSVector2 pos);
}
