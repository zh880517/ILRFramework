using UnityEngine;

public static class SDFUtils
{

    public static RectInt ToRect(this SDFRawData sdf, TSVector2 start, TSVector2 end)
    {
        TSVector2 min = new TSVector2(TSMath.Min(start.x, end.x), TSMath.Min(start.y, end.y));
        TSVector2 max = new TSVector2(TSMath.Max(start.x, end.x), TSMath.Max(start.y, end.y));

        return new RectInt(sdf.FloorToGrid(min), sdf.CeilingToGrid(max));
    }

    public static Vector2Int FloorToGrid(this SDFRawData sdf, TSVector2 positin)
    {
        positin /= sdf.Grain;
        return new Vector2Int((int)FP.Floor(positin.x), (int)FP.Floor(positin.y));
    }

    public static Vector2Int CeilingToGrid(this SDFRawData sdf, TSVector2 positin)
    {
        positin /= sdf.Grain;
        return new Vector2Int((int)FP.Ceiling(positin.x), (int)FP.Ceiling(positin.y));
    }

    /// <summary>
    /// SDF 采样
    /// </summary>
    /// <param name="sdf">sdf数据</param>
    /// <param name="pos">相对于sdf原点的位置</param>
    /// <returns>采样后对应位置的SD值</returns>
    public static FP Sample(this SDFRawData sdf, TSVector2 pos)
    {
        pos /= sdf.Grain;
        int x = (int)FP.Floor(pos.x);
        int y = (int)FP.Floor(pos.y);
        int idx = x + y * sdf.Width;
        FP rx = pos.x - x;
        FP ry = pos.y - y;
        //2 3
        //0 1
        FP v0 = sdf[idx];
        FP v1 = sdf[idx + 1];
        FP v2 = sdf[idx + sdf.Width];
        FP v3 = sdf[idx + sdf.Width + 1];

        return (v0 * (1 - rx) + v1 * rx) * (1 - ry) + (v2 * (1 - rx) + v3 * rx) * ry;
    }

    public static FP Sample(this SDFRawData sdf, FP posX, FP posY)
    {
        return Sample(sdf, new TSVector2(posX, posY));
    }

    public static TSVector2 WorldToLocal(this SDFRawData sdf, TSVector2 pos)
    {
        return pos - sdf.Origin;
    }

    //求位置的梯度方向
    public static TSVector2 Gradient(this SDFRawData sdf, TSVector2 pos)
    {
        FP delat = FP.ONE;
        FP x = Sample(sdf, pos.x + delat, pos.y) - Sample(sdf, pos.x - delat, pos.y);
        FP y = Sample(sdf, pos.x, pos.y + delat) - Sample(sdf, pos.x, pos.y - delat);
        return new TSVector2(FP.Half * x, FP.Half * y);
    }

    /// <summary>
    /// 获取指定方向移动的最佳距离
    /// </summary>
    /// <param name="sdf"></param>
    /// <param name="pos">起始位置</param>
    /// <param name="dir">方向</param>
    /// <param name="speed">速度</param>
    /// <param name="radius">碰撞半径</param>
    /// <returns></returns>
    public static TSVector2 GetVaildPositionBySDF(this SDFRawData sdf, TSVector2 pos, TSVector2 dir, FP speed, FP radius)
    {
        TSVector2 newPos = pos + dir * speed;
        FP sd = Sample(sdf, newPos);
        //距离障碍物太近，不可行走
        if(sd < radius)
        {
            TSVector2 gradient = Gradient(sdf, newPos);
            TSVector2 asjustDir = dir - gradient * TSVector2.Dot(gradient, dir);
            newPos = pos + asjustDir.normalized * speed;
            //多次迭代
            for (int i=0; i < 3; ++i)
            {
                sd = Sample(sdf, newPos);
                if (sd >= radius)
                    break;
                newPos += Gradient(sdf, newPos) * (radius - sd);
            }
            //避免往返
            if (TSVector2.Dot(newPos - pos, dir) < FP.Zero)
                newPos = pos;
        }
        return newPos;
    }

    //圆盘投射，获取最远可移动的位置
    public static TSVector2 DiskCast(this SDFRawData sdf, TSVector2 origin, TSVector2 dir, FP radius, FP maxDistance)
    {
        FP t = FP.Zero;
        while (true)
        {
            TSVector2 p = origin + dir * t;
            FP sd = Sample(sdf, p);
            if (sd <= radius)
                return p;
            t += (sd - radius);
            if (t >= maxDistance)
                return origin + dir * maxDistance;
        }
    }

    public static FP SDCircle(TSVector2 x, TSVector2 c, FP radius)
    {
        return TSVector2.Distance(x, c) - radius;
    }

    //不旋转的box
    public static FP SDBox(TSVector2 x, TSVector2 c, TSVector2 b)
    {
        TSVector2 p = x - c;
        p.x = TSMath.Abs(p.x);
        p.y = TSMath.Abs(p.y);
        TSVector2 d = p - b;
        return TSVector2.Max(d, TSVector2.zero).sqrMagnitude + TSMath.Min(TSMath.Max(d.x, d.y), FP.Zero);
    }

    //旋转的box
    public static FP SDOrientedBox(TSVector2 x, TSVector2 c, TSVector2 rot, TSVector2 b)
    {
        TSVector2 v = x - c;
        FP px = TSMath.Abs(TSVector2.Dot(v, rot));//在box的x轴的投影长度
        FP py = TSMath.Abs(TSVector2.Dot(v, new TSVector2(-rot.y, rot.x)));//在box的y轴的投影长度
        TSVector2 p = new TSVector2(px, py);
        TSVector2 d = p - b;
        return TSVector2.Max(d, TSVector2.zero).sqrMagnitude + TSMath.Min(TSMath.Max(d.x, d.y), FP.Zero);
    }

}
