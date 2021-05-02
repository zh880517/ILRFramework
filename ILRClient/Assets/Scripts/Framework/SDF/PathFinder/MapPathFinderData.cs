using System.Collections.Generic;
using UnityEngine;

public class MapPathFinderData
{
    public Dictionary<int, PathNode> Nodes { get; private set; } = new Dictionary<int, PathNode>();
    private PathFinderAlgorithm pathFinder;

    //当前寻路数据
    public FTBinaryHeap<PathNode> OpenHeap { get; private set; } = new FTBinaryHeap<PathNode>();
    public List<PathNode> CloseList { get; private set; } = new List<PathNode>();
    public List<PathNode> Path { get; private set; } = new List<PathNode>();
    public FP Radius { get; private set; }
    private SDFMap map;

    
    public bool Find(TSVector2 start, TSVector2 end, List<TSVector2> path)
    {
        var startNode = GetNode(start);
        var endNode = GetNode(end);
        if (startNode == null || endNode == null)
            return false;
        if (!pathFinder.FindPath(startNode, endNode))
            return false;
        if (path != null)
        {
            for (int i=Path.Count-1; i>=0; --i)
            {
                path.Add(map.GridPointToWorldPos(Path[i].pos));
            }
        }
        return true;
    }
    
    public PathNode GetNode(TSVector2 worldPos)
    {
        var pos = map.WorldPosFloorGridPoint(worldPos);
        Nodes.TryGetValue(pos.x << 16 | pos.y, out var node);
        return node;
    }


    public void Init(SDFMap map)
    {
        this.map = map;
        Nodes.Clear();
        for (int i=0; i<map.SDF.Width; ++i)
        {
            for (int j=0; j<map.SDF.Heigh; ++j)
            {
                if (map.SDF[i, j] >= 0)
                {
                    PathNode node = new PathNode
                    {
                        pos = new Vector2Int(i, j)
                    };
                    int key = i << 16 | j;
                    Nodes.Add(key, node);
                }
            }
        }
        foreach (var kv in Nodes)
        {
            var node = kv.Value;
            Vector2Int pos = node.pos;
            int key = pos.x << 16 | (pos.y + 1);
            Nodes.TryGetValue(key, out node.neighbor.top);

            key = pos.x << 16 | (pos.y - 1);
            Nodes.TryGetValue(key, out node.neighbor.bottom);

            key = (pos.x - 1) << 16 | pos.y;
            Nodes.TryGetValue(key, out node.neighbor.left);

            key = (pos.x + 1) << 16 | pos.y;
            Nodes.TryGetValue(key, out node.neighbor.right);

            key = (pos.x - 1) << 16 | (pos.y + 1);
            Nodes.TryGetValue(key, out node.neighbor.topLeft);

            key = (pos.x + 1) << 16 | (pos.y + 1);
            Nodes.TryGetValue(key, out node.neighbor.topRight);

            key = (pos.x - 1) << 16 | (pos.y - 1);
            Nodes.TryGetValue(key, out node.neighbor.bottomLeft);

            key = (pos.x + 1) << 16 | (pos.y - 1);
            Nodes.TryGetValue(key, out node.neighbor.bottomRight);
        }
    }

    public bool CheckWalkable(PathNode node)
    {
        if (node == null)
            return false;
        FP sd = map.Get(node.pos);
        return sd >= Radius;
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
