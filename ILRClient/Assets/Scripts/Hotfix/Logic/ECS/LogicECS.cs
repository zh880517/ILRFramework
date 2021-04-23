
public interface ILogicComponent : ECS.Core.IComponent
{

}


public class LogicEntity : ECS.Core.TEntity<ILogicComponent>
{
    public LogicEntity(ECS.Core.Context context, int id) : base(context, id)
    {
    }
}

public static partial class LogicComponents
{
    public static System.Action<LogicContext> OnContextCreat;
    public static int ComponentCount { get; private set; }
}

public class LogicContext : ECS.Core.TContext<LogicEntity>
{
    public LogicContext(int componentTypeCount) : base(componentTypeCount, CreatFunc)
    {
    }

    private static LogicEntity CreatFunc(ECS.Core.Context context, int id)
    {
        return new LogicEntity(context, id);
    }

    public static LogicContext Creat()
    {
        var contxt = new LogicContext(LogicComponents.ComponentCount);
        LogicComponents.OnContextCreat(contxt);
        return contxt;
    }
}