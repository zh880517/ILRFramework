using System;

namespace ECS.Core
{
    public class TContext<TEntity> : Context where TEntity : Entity
    {
        protected Func<Context, int, TEntity> creatFacory;
        public TContext(int componentTypeCount, Func<Context, int, TEntity> creatFunc) : base(componentTypeCount)
        {
            creatFacory = creatFunc;
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

        public EventGroup<TEntity> CreatEventGroup<TComponent>(ComponentEvent eventMask) where TComponent : IComponent
        {
            var group = new EventGroup<TEntity>(this, eventMask);
            collectors[ComponentIdentity<TComponent>.Id].RegistEventGroup(group);
            return group;
        }

    }

}
