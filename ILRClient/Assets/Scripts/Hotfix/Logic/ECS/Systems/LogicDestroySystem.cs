using ECS.Core;
public class LogicDestroySystem : ICleanupSystem
{
    private readonly Group<LogicDestroy> group;
    public LogicDestroySystem(LogicContext context)
    {
        group = context.CreatGroup<LogicDestroy>();
    }

    public void OnCleanup()
    {
        if (group.Count > 0)
        {
            while (group.TryGet(out Entity entity, out _))
            {
                entity.Destroy();
            }
            group.Reset();
        }
    }
}
