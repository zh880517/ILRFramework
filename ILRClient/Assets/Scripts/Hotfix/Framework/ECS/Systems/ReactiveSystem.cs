using System.Collections.Generic;

namespace ECS.Core
{
    /// <summary>
    /// 只有指定的Component有变化的Entity才会被更新
    /// </summary>
    public abstract class ReactiveSystem<TEntity, TComponent> : IExecuteSystem  where TEntity : Entity where TComponent : IComponent
    {
        private readonly IEventGroup<TEntity> eventGroup;
        private readonly List<TEntity> entities = new List<TEntity>();

        public ReactiveSystem(TContext<TEntity> context, ComponentEvent mask)
        {
            eventGroup = context.CreatEventGroup<TComponent>(mask);
        }

        public void OnExecute()
        {
            if (eventGroup != null && !eventGroup.IsEmpty())
            {
                eventGroup.CopyToList(entities);
                OnExecuteEntitis(entities);
                entities.Clear();
            }
        }

        protected abstract void OnExecuteEntitis(List<TEntity> entities);
    }
}
