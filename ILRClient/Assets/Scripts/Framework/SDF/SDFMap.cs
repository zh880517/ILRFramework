using System.Collections.Generic;

public class SDFMap
{
    public SDFRawData SDF { get; private set; }
    public List<SDFShape> DynamicObstacles { get; private set; } = new List<SDFShape>();

    public SDFMap(SDFRawData sdf)
    {
        SDF = sdf;
    }

    public GridPoint PosToGridPoint(TSVector2 pos)
    {
        pos /= SDF.Grain;
        ushort x = (ushort)FP.Floor(pos.x);
        ushort y = (ushort)FP.Floor(pos.y);
        return new GridPoint(x, y);
    }

    public FP Get(GridPoint pt, int layerMask = -1)
    {
        FP val = SDF.Get(pt);
        if (layerMask == 1)
        {
            TSVector2 pos = new TSVector2(pt.x * SDF.Grain, pt.y * SDF.Grain);
            foreach (var shape in DynamicObstacles)
            {
                if ((shape.LayerMask & layerMask) == 0)
                    continue;
                if (!shape.Bound.Contains(pt))
                    continue;
                FP sd = shape.SDValue(pos);
                if (sd < val)
                    val = sd;
            }
        }

        return val;
    }

}
