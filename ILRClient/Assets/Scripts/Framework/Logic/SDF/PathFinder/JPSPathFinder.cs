public class JPSPathFinder : PathFinderAlgorithm
{
    enum JumpType
    {
        Line,
        Tilted
    }

    public JPSPathFinder(MapPathFinderData mapPath) : base(mapPath)
    {
    }


    protected override bool Search(PathNode node)
    {
        node.dir = DirectionType.All;
        while (node != null)
        {
            node.status = NodeStatus.Close;
            mapPath.CloseList.Add(node);

            if (node == endNode)
                return true;
            if ((node.dir & DirectionType.Top) != DirectionType.None)
                SearchTop(node);
            if ((node.dir & DirectionType.Right) != DirectionType.None)
                SearchRight(node);
            if ((node.dir & DirectionType.Left) != DirectionType.None)
                SearchLeft(node);
            if ((node.dir & DirectionType.Bottom) != DirectionType.None)
                SearchBottom(node);
            if ((node.dir & DirectionType.TopRight) != DirectionType.None)
                SearchTopRight(node);
            if ((node.dir & DirectionType.TopLeft) != DirectionType.None)
                SearchTopLeft(node);
            if ((node.dir & DirectionType.BottomRight) != DirectionType.None)
                SearchBottomRight(node);
            if ((node.dir & DirectionType.BottomLeft) != DirectionType.None)
                SearchBottomLeft(node);
            node = mapPath.TryGetOpenNode();
        }
        return false;
    }
    
    private void SearchTop(PathNode fromNode)
    {
        var node = fromNode;
        while (mapPath.CheckWalkable(node))
            node = GetNext(node.neighbor.top, fromNode, DirectionType.Top, JumpType.Line);
    }

    private void SearchRight(PathNode fromNode)
    {
        var node = fromNode;
        while (mapPath.CheckWalkable(node))
            node = GetNext(node.neighbor.right, fromNode, DirectionType.Right, JumpType.Line);
    }

    private void SearchLeft(PathNode fromNode)
    {
        var node = fromNode;
        while (mapPath.CheckWalkable(node))
            node = GetNext(node.neighbor.left, fromNode, DirectionType.Left, JumpType.Line);
    }

    private void SearchBottom(PathNode fromNode)
    {
        var node = fromNode;
        while (mapPath.CheckWalkable(node))
            node = GetNext(node.neighbor.bottom, fromNode, DirectionType.Bottom, JumpType.Line);
    }

    private void SearchTopRight(PathNode fromNode)
    {
        var node = fromNode;
        while (mapPath.CheckWalkable(node))
            node = GetNext(node.neighbor.topRight, fromNode, DirectionType.TopRight, JumpType.Tilted);
    }

    private void SearchTopLeft(PathNode fromNode)
    {
        var node = fromNode;
        while (mapPath.CheckWalkable(node))
            node = GetNext(node.neighbor.topLeft, fromNode, DirectionType.TopLeft, JumpType.Tilted);
    }

    private void SearchBottomRight(PathNode fromNode)
    {
        var node = fromNode;
        while (mapPath.CheckWalkable(node))
            node = GetNext(node.neighbor.bottomRight, fromNode, DirectionType.BottomRight, JumpType.Tilted);
    }

    private void SearchBottomLeft(PathNode fromNode)
    {
        var node = fromNode;
        while (mapPath.CheckWalkable(node))
            node = GetNext(node.neighbor.bottomLeft, fromNode, DirectionType.BottomLeft, JumpType.Tilted);
    }

    private PathNode GetNext(PathNode toCheck, PathNode fromNode, DirectionType dir, JumpType jumpType)
    {
        if (!mapPath.CheckWalkable(toCheck)|| toCheck.status == NodeStatus.Close)
        {
            return null;
        }

        DirectionType jumpNodeDir;
        var tempValue = jumpType == JumpType.Line ? IsLineJumpNode(toCheck, dir, out jumpNodeDir) : IsTitleJumpNode(toCheck, dir, out jumpNodeDir);
        if (tempValue)  // toCheck是跳点
        {
            if (toCheck.status == NodeStatus.Open) // 已经在openlist里了 判断是否更新G值 更新parent
            {
                var cost = ComputeG(dir, fromNode, toCheck);
                var gTemp = fromNode.g + cost;
                if (gTemp < toCheck.g)
                {
                    var oldDir = GetDirection(toCheck.parent, toCheck);
                    toCheck.dir = (toCheck.dir ^ oldDir) | jumpNodeDir; // 去掉旧的父亲方向 保留自身方向 添加新的方向 
                    toCheck.parent = fromNode;
                    toCheck.g = gTemp;
                    toCheck.f = gTemp + toCheck.h;
                }

                mapPath.OpenHeap.TryUpAdjust(toCheck);
                return null;
            }
            //加入openlist
            toCheck.parent = fromNode;
            toCheck.g = fromNode.g + ComputeG(dir, toCheck, fromNode);
            toCheck.h = PathNode.ComputeH(toCheck, endNode);
            toCheck.f = toCheck.g + toCheck.h;
            toCheck.status = NodeStatus.Open;
            toCheck.dir = jumpNodeDir;
            mapPath.OpenHeap.Enqueue(toCheck);
            return null;
        }
        return toCheck;
    }

    private bool IsLineJumpNode(PathNode toCheck, DirectionType dir, out DirectionType jumpNodeDir)
    {
        jumpNodeDir = dir;
        if (!mapPath.CheckWalkable(toCheck))
            return false;
        if (dir == DirectionType.Right)
            return IsRightJumpNode(toCheck, ref jumpNodeDir);
        else if (dir == DirectionType.Left)
            return IsLeftJumpNode(toCheck, ref jumpNodeDir);
        else if (dir == DirectionType.Top)
            return IsTopJumpNode(toCheck, ref jumpNodeDir);
        else if (dir == DirectionType.Bottom)
            return IsBottomJumpNode(toCheck, ref jumpNodeDir);
        return false;
    }

    private bool IsRightJumpNode(PathNode toCheck, ref DirectionType jumpNodeDir)
    {
        if (toCheck == endNode)
            return true;
        var result = false;
        if (mapPath.CheckWalkable(toCheck.neighbor.right))
        {
            var up = toCheck.neighbor.top;
            var down = toCheck.neighbor.bottom;
            var topRight = toCheck.neighbor.topRight;
            var bottomRight = toCheck.neighbor.bottomRight;
            if (!mapPath.CheckWalkable(up) && mapPath.CheckWalkable(topRight))
            {
                jumpNodeDir |= DirectionType.TopRight;
                result = true;
            }
            if (!mapPath.CheckWalkable(down) && mapPath.CheckWalkable(bottomRight))
            {
                jumpNodeDir |= DirectionType.BottomRight;
                result = true;
            }
        }
        return result;
    }

    private bool IsLeftJumpNode(PathNode toCheck, ref DirectionType jumpNodeDir)
    {
        if (toCheck == endNode)
            return true;
        var result = false;
        if (mapPath.CheckWalkable(toCheck.neighbor.left))
        {
            var top = toCheck.neighbor.top;
            var bottom = toCheck.neighbor.bottom;
            var topLeft = toCheck.neighbor.topLeft;
            var bottomLeft = toCheck.neighbor.bottomLeft;
            if (!mapPath.CheckWalkable(top) && mapPath.CheckWalkable(topLeft))
            {
                jumpNodeDir |= DirectionType.TopLeft;
                result = true;
            }
            if (!mapPath.CheckWalkable(bottom) && mapPath.CheckWalkable(bottomLeft))
            {
                jumpNodeDir |= DirectionType.BottomLeft;
                result = true;
            }
        }
        return result;
    }

    private bool IsTopJumpNode(PathNode toCheck, ref DirectionType jumpNodeDir)
    {
        if (toCheck == endNode)
            return true;
        var result = false;
        if (mapPath.CheckWalkable(toCheck.neighbor.top))
        {
            var left = toCheck.neighbor.left;
            var right = toCheck.neighbor.right;
            var topLeft = toCheck.neighbor.topLeft;
            var topRight = toCheck.neighbor.topRight;
            if (!mapPath.CheckWalkable(left) && mapPath.CheckWalkable(topLeft))
            {
                jumpNodeDir |= DirectionType.TopLeft;
                result = true;
            }
            if (!mapPath.CheckWalkable(right) && mapPath.CheckWalkable(topRight))
            {
                jumpNodeDir |= DirectionType.TopRight;
                result = true;
            }
        }
        return result;
    }

    private bool IsBottomJumpNode(PathNode toCheck, ref DirectionType jumpNodeDir)
    {
        if (toCheck == endNode)
            return true;
        var result = false;
        if (mapPath.CheckWalkable(toCheck.neighbor.bottom))
        {
            var left = toCheck.neighbor.left;
            var right = toCheck.neighbor.right;
            var bottomLeft = toCheck.neighbor.bottomLeft;
            var bottomRight = toCheck.neighbor.bottomRight;
            if (!mapPath.CheckWalkable(left) && mapPath.CheckWalkable(bottomLeft))
            {
                jumpNodeDir |= DirectionType.BottomLeft;
                result = true;
            }
            if (!mapPath.CheckWalkable(right) && mapPath.CheckWalkable(bottomRight))
            {
                jumpNodeDir |= DirectionType.BottomRight;
                result = true;
            }
        }
        return result;
    }

    private bool IsTitleJumpNode(PathNode toCheck, DirectionType dir, out DirectionType jumpNodeDir)  //是否是斜方向的跳点
    {
        jumpNodeDir = dir;
        if (toCheck == endNode || !mapPath.CheckWalkable(toCheck))
            return true;
        if (dir == DirectionType.TopRight)
            return IsTopRightJumpNode(toCheck, ref jumpNodeDir);
        else if (dir == DirectionType.TopLeft)
            return IsTopLeftJumpNode(toCheck, ref jumpNodeDir);
        else if (dir == DirectionType.BottomRight)
            return IsBottomRightJumpNode(toCheck, ref jumpNodeDir);
        else if (dir == DirectionType.BottomLeft)
            return IsBottomLeftJumpNode(toCheck, ref jumpNodeDir);
        return false;
    }

    private bool IsTopRightJumpNode(PathNode toCheck, ref DirectionType jumpNodeDir)
    {
        var result = false;
        result |= IsTopJumpNode(toCheck, ref jumpNodeDir);
        result |= IsRightJumpNode(toCheck, ref jumpNodeDir);  // 先检查自身是否符合line跳点, 是的话追加方向到jumpNodeDir
        if (!result) // 自身不符合line跳点 检查line方向有无跳点
        {
            var temp = DirectionType.None;
            var node = toCheck.neighbor.top;
            while (mapPath.CheckWalkable(node))
            {
                if (IsTopJumpNode(node, ref temp))
                {
                    result = true;
                    break;
                }
                node = node.neighbor.top;
            }
            node = toCheck.neighbor.right;
            while (mapPath.CheckWalkable(node))
            {
                if (IsRightJumpNode(node, ref temp))
                {
                    result = true;
                    break;
                }
                node = node.neighbor.right;
            }
        }
        if (result)
        {
            jumpNodeDir |= DirectionType.Top;
            jumpNodeDir |= DirectionType.Right;
        }
        return result;
    }

    private bool IsTopLeftJumpNode(PathNode toCheck, ref DirectionType jumpNodeDir)
    {
        var result = false;
        result |= IsTopJumpNode(toCheck, ref jumpNodeDir);
        result |= IsLeftJumpNode(toCheck, ref jumpNodeDir);
        if (!result)
        {
            var temp = DirectionType.None;
            var node = toCheck.neighbor.top;
            while (mapPath.CheckWalkable(node))
            {
                if (IsTopJumpNode(node, ref temp))
                {
                    result = true;
                    break;
                }
                node = node.neighbor.top;
            }
            node = toCheck.neighbor.left;
            while (mapPath.CheckWalkable(node))
            {
                if (IsLeftJumpNode(node, ref temp))
                {
                    result = true;
                    break;
                }
                node = node.neighbor.left;
            }
        }
        if (result)
        {
            jumpNodeDir |= DirectionType.Top;
            jumpNodeDir |= DirectionType.Left;
        }
        return result;
    }

    private bool IsBottomRightJumpNode(PathNode toCheck, ref DirectionType jumpNodeDir)
    {
        var result = false;
        result |= IsBottomJumpNode(toCheck, ref jumpNodeDir);
        result |= IsRightJumpNode(toCheck, ref jumpNodeDir);
        if (!result)
        {
            var temp = DirectionType.None;
            var node = toCheck.neighbor.bottom;
            while (node != null)
            {
                if (IsBottomJumpNode(node, ref temp))
                {
                    result = true;
                    break;
                }
                node = node.neighbor.bottom;
            }
            node = toCheck.neighbor.right;
            while (node != null)
            {
                if (IsRightJumpNode(node, ref temp))
                {
                    result = true;
                    break;
                }
                node = node.neighbor.right;
            }
        }
        if (result)
        {
            jumpNodeDir |= DirectionType.Bottom;
            jumpNodeDir |= DirectionType.Right;
        }
        return result;
    }

    private bool IsBottomLeftJumpNode(PathNode toCheck, ref DirectionType jumpNodeDir)
    {
        var result = false;
        result |= IsBottomJumpNode(toCheck, ref jumpNodeDir);
        result |= IsLeftJumpNode(toCheck, ref jumpNodeDir);
        if (!result)
        {
            var temp = DirectionType.None;
            var node = toCheck.neighbor.bottom;
            while (node != null)
            {
                if (IsBottomJumpNode(node, ref temp))
                {
                    result = true;
                    break;
                }
                node = node.neighbor.bottom;
            }
            node = toCheck.neighbor.left;
            while (node != null)
            {
                if (IsLeftJumpNode(node, ref temp))
                {
                    result = true;
                    break;
                }
                node = node.neighbor.left;
            }
        }
        if (result)
        {
            jumpNodeDir |= DirectionType.Bottom;
            jumpNodeDir |= DirectionType.Left;
        }
        return result;
    }

    private DirectionType GetDirection(PathNode ori, PathNode dest)
    {
        int xDelta = dest.pos.x - ori.pos.x, yDelta = dest.pos.y - ori.pos.y;
        if (xDelta > 0 && yDelta > 0)
            return DirectionType.TopRight;
        if (xDelta > 0 && yDelta == 0)
            return DirectionType.Right;
        if (xDelta > 0 && yDelta < 0)
            return DirectionType.BottomRight;
        if (xDelta < 0 && yDelta > 0)
            return DirectionType.TopLeft;
        if (xDelta < 0 && yDelta == 0)
            return DirectionType.Left;
        if (xDelta < 0 && yDelta < 0)
            return DirectionType.BottomLeft;
        if (xDelta == 0 && yDelta > 0)
            return DirectionType.Top;
        if (xDelta == 0 && yDelta < 0)
            return DirectionType.Bottom;
        return DirectionType.None;
    }

    public static int ComputeG(DirectionType direction, PathNode ori, PathNode dest)
    {
        int xDelta, yDelta;
        switch (direction)
        {
            case DirectionType.Bottom:
            case DirectionType.Top:
                yDelta = dest.pos.y > ori.pos.y ? dest.pos.y - ori.pos.y : ori.pos.y - dest.pos.y;
                return yDelta * PathNode.Line;
            case DirectionType.Left:
            case DirectionType.Right:
                xDelta = dest.pos.x > ori.pos.x ? dest.pos.x - ori.pos.x : ori.pos.x - dest.pos.x;
                return xDelta * PathNode.Line;
            default:
                xDelta = dest.pos.x > ori.pos.x ? dest.pos.x - ori.pos.x : ori.pos.x - dest.pos.x;
                return xDelta * PathNode.Tilted;
        }
    }
}
