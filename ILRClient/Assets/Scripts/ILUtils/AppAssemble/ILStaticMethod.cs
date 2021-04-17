using ILRuntime.CLR.Method;

public class ILStaticMethod : IStaticMethod
{
    private readonly IMethod method;
    private readonly object[] param;

    public ILStaticMethod(IMethod method, int paramsCount)
    {
        this.method = method;
        param = new object[paramsCount];
    }

    public void Run()
    {
        ILAppAssembly.Domain?.Invoke(method, null);
    }

    public void Run(object a)
    {
        param[0] = a;
        ILAppAssembly.Domain?.Invoke(method, null, param);
    }

    public void Run(object a, object b)
    {
        param[0] = a;
        param[1] = b;
        ILAppAssembly.Domain?.Invoke(method, null, param);
    }

    public void Run(object a, object b, object c)
    {
        param[0] = a;
        param[1] = b;
        param[2] = c;
        ILAppAssembly.Domain?.Invoke(method, null, param);
    }
    
}
