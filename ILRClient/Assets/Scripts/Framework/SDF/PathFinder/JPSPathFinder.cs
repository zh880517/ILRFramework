public class JPSPathFinder : PathFinder
{
    enum JumpType 
    { 
        Line, 
        Tilted 
    }

    protected override bool Search(PathNode node)
    {
        node.dir = DirectionType.All;
        while (node != null)
        {
            node.status = NodeStatus.Close;
            MapPath.CloseList.Add(node);

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
            node = MapPath.TryGetOpenNode();
        }
        return false;
    }
    
    private void SearchTop(PathNode fromNode)
    {
        var node = fromNode;
        while (node != null && node.Walkable())
            node = GetNext(node.neighbor.top, fromNode, DirectionType.Top, JumpType.Line);
    }

    private void SearchRight(PathNode fromNode)
    {
        var node = fromNode;
        while (node != null && node.Walkable())
            node = GetNext(node.neighbor.right, fromNode, DirectionType.Right, JumpType.Line);
    }

    private void SearchLeft(PathNode fromNode)
    {
        var node = fromNode;
        while (node != null && node.Walkable())
            node = GetNext(node.neighbor.left, fromNode, DirectionType.Left, JumpType.Line);
    }

    private void SearchBottom(PathNode fromNode)
    {
        var node = fromNode;
        while (node != null && node.Walkable())
            node = GetNext(node.neighbor.bottom, fromNode, DirectionType.Bottom, JumpType.Line);
    }

    private void SearchTopRight(PathNode fromNode)
    {
        var node = fromNode;
        while (node != null && node.Walkable())
            node = GetNext(node.neighbor.topRight, fromNode, DirectionType.TopRight, JumpType.Tilted);
    }

    private void SearchTopLeft(PathNode fromNode)
    {
        var node = fromNode;
        while (node != null && node.Walkable())
            node = GetNext(node.neighbor.topLeft, fromNode, DirectionType.TopLeft, JumpType.Tilted);
    }

    private void SearchBottomRight(PathNode fromNode)
    {
        var node = fromNode;
        while (node != null && node.Walkable())
            node = GetNext(node.neighbor.bottomRight, fromNode, DirectionType.BottomRight, JumpType.Tilted);
    }

    private void SearchBottomLeft(PathNode fromNode)
    {
        var node = fromNode;
        while (node != null && node.Walkable())
            node = GetNext(node.neighbor.bottomLeft, fromNode, DirectionType.BottomLeft, JumpType.Tilted);
    }

    private PathNode GetNext(PathNode toCheck, PathNode fromNode, DirectionType dir, JumpType jumpType)
    {
        if (toCheck == null || toCheck.Walkable() == false || toCheck.status == NodeStatus.Close)
        {
            return null;
        }

        DirectionType jumpNodeDir;
        var tempValue = jumpType == JumpType.Line ? IsLineJumpNode(toCheck, dir, out jumpNodeDir) : IsTitleJumpNode(toCheck, dir, out jumpNodeDir);
        if (tempValue)  // toCheck是跳点
        {
            if (toCheck.status == NodeStatus.Open) // 已经在openlist里了 判断是否更新G值 更新parent
            {
                var cost = PathNode.ComputeGForJPS(dir, fromNode, toCheck);
                var gTemp = fromNode.g + cost;
                if (gTemp < toCheck.g)
                {
                    var oldDir = GetDirection(toCheck.parent, toCheck);
                    toCheck.dir = (toCheck.dir ^ oldDir) | jumpNodeDir; // 去掉旧的父亲方向 保留自身方向 添加新的方向 
                    toCheck.parent = fromNode;
                    toCheck.g = gTemp;
                    toCheck.f = gTemp + toCheck.h;
                }

                MapPath.OpenHeap.TryUpAdjust(toCheck);
                return null;
            }
            //加入openlist
            toCheck.parent = fromNode;
            toCheck.g = fromNode.g + PathNode.ComputeGForJPS(dir, toCheck, fromNode);
            toCheck.h = PathNode.ComputeH(toCheck, endNode);
            toCheck.f = toCheck.g + toCheck.h;
            toCheck.status = NodeStatus.Open;
            toCheck.dir = jumpNodeDir;
            MapPath.OpenHeap.Enqueue(toCheck);
            return null;
        }
        return toCheck;
    }

    private bool IsLineJumpNode(PathNode toCheck, DirectionType dir, out DirectionType jumpNodeDir)
    {
        jumpNodeDir = dir;
        if (toCheck == null || toCheck.Walkable() == false)
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
        if (toCheck.neighbor.right != null && toCheck.neighbor.right.Walkable())
        {
            var up = toCheck.neighbor.top;
            var down = toCheck.neighbor.bottom;
            var topRight = toCheck.neighbor.topRight;
            var bottomRight = toCheck.neighbor.bottomRight;
            if (up != null && !up.Walkable() && topRight != null && topRight.Walkable())
            {
                jumpNodeDir |= DirectionType.TopRight;
                result = true;
            }
            if (down != null && !down.Walkable() && bottomRight != null && bottomRight.Walkable())
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
        if (toCheck.neighbor.left != null && toCheck.neighbor.left.Walkable())
        {
            var top = toCheck.neighbor.top;
            var bottom = toCheck.neighbor.bottom;
            var topLeft = toCheck.neighbor.topLeft;
            var bottomLeft = toCheck.neighbor.bottomLeft;
            if (top != null && !top.Walkable() && topLeft != null && topLeft.Walkable())
            {
                jumpNodeDir |= DirectionType.TopLeft;
                result = true;
            }
            if (bottom != null && !bottom.Walkable() && bottomLeft != null && bottomLeft.Walkable())
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
        if (toCheck.neighbor.top != null && toCheck.neighbor.top.Walkable())
        {
            var left = toCheck.neighbor.left;
            var right = toCheck.neighbor.right;
            var topLeft = toCheck.neighbor.topLeft;
            var topRight = toCheck.neighbor.topRight;
            if (left != null && !left.Walkable() && topLeft != null && topLeft.Walkable())
            {
                jumpNodeDir |= DirectionType.TopLeft;
                result = true;
            }
            if (right != null && !right.Walkable() && topRight != null && topRight.Walkable())
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
        if (toCheck.neighbor.bottom != null && toCheck.neighbor.bottom.Walkable())
        {
            var left = toCheck.neighbor.left;
            var right = toCheck.neighbor.right;
            var bottomLeft = toCheck.neighbor.bottomLeft;
            var bottomRight = toCheck.neighbor.bottomRight;
            if (left != null && !left.Walkable() && bottomLeft != null && bottomLeft.Walkable())
            {
                jumpNodeDir |= DirectionType.BottomLeft;
                result = true;
            }
            if (right != null && !right.Walkable() && bottomRight != null && bottomRight.Walkable())
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
        if (toCheck == endNode || toCheck == null || toCheck.Walkable() == false)
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
            while (node != null && node.Walkable())
            {
                if (IsTopJumpNode(node, ref temp))
                {
                    result = true;
                    break;
                }
                node = node.neighbor.top;
            }
            node = toCheck.neighbor.right;
            while (node != null && node.Walkable())
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
            while (node != null && node.Walkable())
            {
                if (IsTopJumpNode(node, ref temp))
                {
                    result = true;
                    break;
                }
                node = node.neighbor.top;
            }
            node = toCheck.neighbor.left;
            while (node != null && node.Walkable())
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
        int xDelta = dest.x - ori.x, yDelta = dest.y - ori.y;
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

}
