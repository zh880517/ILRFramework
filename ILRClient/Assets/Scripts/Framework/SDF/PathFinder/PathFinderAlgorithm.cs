public abstract class PathFinderAlgorithm
{
    protected PathNode endNode;
    protected PathNode startNode;

    protected MapPathFinderData mapPath;


    public bool FindPath(PathNode start, PathNode end, MapPathFinderData mapPath)
    {
        mapPath.Clear();
        startNode = start;
        endNode = end;
        this.mapPath = mapPath;
        if (Search(startNode))
        {
            var node = endNode;
            while (node.parent != null)
            {
                mapPath.Path.Add(node);
                node = node.parent;
            }
            return true;
        }
        return false;
    }

    protected abstract bool Search(PathNode node);

}
