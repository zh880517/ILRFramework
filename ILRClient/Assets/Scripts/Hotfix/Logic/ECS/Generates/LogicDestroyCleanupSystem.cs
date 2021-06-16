using TComponent = LogicDestroy;
public class LogicDestroyCleanupSystem : ECS.Core.ICleanupSystem
{
    private LogicContext context;
    public LogicDestroyCleanupSystem(LogicContext context)
    {
        this.context = context;
    }
    public void OnCleanup()
    {
        var group = context.CreatGroup<TComponent>();
        while (group.TryGet(out var entity, out _))
        {
            entity.Destroy();
        }
    }
    
}
