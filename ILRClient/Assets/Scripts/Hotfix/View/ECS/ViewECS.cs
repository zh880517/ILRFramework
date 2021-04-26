public interface IViewComponent : ECS.Core.IComponent
{
}

public class ViewEntity : ECS.Core.TEntity<IViewComponent>
{
    public ViewEntity(ECS.Core.Context context, int id) : base(context, id)
    {
    } 
}
public static partial class ViewComponents
{
    public static System.Action<ViewContext> OnContextCreat;
    public static int ComponentCount { get; private set; }
    
}
public class ViewContext : ECS.Core.TContext<ViewEntity>
{
    public ViewContext(int componentTypeCount) : base(componentTypeCount, CreatFunc)
    {
    }

    private static ViewEntity CreatFunc(ECS.Core.Context context, int id)
    {
        return new ViewEntity(context, id);
    }
    public static ViewContext Creat()
    {
        var contxt = new ViewContext(ViewComponents.ComponentCount);
        ViewComponents.OnContextCreat(contxt);
        return contxt;
    }
}
