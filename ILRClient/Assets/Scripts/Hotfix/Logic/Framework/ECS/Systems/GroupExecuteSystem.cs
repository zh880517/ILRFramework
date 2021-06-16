namespace ECS.Core
{
    public abstract class GroupExecuteSystem<TEntity, TComponent> : IExecuteSystem where TComponent : class, IComponent, new() where TEntity : Entity
    {
        private readonly int groupId;
        protected TContext<TEntity> context;
        public GroupExecuteSystem(TContext<TEntity> context)
        {
            groupId = context.RegisterReactiveGroup<TComponent>();
            this.context = context;
        }

        public void OnExecute()
        {
            var group = context.GetReactiveGroup<TComponent>(groupId);
            while(group.TryGet(out var entity, out TComponent component))
            {
                OnExecuteEntity(entity, component);
            }
        }

        protected abstract void OnExecuteEntity(TEntity entity, TComponent component);
    }

}