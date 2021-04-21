using System.IO;
using UnityEditor;
[System.Reflection.Obfuscation(Exclude = true)]
public static class ILRuntimeCLRBinding
{
    /// <summary>
    /// 类型绑定注意事项：
    /// 1、值类型的绑定为了简化只实现ValueTypeBinder<T>的 CopyValueTypeToStack和AssignFromStack 两个接口
    /// 这样在热更新模块调用外部值类型的操作时就不用担心装箱拆箱的GC问题，如果没有实现ValueTypeBinder<T>，是有装箱的
    /// 也可以实现部分函数，不过通过自动生成比较方便
    /// 2、
    /// </summary>


    [MenuItem("ILRuntime/通过自动分析热更DLL生成CLR绑定")]
    static void GenerateCLRBindingByAnalysis()
    {
        //用新的分析热更dll调用引用来生成绑定代码
        ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        using (FileStream fs = new FileStream(HotfixBuild.DllFullPath, FileMode.Open, FileAccess.Read))
        {
	        domain.LoadAssembly(fs);
	        //Crossbind Adapter is needed to generate the correct binding code
	        ILRegister.InitILRuntime(domain);
            CustomExportDefine customExportDefine = new CustomExportDefine();

            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, 
                "Assets/Scripts/ILBinding/Binder",
                customExportDefine.valueTypeBinders,
                customExportDefine.delegateTypes,
                "UnityEngine_Debug_Binding"
                );
	        AssetDatabase.Refresh();
        }
    }

    static void GenerateAdapt()
    {
        //第一个参数是需要生成的类型，第二个参数命名空间
        //返回值文件内容，保存到文件即可
        //var script = ILRuntime.Runtime.Enviorment.CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(typeof(TestClassBase), null);
    }

}
