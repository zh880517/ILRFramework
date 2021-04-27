using System.Collections.Generic;

public class MapPathData
{
    public FTBinaryHeap<PathNode> OpenHeap { get; private set; } = new FTBinaryHeap<PathNode>();
    public List<PathNode> CloseList { get; private set; } = new List<PathNode>();
    public List<PathNode> Path { get; private set; } = new List<PathNode>();

    public Dictionary<int, PathNode> Nodes { get; private set; } = new Dictionary<int, PathNode>();

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
