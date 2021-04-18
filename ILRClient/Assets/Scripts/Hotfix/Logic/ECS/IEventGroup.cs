namespace ECS.Core
{
    public enum ComponentEvent
    {
        OnAdd = 1,
        OnModify = 2,
        OnAddOrModify = 3,
    }

    public interface IEventGroup
    {
        void OnAdd<T>(int entityId) where T : class, IComponent;
        void OnModify<T>(int entityId) where T : class, IComponent;
        void OnRemove<T>(int entityId) where T : class, IComponent;
        void Reset();
        bool IsEmpty();
    }

    public interface IEventGroup<TEntity> : IEventGroup where TEntity : Entity
    {
        TEntity Next();
    }

}
