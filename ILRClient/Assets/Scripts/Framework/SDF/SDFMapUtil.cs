using System.Collections.Generic;

public static class SDFMapUtil
{
    //移动计算中用来筛选动态障碍物的列表缓存，防止重复创建销毁的GC
    private static List<SDFShape> moveFilterCache = new List<SDFShape>();
    public static bool Move(this SDFMap map, TSVector2 start, TSVector2 dir, FP len, out TSVector2 result)
    {
        start = map.SDF.WorldToLocal(start);
        TSVector2 end = start + dir * len;
        
        result = start;
        return true;
    }
}
