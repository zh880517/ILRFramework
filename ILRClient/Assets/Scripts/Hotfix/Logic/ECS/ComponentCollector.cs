using System.Collections.Generic;
namespace ECS.Core
{

    public interface IComponentCollector
    {
        int Count { get;}
        IComponent Add(int entityId);
        IComponent Get(int entityId);
        void Remove(int entityId);
        void Modify(int entityId);
        void RegistEventGroup(IEventGroup eventGroup);
        Entity GetValid(int startIndex);
        Entity GetValid(int startIndex, out IComponent component);
        //void Tick();
    }

    public class ComponentCollector<T> : IComponentCollector where T : class, IComponent, new()
    {
        private class ComponentUnit
        {
            public T Component;
            public Entity Owner;
            public int Index;
        }
        private List<ComponentUnit> units = new List<ComponentUnit>();
        private Queue<int> unUsedIdxs = new Queue<int>();
        private Dictionary<int, int> idIdxMap = new Dictionary<int, int>();//EntityId => 数组索引
        private List<IEventGroup> eventGroups = new List<IEventGroup>();
        public int Count { get; private set; }

        public ComponentCollector()
        {
        }

        private ComponentUnit CreateUnit()
        {
            if (unUsedIdxs.Count > 0)
            {
                var index = unUsedIdxs.Dequeue();
                return units[index];
            }
            var unit = new ComponentUnit
            {
                Component = new T(),
                Index = units.Count,
            };
            units.Add(unit);
            return unit;
        }

        public IComponent Add(int entityId)
        {
            if (idIdxMap.TryGetValue(entityId, out int idx))
            {
                return units[idx].Component;
            }
            var unit = CreateUnit();
            idIdxMap.Add(entityId, idx);
            ++Count;
            for (int i = 0; i < eventGroups.Count; ++i)
            {
                eventGroups[i].OnAdd<T>(entityId);
            }
            return unit.Component;
        }

        public IComponent Get(int entityId)
        {
            if (idIdxMap.TryGetValue(entityId, out int idx))
            {
                return units[idx].Component;
            }
            return null;
        }

        public void Modify(int entityId)
        {
            if (idIdxMap.ContainsKey(entityId))
            {
                for (int i=0; i<eventGroups.Count; ++i)
                {
                    eventGroups[i].OnModify<T>(entityId);
                }
            }
        }

        public void Remove(int entityId)
        {
            if (idIdxMap.TryGetValue(entityId, out int idx))
            {
                var unit = units[idx];
                unit.Component.Reset();
                unit.Owner = null;
                unUsedIdxs.Enqueue(idx);
                idIdxMap.Remove(entityId);
                --Count;
                for (int i = 0; i < eventGroups.Count; ++i)
                {
                    eventGroups[i].OnRemove<T>(entityId);
                }
            }
        }

        public Entity GetValid(int startIndex)
        {
            for (int i=startIndex; i<units.Count; ++i)
            {
                var unit = units[i];
                if (unit.Owner != null)
                {
                    return unit.Owner;
                }
            }
            return null;
        }

        public Entity GetValid(int startIndex, out IComponent component)
        {
            for (int i = startIndex; i < units.Count; ++i)
            {
                var unit = units[i];
                if (unit.Owner != null)
                {
                    component = unit.Component;
                    return unit.Owner;
                }
            }
            component = null;
            return null;
        }

        public void RegistEventGroup(IEventGroup eventGroup)
        {
            eventGroups.Add(eventGroup);
        }
    }

}