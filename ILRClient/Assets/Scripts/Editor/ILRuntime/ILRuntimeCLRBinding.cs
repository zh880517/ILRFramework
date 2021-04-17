using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
[System.Reflection.Obfuscation(Exclude = true)]
public static class ILRuntimeCLRBinding
{
    [MenuItem("ILRuntime/通过自动分析热更DLL生成CLR绑定")]
    static void GenerateCLRBindingByAnalysis()
    {
        //用新的分析热更dll调用引用来生成绑定代码
        ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        using (FileStream fs = new FileStream("Library/ScriptAssemblies/Hotfix.dll", FileMode.Open, FileAccess.Read))
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

#if ILRuntime
    [MenuItem("ILRuntime/切换到Mono模式", false, 10)]
    static void UseMono()
    {
        var old = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (old.Contains("ILRuntime"))
        {
            string newDefine = old.Replace("ILRuntime;", "").Replace("ILRuntime", "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefine);
        }
    }
#else
    [MenuItem("ILRuntime/切换到ILRuntime模式", false, 10)]
    static void UseILRuntime()
    {
        var old = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (!old.Contains("ILRuntime"))
        {
            string newDefine;
            if (old.EndsWith(";"))
            {
                newDefine = old + "ILRuntime";
            }
            else
            {
                newDefine = old + ";ILRuntime";
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefine);
        }
    }
#endif
}
