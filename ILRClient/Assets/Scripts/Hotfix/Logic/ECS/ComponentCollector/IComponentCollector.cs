namespace ECS.Core
{
    public interface IComponentCollector
    {
        int Count { get; }
        IComponent Add(Entity entity);
        IComponent Get(Entity entity);
        void Remove(Entity entity);
        void Modify(Entity entity);
        void RegistEventGroup(IEventGroup eventGroup);
        Entity GetValid(int startIndex);
        Entity GetValid(int startIndex, out IComponent component);
    }

    public interface IComponentCollectorT<T> : IComponentCollector where T : class, IComponent, new()
    {
        Entity GetValid(int startIndex, out T component);
    }
}
