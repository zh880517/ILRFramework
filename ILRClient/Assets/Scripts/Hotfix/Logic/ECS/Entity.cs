namespace ECS.Core
{
    public class Entity
    {
        public int Id { get; private set; }
        private int generation;
        protected Context owner { get; private set; }

        public Entity(Context context, int id)
        {
            Id = id;
            generation = context.Generation;
            owner = context;
        }

        public void Destroy()
        {
            if (owner != null)
            {
                owner.DestroyEntity(this);
            }
            owner = null;

        }

        public bool Check(Context context)
        {
            return owner == context && context.Generation == generation;
        }

        public static implicit operator bool(Entity exists)
        {
            return exists != null && exists.owner != null && exists.owner.Generation == exists.generation;
        }

        public T Get<T>() where T : class, IComponent, new()
        {
            return owner.GetComponent<T>(this);
        }

        public T Add<T>() where T : class, IComponent, new()
        {
            return owner.AddComponent<T>(this);
        }

        public void Modify<T>() where T : class, IComponent, new()
        {
            owner.SetComponentModify<T>(this);
        }

        public void Remove<T>() where T : class, IComponent, new()
        {
            owner.RemoveComponent<T>(this);
        }

    }
}