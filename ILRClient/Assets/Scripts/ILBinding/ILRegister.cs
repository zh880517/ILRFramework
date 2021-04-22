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

		//值类型绑定，后续改成自动生成
        appdomain.RegisterValueTypeBinder(typeof(UnityEngine.Vector3), new Vector3Binder());
        appdomain.RegisterValueTypeBinder(typeof(UnityEngine.Quaternion), new QuaternionBinder());
        appdomain.RegisterValueTypeBinder(typeof(UnityEngine.Vector2), new Vector2Binder());
        appdomain.RegisterValueTypeBinder(typeof(UnityEngine.Vector2Int), new Vector2IntBinder());
        appdomain.RegisterValueTypeBinder(typeof(FP), new Fix64Binder());
        appdomain.RegisterValueTypeBinder(typeof(TSVector2), new TSVector2Binder());
        appdomain.RegisterValueTypeBinder(typeof(TSVector), new TSVectorBinder());
        appdomain.RegisterValueTypeBinder(typeof(TSQuaternion), new TSQuaternionBinder());
        appdomain.RegisterValueTypeBinder(typeof(TSMatrix), new TSMatrixBinder());
        appdomain.RegisterValueTypeBinder(typeof(TSMatrix4x4), new TSMatrix4x4Binder());

		CLRBindings.Initialize(appdomain);
        appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());

        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
	}

	public static void DoDestroy(AppDomain appdomain)
    {
		CLRBindings.Shutdown(appdomain);

	}

}