using System;

namespace ECS.Core
{
    public struct ECPair<TEntity, TComponent>
    {
        public TEntity Owner;
        public TComponent Value;
    }

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
            return collector.Find(ref startIndex, ComponentStatus.Normal, out component, match) as TEntity;
        }

        public Group<T> CreatGroup<T>(ComponentStatus status = ComponentStatus.Normal) where T : class, IComponent, new()
        {
            return new Group<T>(collectors[ComponentIdentity<T>.Id] as IComponentCollectorT<T>, status);
        }

        public ECPair<TEntity, T> GetUniquePair<T>() where T : class, IComponent, IUnique, new()
        {
            if (!ComponentIdentity<T>.Unique)
            {
                throw new Exception($"{ComponentIdentity<T>.Name} not a UniqueComponent");
            }
            int id = ComponentIdentity<T>.Id;
            var collector = collectors[id] as UniqueComponentCollector<T>;
            return collector.GetPair<TEntity>();
        }

        public TComponent GetUnique<TComponent>() where TComponent : class, IComponent, IUnique, new()
        {
            if (!ComponentIdentity<TComponent>.Unique)
            {
                throw new Exception($"{ComponentIdentity<TComponent>.Name} not a UniqueComponent");
            }
            int id = ComponentIdentity<TComponent>.Id;
            var collector = collectors[id] as UniqueComponentCollector<TComponent>;
            return collector.Get();
        }

    }

}
