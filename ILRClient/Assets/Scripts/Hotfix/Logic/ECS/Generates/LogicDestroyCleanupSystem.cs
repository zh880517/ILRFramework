using TCompoment = LogicDestroy;
public class LogicDestroyCleanupSystem : ECS.Core.ICleanupSystem
{
    private readonly ECS.Core.Group<TCompoment> group;
    public LogicDestroyCleanupSystem(LogicContext context)
    {
        group = context.CreatGroup<TCompoment>();
    }
    public void OnCleanup()
    {
        if (group.Count > 0)
        {
            while (group.TryGet(out ECS.Core.Entity entity, out _))
            {
                entity.Destroy();
            }
            group.Reset();
        }
        
    }
    
}
