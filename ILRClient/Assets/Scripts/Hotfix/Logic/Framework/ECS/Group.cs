namespace ECS.Core
{
    public class Group<T> where T : class, IComponent, new()
    {
        private readonly IComponentCollectorT<T> collector;
        private int index = 0;
        private ComponentStatus status;
        public int Count => collector.Count;

        public Group(IComponentCollectorT<T> collector, ComponentStatus status)
        {
            this.collector = collector;
            this.status = status;
        }

        public void Reset()
        {
            index = 0;
        }

        public Entity Next()
        {
            return collector.Find(ref index, status);
        }

        public bool TryGet(out Entity entity, out T component)
        {
            entity = collector.Find(ref index, status, out component);
            return entity != null;
        }
    }

}
