using System.Collections.Generic;

namespace ECS.Core
{
    public class UniqueComponentCollector<T> : IComponentCollectorT<T> where T : class, IComponent, IUnique, new()
    {
        public int Count => Component.Owner != null ? 1 : 0;
        private ComponentEntity<T> Component = new ComponentEntity<T>();
        public IComponent Add(Entity entity, bool forceModify)
        {
            if (Component.Owner == entity)
            {
                return Component.Component;
            }
            else
            {
                Remove(entity);
            }
            Component.Owner = entity;
            Component.Status = ComponentStatus.Add;
            return Component.Component;
        }

        public IComponent Get(Entity entity)
        {
            if (entity == Component.Owner)
                return Component.Component;
            return null;
        }

        public T Get()
        {
            if (Component.Owner != null)
            {
                return Component.Component;
            }
            return null;
        }

        public ECPair<TEntity, T> GetPair<TEntity>() where TEntity : Entity
        {
            if (Component.Owner != null)
            {
                return new ECPair<TEntity, T> { Owner = Component.Owner as TEntity, Value = Component.Component };
            }
            return new ECPair<TEntity, T>();
        }

        public Entity Find(ref int startIndex, ComponentStatus status, out T component, System.Func<T, bool> condition)
        {
            if (startIndex == 0 && Component.Owner != null && status <= Component.Status && (condition == null || condition(Component.Component)))
            {
                startIndex = 1;
                component = Component.Component;
                return Component.Owner;
            }
            component = null;
            startIndex = 1;
            return null;
        }

        public Entity Find(ref int startIndex, ComponentStatus status, System.Func<T, bool> condition)
        {
            if (startIndex == 0 && Component.Owner != null && status <= Component.Status && (condition == null || condition(Component.Component)))
            {
                startIndex = 1;
                return Component.Owner;
            }
            startIndex = 1;
            return null;
        }

        public IComponent Modify(Entity entity)
        {
            if (Component.Owner == entity)
            {
                Component.Modify();
                return Component.Component;
            }
            return null;
        }

        public void Remove(Entity entity)
        {
            if (entity == Component.Owner)
            {
                int id = Component.Owner.Id;
                Component.Reset();
            }
        }

        public void RemoveAll()
        {
            if (Component.Owner != null)
            {
                int id = Component.Owner.Id;
                Component.Reset();
            }
        }

        public void OnTickEnd()
        {
            Component.Status = ComponentStatus.Normal;
        }
    }

}
