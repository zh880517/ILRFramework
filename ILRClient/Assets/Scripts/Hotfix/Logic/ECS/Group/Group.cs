namespace ECS.Core
{
    public class Group<T> where T : class, IComponent, new()
    {
        private readonly IComponentCollectorT<T> collector;
        private int index = 0;
        public int Count => collector.Count;

        public Group(IComponentCollectorT<T> collector)
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

        public bool TryGet(out Entity entity, out T component)
        {
            entity = collector.GetValid(index++, out component);
            return entity != null;
        }
    }

}
