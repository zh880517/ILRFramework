using System;

namespace ECS.Core
{
    public struct Group<TEntity, TComponent>  where TComponent : class, IComponent, new() where TEntity : Entity
    {
        private TContext<TEntity> Context;
        private int Index;
        private Func<TComponent, bool> Condition;
        public Group(TContext<TEntity> context, Func<TComponent, bool> condition)
        {
            Index = 0;
            Context = context;
            Condition = condition;
        }

        public bool TryGet(out TEntity entity, out TComponent component)
        {
            var result = Context.Find(Index, 0, Condition);
            component = result.Component;
            if (result.Id == 0)
            {
                entity = null;
                return false;
            }
            entity = Context.FindEntity(result.Id);
            Index = result.Index;
            return true;
        }
    }

}
