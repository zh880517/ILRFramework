public class AStarPathFinder : PathFinder
{
    protected override bool Search(PathNode node)
    {
        while (node != null)
        {
            MapPath.CloseList.Add(node);
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
            node = MapPath.TryGetOpenNode();
        }
        return false;
    }

    void SearchOpenNode(PathNode toCheck, PathNode fromNode, DirectionType dir = DirectionType.None)
    {
        if (toCheck == null || !toCheck.Walkable() || toCheck.status == NodeStatus.Close)
            return;
        if (dir == DirectionType.TopRight && (!fromNode.neighbor.top.Walkable() || !fromNode.neighbor.right.Walkable()))
            return;
        if (dir == DirectionType.TopLeft && (!fromNode.neighbor.top.Walkable() || !fromNode.neighbor.left.Walkable()))
            return;
        if (dir == DirectionType.BottomRight && (!fromNode.neighbor.bottom.Walkable() || !fromNode.neighbor.right.Walkable()))
            return;
        if (dir == DirectionType.BottomLeft && (!fromNode.neighbor.bottom.Walkable() || !fromNode.neighbor.left.Walkable()))
            return;
        if (toCheck.status == NodeStatus.Open)
        {
            var cost = PathNode.ComputeGForAStar(dir);
            var gTemp = fromNode.g + cost;
            if (gTemp < toCheck.g)
            {
                toCheck.parent = fromNode;
                toCheck.g = gTemp;
                toCheck.f = gTemp + toCheck.h;
            }

            MapPath.OpenHeap.TryUpAdjust(toCheck);
            return;
        }
        toCheck.parent = fromNode;
        toCheck.g = fromNode.g + PathNode.ComputeGForAStar(dir);
        toCheck.h = PathNode.ComputeH(toCheck, endNode);
        toCheck.f = toCheck.g + toCheck.h;
        toCheck.status = NodeStatus.Open;
        MapPath.OpenHeap.Enqueue(toCheck);

    }
}
