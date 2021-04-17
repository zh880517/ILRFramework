using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Enviorment;
public static class ILRegister
{
	public static void InitILRuntime(AppDomain appdomain)
	{
		// 注册重定向函数

		// 注册委托
		//appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
		//appdomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();
		//appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();

		//注册绑定的导出类
		//Debug的功能需要打印堆栈，所以这里要自定义，不能用系统生成的
		UnityEngineDebugBinding.Register(appdomain);
		CLRBindings.Initialize(appdomain);
        appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());

        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
	}


}