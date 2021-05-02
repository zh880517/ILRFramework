using System.Collections.Generic;
using UnityEngine;

public static class SDFMapUtil
{

    //移动计算中用来筛选动态障碍物的列表缓存，防止重复创建销毁的GC
    private static readonly List<SDFShape> moveFilterCache = new List<SDFShape>();
    public static readonly List<DynamicCircle> CircleCaches = new List<DynamicCircle>();
    public static FP GetSDFFromShapes(List<SDFShape> sharpes, List<DynamicCircle> circles, TSVector2 pos, FP val)
    {
        if (sharpes != null && sharpes.Count > 0)
        {
            foreach (var shape in sharpes)
            {
                FP sd = shape.SDValue(pos);
                if (sd < val)
                    val = sd;
            }
        }
        if (circles != null && circles.Count > 0)
        {
            foreach (var circle in circles)
            {
                FP sd = circle.SDValue(pos);
                if (sd < val)
                    val = sd;
            }
        }
        return val;
    }

    public static FP Sample(this SDFMap map, TSVector2 pos, List<SDFShape> sharpes, List<DynamicCircle> circles)
    {
        FP val = map.SDF.Sample(pos);
        //如果已经在不可行走区域就没必要再计算碰撞体的了
        if (val < 0)
            return val;
        return GetSDFFromShapes(sharpes, circles, pos, val);
    }

    public static TSVector2 Gradient(this SDFMap map, List<SDFShape> sharpes, List<DynamicCircle> circles, TSVector2 pos)
    {
        FP delat = FP.ONE;
        FP x = map.Sample(new TSVector2(pos.x + delat, pos.y), sharpes, circles) - map.Sample(new TSVector2(pos.x - delat, pos.y), sharpes, circles);
        FP y = map.Sample(new TSVector2(pos.x, pos.y + delat), sharpes, circles) - map.Sample(new TSVector2(pos.x, pos.y - delat), sharpes, circles);
        return new TSVector2(FP.Half * x, FP.Half * y);
    }

    public static TSVector2 DiskCast(this SDFMap map, TSVector2 origin, TSVector2 dir, FP radius, FP maxDistance, List<SDFShape> sharpes, List<DynamicCircle> circles)
    {
        FP t = FP.Zero;
        while (true)
        {
            TSVector2 p = origin + dir * t;
            FP sd = map.Sample(p, sharpes, circles);
            if (sd <= radius)
                return p;
            t += (sd - radius);
            if (t >= maxDistance)
                return origin + dir * maxDistance;
        }
    }

    //直线移动，用于AI的移动
    public static TSVector2 StraightMove(this SDFMap map, List<DynamicCircle> circles, FP radius, TSVector2 start, TSVector2 dir, FP len, int layerMask = -1)
    {
        start = map.SDF.WorldToLocal(start);
        TSVector2 end = start + dir * len;
        RectInt rect = map.SDF.ToRect(start, end);
        int externSize = (int)TSMath.Ceiling(radius / map.SDF.Grain);
        rect.max += new Vector2Int(externSize, externSize);
        moveFilterCache.Clear();
        map.FilterToList(moveFilterCache, rect, layerMask);

        FP maxStepLen = radius * FP.Half;
        int moveStep = (int)TSMath.Ceiling(len / maxStepLen);
        TSVector2 result = start;
        for (int i = 1; i <= moveStep; ++i)
        {
            FP moveLen = maxStepLen;
            if (i == moveStep)
                moveLen = len - (moveStep - 1) * maxStepLen;
            TSVector2 newPos = result + dir * moveLen;
            FP sd = map.Sample(newPos, moveFilterCache, circles);
            if (sd < radius)
            {
                newPos = result + dir * sd;
                sd = map.Sample(newPos, moveFilterCache, circles);
                if (sd >= radius)
                {
                    result = newPos;
                }
                break;
            }
            result = newPos;
        }
        return result + map.SDF.Origin;
    }

    public static TSVector2 Move(this SDFMap map, List<DynamicCircle> circles, FP radius, TSVector2 start, TSVector2 dir, FP len, int layerMask = -1)
    {
        start = map.SDF.WorldToLocal(start);
        TSVector2 end = start + dir * len;
        RectInt rect = map.SDF.ToRect(start, end);
        int externSize = (int)TSMath.Ceiling(radius / map.SDF.Grain);
        rect.max += new Vector2Int(externSize, externSize);
        moveFilterCache.Clear();
        map.FilterToList(moveFilterCache, rect, layerMask);

        FP maxStepLen = radius * FP.Half;
        int moveStep = (int)TSMath.Ceiling(len / maxStepLen);
        TSVector2 result = start;
        for (int i=1; i<=moveStep; ++i)
        {
            FP moveLen = maxStepLen;
            if (i == moveStep)
                moveLen = len - (moveStep - 1) * maxStepLen;

            TSVector2 newPos = result + dir * moveLen;
            FP sd = map.Sample(newPos, moveFilterCache, circles);
            if (sd < radius)
            {
                TSVector2 gradient = map.Gradient(moveFilterCache, circles, newPos);
                TSVector2 asjustDir = dir - gradient * TSVector2.Dot(gradient, dir);
                newPos = result + asjustDir.normalized * moveLen;
                //多次迭代
                for (int j = 0; j < 3; ++j)
                {
                    sd = map.Sample(newPos, moveFilterCache, circles);
                    if (sd >= radius)
                        break;
                    newPos += map.Gradient(moveFilterCache, circles, newPos) * (radius - sd);
                }
                //避免往返
                if (TSVector2.Dot(newPos - start, dir) < FP.Zero)
                {
                    return result + map.SDF.Origin;
                }
                else
                {
                    result = newPos;
                }
                break;
            }
            else
            {
                result = newPos;
            }
        }
        return result + map.SDF.Origin;
    }
}
