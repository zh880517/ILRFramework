using System.Collections.Generic;

public class MapPathFinderData
{
    public Dictionary<int, PathNode> Nodes { get; private set; } = new Dictionary<int, PathNode>();
    private PathFinderAlgorithm pathFinder;

    //当前寻路数据
    public FTBinaryHeap<PathNode> OpenHeap { get; private set; } = new FTBinaryHeap<PathNode>();
    public List<PathNode> CloseList { get; private set; } = new List<PathNode>();
    public List<PathNode> Path { get; private set; } = new List<PathNode>();
    public FP Radius { get; private set; }

    /*
    public bool Find(TSVector2 start, TSVector2 end)
    {

    }
    */

    //todo: 根据sdf和半径判断是否可以行走
    public bool CheckWalkable(PathNode node)
    {
        if (node == null)
            return false;
        return true;
    }

    public void InitBySDF(SDFRawData sdf)
    {

    }

    public PathNode TryGetOpenNode()
    {
        if (OpenHeap.Count > 0)
            return OpenHeap.Dequeue();
        return null;
    }

    public void Clear()
    {
        Path.Clear();
        while (OpenHeap.Count > 0)
        {
            var node = OpenHeap.RemoveAtEnd();
            node.Clear();
        }

        for (int i = 0; i < CloseList.Count; i++)
        {
            CloseList[i].Clear();
        }
        CloseList.Clear();
    }
}
