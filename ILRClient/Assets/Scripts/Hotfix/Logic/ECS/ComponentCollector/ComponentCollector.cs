using System.Collections.Generic;
namespace ECS.Core
{

    public class ComponentCollector<T> : IComponentCollectorT<T> where T : class, IComponent, new()
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

        public IComponent Add(Entity entity)
        {
            if (idIdxMap.TryGetValue(entity.Id, out int idx))
            {
                return units[idx].Component;
            }
            var unit = CreateUnit();
            idIdxMap.Add(entity.Id, idx);
            unit.Owner = entity;
            ++Count;
            for (int i = 0; i < eventGroups.Count; ++i)
            {
                eventGroups[i].OnAdd<T>(entity.Id);
            }
            return unit.Component;
        }

        public IComponent Get(Entity entity)
        {
            if (idIdxMap.TryGetValue(entity.Id, out int idx))
            {
                return units[idx].Component;
            }
            return null;
        }

        public void Modify(Entity entity)
        {
            if (idIdxMap.ContainsKey(entity.Id))
            {
                for (int i=0; i<eventGroups.Count; ++i)
                {
                    eventGroups[i].OnModify<T>(entity.Id);
                }
            }
        }

        public void Remove(Entity entity)
        {
            if (idIdxMap.TryGetValue(entity.Id, out int idx))
            {
                var unit = units[idx];
                if (unit.Component is IReset resetComp)
                    resetComp.Reset();
                unit.Owner = null;
                unUsedIdxs.Enqueue(idx);
                idIdxMap.Remove(entity.Id);
                --Count;
                for (int i = 0; i < eventGroups.Count; ++i)
                {
                    eventGroups[i].OnRemove<T>(entity.Id);
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

        public Entity GetValid(int startIndex, out T component)
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
    }

}