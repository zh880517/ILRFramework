using System;

namespace ECS.Core
{
    public struct Group<TEntity, TComponent>  where TComponent : class, IComponent, new() where TEntity : Entity
    {
        private ContextT<TEntity> Context;
        private int Index;
        private Func<TComponent, bool> Condition;
        public TEntity Entity { get; private set; }
        public TComponent Component { get; private set; }
        public Group(ContextT<TEntity> context, Func<TComponent, bool> condition)
        {
            Index = 0;
            Context = context;
            Condition = condition;
            Entity = null;
            Component = default;
        }

        public bool MoveNext()
        {
            var result = Context.Find(Index, 0, Condition);
            Component = result.Component;
            if (result.Id == 0)
            {
                Entity = null;
                return false;
            }
            Entity = Context.FindEntity(result.Id);
            Index = result.Index;
            return true;
        }
    }

}
