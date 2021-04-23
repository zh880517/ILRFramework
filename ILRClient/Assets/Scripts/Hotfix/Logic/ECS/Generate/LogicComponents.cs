
public static partial class LogicComponents
{
    static LogicComponents()
    {
        OnContextCreat = DoContentInit;
        ComponentCount = 0;
        InitComponentsIdentity();
    }

    static void InitComponentsIdentity()
    {
        //ECS.Core.ComponentIdentity<>.Id = 1;
    }

    static void DoContentInit(LogicContext context)
    {
        //context.InitComponentCollector<>();
    }
}
