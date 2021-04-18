namespace ECS.Core
{
    public class Group<T> where T : class, IComponent, new()
    {
        private readonly ComponentCollector<T> collector;
        private int index;
        public int Count => collector.Count;

        public Group(ComponentCollector<T> collector)
        {
            this.collector = collector;
        }

        public void Reset()
        {
            index = 0;
        }

        public Entity Next()
        {
            return collector.GetValid(index++);
        }

        public Entity NextWithComponent(out T component)
        {
            var entity = collector.GetValid(index++, out IComponent comp);
            component = comp as T;
            return entity;
        }
    }

}
