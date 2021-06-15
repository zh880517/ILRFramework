namespace ECS.Core
{

    public enum ComponentStatus
    {
        Normal = 0,
        Modify = 1,
        Add = 2,
    }

    public interface IComponentCollector
    {
        int Count { get; }
        IComponent Add(Entity entity, bool forceModify);
        IComponent Get(Entity entity);
        void Remove(Entity entity);
        void RemoveAll();
        IComponent Modify(Entity entity);
        void OnTickEnd();
    }

    public interface IComponentCollectorT<T> : IComponentCollector where T : class, IComponent, new()
    {
        Entity Find(ref int startIndex, ComponentStatus status, System.Func<T, bool> condition = null);
        Entity Find(ref int startIndex, ComponentStatus status, out T component, System.Func<T,bool> condition = null);
    }


    public class ComponentEntity<T> where T : class, IComponent, new()
    {
        public T Component = new T();
        public Entity Owner;
        public ComponentStatus Status;

        public void Modify()
        {
            if (Status == ComponentStatus.Normal)
                Status = ComponentStatus.Modify;
        }

        public void OnTickEnd()
        {
            Status = ComponentStatus.Normal;
        }

        public void Reset()
        {
            Owner = null;
            Status = ComponentStatus.Normal;
            if (Component is IReset resetComp)
                resetComp.Reset();
        }
    }
}
