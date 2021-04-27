public abstract class PathFinder
{
    protected PathNode endNode;
    protected PathNode startNode;

    protected MapPathData MapPath;



    private void FindPath()
    {
        MapPath.Clear();
        if (Search(startNode))
        {
            var node = endNode;
            while (node.parent != null)
            {
                MapPath.Path.Add(node);
                node = node.parent;
            }
        }
    }

    protected abstract bool Search(PathNode node);

}
