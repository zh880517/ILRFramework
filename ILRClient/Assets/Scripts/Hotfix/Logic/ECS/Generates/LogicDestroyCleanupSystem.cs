using TComponent = LogicDestroy;
public class LogicDestroyCleanupSystem : ECS.Core.ICleanupSystem
{
    private readonly LogicContext context;
    public LogicDestroyCleanupSystem(LogicContext context)
    {
        this.context = context;
    }
    public void OnCleanup()
    {
        var group = context.CreatGroup<TComponent>();
        while (group.MoveNext())
        {
            group.Entity.Destroy();
        }
        
    }
    
}
