namespace ECS.Core
{
    public abstract class ReactiveSystem<TEntity, TComponent> : IExecuteSystem  where TEntity : Entity where TComponent : IComponent
    {
        private readonly IEventGroup<TEntity> eventGroup;

        public ReactiveSystem(TContext<TEntity> context, ComponentEvent mask)
        {
            eventGroup = context.CreatEventGroup<TComponent>(mask);
        }

        public void OnExecute()
        {
            if (eventGroup != null && !eventGroup.IsEmpty())
            {
                var entity = eventGroup.Next();
                while(entity != null)
                {
                    OnExecuteEntity(entity);
                    entity = eventGroup.Next();
                }
                eventGroup.Reset();
            }
        }

        protected abstract void OnExecuteEntity(Entity entity);
    }
}
