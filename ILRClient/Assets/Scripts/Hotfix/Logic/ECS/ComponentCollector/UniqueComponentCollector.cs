using System.Collections.Generic;

namespace ECS.Core
{
    public class UniqueComponentCollector<T> : IComponentCollectorT<T> where T : class, IComponent, new()
    {
        public int Count => uniqueEntity != null ? 1 : 0;
        private readonly List<IEventGroup> eventGroups = new List<IEventGroup>();

        private Entity uniqueEntity;
        private T uniqueComponent;
        public IComponent Add(Entity entity)
        {
            if (uniqueEntity == entity)
                return uniqueComponent;
            else
            {
                Remove(entity);
            }
            if (uniqueComponent == null)
            {
                uniqueComponent = new T();
            }
            uniqueEntity = entity;
            for (int i = 0; i < eventGroups.Count; ++i)
            {
                eventGroups[i].OnAdd<T>(entity.Id);
            }
            return uniqueComponent;
        }

        public IComponent Get(Entity entity)
        {
            if (entity == uniqueEntity)
                return uniqueComponent;
            return null;
        }

        public Entity GetValid(int startIndex, out T component)
        {
            if (startIndex == 0 && uniqueEntity != null)
            {
                component = uniqueComponent;
                return uniqueEntity;
            }
            component = null;
            return null;
        }

        public Entity GetValid(int startIndex)
        {
            if (startIndex == 0 && uniqueEntity != null)
            {
                return uniqueEntity;
            }
            return null;
        }

        public Entity GetValid(int startIndex, out IComponent component)
        {
            if (startIndex == 0 && uniqueEntity != null)
            {
                component = uniqueComponent;
                return uniqueEntity;
            }
            component = null;
            return null;
        }

        public void Modify(Entity entity)
        {
            if (uniqueEntity == entity)
            {
                for (int i = 0; i < eventGroups.Count; ++i)
                {
                    eventGroups[i].OnModify<T>(entity.Id);
                }
            }
        }

        public void RegistEventGroup(IEventGroup eventGroup)
        {
            eventGroups.Add(eventGroup);
        }

        public void Remove(Entity entity)
        {
            if (entity == uniqueEntity)
            {
                uniqueEntity = null;
                if (uniqueEntity is IReset resetComp)
                    resetComp.Reset();
                for (int i = 0; i < eventGroups.Count; ++i)
                {
                    eventGroups[i].OnRemove<T>(entity.Id);
                }
            }
        }
    }

}
