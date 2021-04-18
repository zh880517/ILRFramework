using System.Collections.Generic;
namespace ECS.Core
{
    public class Context
    {
        private int idIndex = 1;
        protected readonly Dictionary<int, Entity> entitis = new Dictionary<int, Entity>();
        protected readonly List<IComponentCollector> collectors;
        public int Generation { get; private set; }

        public Context(int componentTypeCount)
        {
            collectors = new List<IComponentCollector>(componentTypeCount);
        }

        public void InitComponentCollector<T>() where T : class, IComponent, new()
        {
            collectors[ComponentIdentity<T>.Id] = new ComponentCollector<T>();
        }

        protected Entity Find(int id)
        {
            if (entitis.TryGetValue(id, out var enity))
            {
                return enity;
            }
            return null;
        }

        protected int GenId()
        {
            int newId;
            do
            {
                newId = idIndex++;
                if (idIndex == 0)
                {
                    idIndex++;
                    ++Generation;
                }
            } while (entitis.ContainsKey(newId));
            return newId;
        }

        public void DestroyEntity(Entity entity)
        {
            if (!entity.Check(this))
                return;
            for (int i = 0; i < collectors.Count; ++i)
            {
                collectors[i].Remove(entity.Id);
            }
            entitis.Remove(idIndex);
        }

        public T AddComponent<T>(Entity entity) where T : class, IComponent, new()
        {
            if (!entity.Check(this) && entitis.ContainsKey(entity.Id))
                return default(T);
            return collectors[ComponentIdentity<T>.Id].Add(entity.Id) as T;
        }

        public void SetComponentModify<T>(Entity entity) where T : class, IComponent, new()
        {
            if (!entity.Check(this))
                return;
            collectors[ComponentIdentity<T>.Id].Modify(entity.Id);
        }

        public T GetComponent<T>(Entity entity) where T : class, IComponent, new()
        {
            if (!entity.Check(this))
                return default(T);
            return collectors[ComponentIdentity<T>.Id].Get(entity.Id) as T;
        }

        public void RemoveComponent<T>(Entity entity) where T : class, IComponent, new()
        {
            if (!entity.Check(this))
                return;
            collectors[ComponentIdentity<T>.Id].Remove(entity.Id);
        }

        public Group<T> CreatGroup<T>() where T : class, IComponent, new()
        {
            return new Group<T>(collectors[ComponentIdentity<T>.Id] as ComponentCollector<T>);
        }

    }
}