namespace ECS.Core
{
    public class TEntity<TComponent> : Entity where TComponent : IComponent
    {
        public TEntity(Context context, int id) : base(context, id)
        {
        }

        public T Get<T>() where T : class, TComponent, new()
        {
            return owner.GetComponent<T>(this);
        }

        public bool Has<T>() where T : class, TComponent, new()
        {
            return owner.GetComponent<T>(this) != null;
        }

        public T Add<T>() where T : class, TComponent, new()
        {
            return owner.AddComponent<T>(this);
        }

        public void Modify<T>() where T : class, TComponent, new()
        {
            owner.SetComponentModify<T>(this);
        }

        public void Remove<T>() where T : class, TComponent, new()
        {
            owner.RemoveComponent<T>(this);
        }
    }

}
