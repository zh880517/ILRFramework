using System;

namespace ECS.Core
{
    public class TContext<TEntity> : Context where TEntity : Entity
    {
        protected Func<Context, int, TEntity> creatFacory;

        public TEntity Unique { get; private set; }
        public TContext(int componentTypeCount, Func<Context, int, TEntity> creatFunc) : base(componentTypeCount)
        {
            creatFacory = creatFunc;
            Unique = CreatEntity();
        }

        public TEntity CreatEntity()
        {
            int newId = GenId();
            var entity = creatFacory(this, newId);
            entitis.Add(newId, entity);
            return entity;
        }

        public TEntity FindEntity(int id)
        {
            return Find(id) as TEntity;
        }

        public TEntity FindComponent<T>(ref int startIndex, out T component, Func<T, bool> match = null) where T : class, IComponent, new()
        {
            var collector = collectors[ComponentIdentity<T>.Id] as IComponentCollectorT<T>;
            return collector.Find(ref startIndex, out component, match) as TEntity;
        }

        public Group<T> CreatGroup<T>() where T : class, IComponent, new()
        {
            return new Group<T>(collectors[ComponentIdentity<T>.Id] as IComponentCollectorT<T>);
        }

        public EventGroup<TEntity> CreatEventGroup<TComponent>(ComponentEvent eventMask) where TComponent : IComponent
        {
            var group = new EventGroup<TEntity>(this, eventMask);
            collectors[ComponentIdentity<TComponent>.Id].RegistEventGroup(group);
            return group;
        }

    }

}
