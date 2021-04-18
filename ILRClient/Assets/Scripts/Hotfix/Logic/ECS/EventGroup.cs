using System.Collections.Generic;
namespace ECS.Core
{
    public class EventGroup<TEntity> : IEventGroup<TEntity> where TEntity : Entity
    {
        //ILRuntime,直接使用foreach遍历会产生GC，这里不遍历Dictionary
        private readonly Dictionary<int, int> idIndexMap = new Dictionary<int, int>();
        private readonly List<int> modifyEnitis = new List<int>();
        private readonly Queue<int> unUsedIdxs = new Queue<int>();
        private readonly TContext<TEntity> context;
        private readonly ComponentEvent mask;
        private int itIdx = 0;

        public EventGroup(TContext<TEntity> context, ComponentEvent eventMask)
        {
            this.context = context;
            mask = eventMask;
        }

        private int GetIndex()
        {
            if (unUsedIdxs.Count > 0)
            {
                return unUsedIdxs.Dequeue();
            }
            modifyEnitis.Add(0);
            return modifyEnitis.Count - 1;
        }

        private void AddEnity(int entityId)
        {
            if (!idIndexMap.ContainsKey(entityId))
            {
                int idx = GetIndex();
                modifyEnitis[idx] = entityId;
                idIndexMap.Add(entityId, idx);
            }
        }

        public void OnAdd<T>(int entityId) where T : class, IComponent
        {
            if ((mask & ComponentEvent.OnAdd) != 0)
            {
                AddEnity(entityId);
            }
        }

        public void OnModify<T>(int entityId) where T : class, IComponent
        {
            if ((mask & ComponentEvent.OnModify) != 0)
            {
                AddEnity(entityId);
            }
        }

        public void OnRemove<T>(int entityId) where T : class, IComponent
        {
            if (idIndexMap.TryGetValue(entityId, out int idx))
            {
                modifyEnitis[idx] = 0;
                unUsedIdxs.Enqueue(idx);
                idIndexMap.Remove(entityId);
            }
        }

        public void Reset()
        {
            idIndexMap.Clear();
            modifyEnitis.Clear();
            unUsedIdxs.Clear();
        }

        public TEntity Next()
        {
            for (int i = itIdx; i<modifyEnitis.Count; ++i)
            {
                ++itIdx;
                int id = modifyEnitis[i];
                if (id != 0)
                {
                    return context.FindEntity(id);
                }
            }
            return null;
        }

        public bool IsEmpty()
        {
            return idIndexMap.Count == 0;
        }
    }
}
