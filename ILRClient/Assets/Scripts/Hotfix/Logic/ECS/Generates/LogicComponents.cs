public static partial class LogicComponents
{
    static LogicComponents()
    {
        OnContextCreat = DoContentInit;
        ComponentCount = 8;
        InitComponentsIdentity();
    }
    static void InitComponentsIdentity()
    {
        ECS.Core.ComponentIdentity<LogicBlockMoveInput>.Id = 0;
        ECS.Core.ComponentIdentity<LogicMoveInput>.Id = 1;
        ECS.Core.ComponentIdentity<LogicCollisionRadius>.Id = 2;
        ECS.Core.ComponentIdentity<LogicDestroy>.Id = 3;
        ECS.Core.ComponentIdentity<LogicForward>.Id = 4;
        ECS.Core.ComponentIdentity<LogicPosition>.Id = 5;
        ECS.Core.ComponentIdentity<LogicFrameMove>.Id = 6;
        ECS.Core.ComponentIdentity<LogicSlideMove>.Id = 7;
    }
    static void DoContentInit(LogicContext context)
    {
        context.InitComponentCollector<LogicBlockMoveInput>();
        context.InitComponentCollector<LogicMoveInput>();
        context.InitComponentCollector<LogicCollisionRadius>();
        context.InitComponentCollector<LogicDestroy>();
        context.InitComponentCollector<LogicForward>();
        context.InitComponentCollector<LogicPosition>();
        context.InitComponentCollector<LogicFrameMove>();
        context.InitComponentCollector<LogicSlideMove>();
    }
    
}
