public class AStarPathFinder : PathFinderAlgorithm
{
    public AStarPathFinder(MapPathFinderData mapPath) : base(mapPath)
    {
    }

    protected override bool Search(PathNode node)
    {
        while (node != null)
        {
            mapPath.CloseList.Add(node);
            node.status = NodeStatus.Close;

            if (node == endNode)
                return true;
            SearchOpenNode(node.neighbor.top, node, DirectionType.Top);
            SearchOpenNode(node.neighbor.right, node, DirectionType.Right);
            SearchOpenNode(node.neighbor.bottom, node, DirectionType.Bottom);
            SearchOpenNode(node.neighbor.left, node, DirectionType.Left);
            SearchOpenNode(node.neighbor.topRight, node, DirectionType.TopRight);
            SearchOpenNode(node.neighbor.bottomRight, node, DirectionType.BottomRight);
            SearchOpenNode(node.neighbor.topLeft, node, DirectionType.TopLeft);
            SearchOpenNode(node.neighbor.bottomLeft, node, DirectionType.BottomLeft);
            node = mapPath.TryGetOpenNode();
        }
        return false;
    }

    void SearchOpenNode(PathNode toCheck, PathNode fromNode, DirectionType dir = DirectionType.None)
    {
        if (!mapPath.CheckWalkable(toCheck) || toCheck.status == NodeStatus.Close)
            return;
        if (dir == DirectionType.TopRight && (!mapPath.CheckWalkable(fromNode.neighbor.top) || !mapPath.CheckWalkable(fromNode.neighbor.right)))
            return;
        if (dir == DirectionType.TopLeft && (!mapPath.CheckWalkable(fromNode.neighbor.top) || !mapPath.CheckWalkable(fromNode.neighbor.left)))
            return;
        if (dir == DirectionType.BottomRight && (!mapPath.CheckWalkable(fromNode.neighbor.bottom) || !mapPath.CheckWalkable(fromNode.neighbor.right)))
            return;
        if (dir == DirectionType.BottomLeft && (!mapPath.CheckWalkable(fromNode.neighbor.bottom) || !mapPath.CheckWalkable(fromNode.neighbor.left)))
            return;
        if (toCheck.status == NodeStatus.Open)
        {
            var cost = ComputeG(dir);
            var gTemp = fromNode.g + cost;
            if (gTemp < toCheck.g)
            {
                toCheck.parent = fromNode;
                toCheck.g = gTemp;
                toCheck.f = gTemp + toCheck.h;
            }

            mapPath.OpenHeap.TryUpAdjust(toCheck);
            return;
        }
        toCheck.parent = fromNode;
        toCheck.g = fromNode.g + ComputeG(dir);
        toCheck.h = PathNode.ComputeH(toCheck, endNode);
        toCheck.f = toCheck.g + toCheck.h;
        toCheck.status = NodeStatus.Open;
        mapPath.OpenHeap.Enqueue(toCheck);

    }

    public static int ComputeG(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.Bottom:
            case DirectionType.Top:
            case DirectionType.Left:
            case DirectionType.Right:
                return PathNode.Line;
            default:
                return PathNode.Tilted;
        }
    }

}
