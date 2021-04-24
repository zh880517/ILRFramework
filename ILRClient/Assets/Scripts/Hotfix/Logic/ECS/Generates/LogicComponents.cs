public static partial class LogicComponents
{
    static LogicComponents()
    {
        OnContextCreat = DoContentInit;
        ComponentCount = 6;
        InitComponentsIdentity();
    }
    static void InitComponentsIdentity()
    {
        ECS.Core.ComponentIdentity<LogicBlockMoveInput>.Id = 0;
        ECS.Core.ComponentIdentity<LogicMoveInput>.Id = 1;
        ECS.Core.ComponentIdentity<LogicDestroy>.Id = 2;
        ECS.Core.ComponentIdentity<LogicPosition>.Id = 3;
        ECS.Core.ComponentIdentity<LogicFrameMove>.Id = 4;
        ECS.Core.ComponentIdentity<LogicSlideMove>.Id = 5;
    }
    static void DoContentInit(LogicContext context)
    {
        context.InitComponentCollector<LogicBlockMoveInput>();
        context.InitComponentCollector<LogicMoveInput>();
        context.InitComponentCollector<LogicDestroy>();
        context.InitComponentCollector<LogicPosition>();
        context.InitComponentCollector<LogicFrameMove>();
        context.InitComponentCollector<LogicSlideMove>();
    }
    
}
