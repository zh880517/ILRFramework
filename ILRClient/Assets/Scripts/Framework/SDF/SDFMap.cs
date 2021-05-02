using System.Collections.Generic;
using UnityEngine;

public class SDFMap
{
    public SDFRawData SDF { get; private set; }
    public List<SDFShape> Obstacles { get; private set; } = new List<SDFShape>();

    public SDFMap(SDFRawData sdf)
    {
        SDF = sdf;
    }

    public void FilterToList(List<SDFShape> shapes, RectInt rect, int layerMask = -1)
    {
        foreach (var shape in Obstacles)
        {
            if ((shape.LayerMask & layerMask) == 0)
                continue;
            if (!shape.Bound.Overlaps(rect))
                continue;
            shapes.Add(shape);
        }
    }

    public Vector2Int WorldPosFloorGridPoint(TSVector2 pos)
    {
        pos -= SDF.Origin;
        pos /= SDF.Grain;
        ushort x = (ushort)FP.Floor(pos.x);
        ushort y = (ushort)FP.Floor(pos.y);
        return new Vector2Int(x, y);
    }

    public Vector2Int WorldPosCeilingGridPoint(TSVector2 pos)
    {
        pos -= SDF.Origin;
        pos /= SDF.Grain;
        ushort x = (ushort)FP.Ceiling(pos.x);
        ushort y = (ushort)FP.Ceiling(pos.y);
        return new Vector2Int(x, y);
    }

    public TSVector2 GridPointToWorldPos(Vector2Int pt)
    {
        return new TSVector2(pt.x * SDF.Grain, pt.y * SDF.Grain) + SDF.Origin;
    }

    public FP Get(Vector2Int pt, int layerMask = -1)
    {
        FP val = SDF.Get(pt);
        if (layerMask == 1)
        {
            TSVector2 pos = new TSVector2(pt.x * SDF.Grain, pt.y * SDF.Grain);
            foreach (var shape in Obstacles)
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
