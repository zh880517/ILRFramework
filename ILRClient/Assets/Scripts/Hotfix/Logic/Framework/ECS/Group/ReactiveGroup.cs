namespace ECS.Core
{
    public struct ReactiveGroup<TEntity, TComponent> where TEntity : Entity where TComponent : class, IComponent, new()
    {
        private int Index;
        private int GroupIndex;
        private uint Version;
        private TContext<TEntity> Context;
        public ReactiveGroup(int groupIndex, uint version, TContext<TEntity> context)
        {
            Version = version;
            Context = context;
            Index = 0;
            GroupIndex = groupIndex;
        }

        public bool TryGet(out TEntity entity, out TComponent component)
        {
            var result = Context.Find<TComponent>(Index, Version, null, GroupIndex);
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