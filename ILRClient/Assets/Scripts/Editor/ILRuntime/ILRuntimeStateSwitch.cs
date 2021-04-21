using System.Collections.Generic;
using UnityEditor;

public static class ILRuntimeStateSwitch
{

    public static List<string> GetDefine()
    {
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        return new List<string>(defines.Split(';'));
    }

    public static void SetDefine(List<string> defines)
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", defines));
    }

#if ILRuntime
    [MenuItem("ILRuntime/模式切换/切换到Mono模式", false, 10)]
    static void UseMono()
    {
        var defins = GetDefine();
        defins.Remove("ILRuntime");
        SetDefine(defins);
    }
#else
    [MenuItem("ILRuntime/模式切换/切换到ILRuntime模式", false, 10)]
    static void UseILRuntime()
    {
        var defins = GetDefine();
        defins.Add("ILRuntime");
        SetDefine(defins);
    }
#endif

#if DISABLE_ILRUNTIME_DEBUG
    [MenuItem("ILRuntime/模式切换/Enable IL Debug", false, 10)]
    static void DisableDebug()
    {
        var defins = GetDefine();
        defins.Remove("DISABLE_ILRUNTIME_DEBUG");
        SetDefine(defins);
    }
#else
    [MenuItem("ILRuntime/模式切换/Disable IL Debug", false, 10)]
    static void EnableDebug()
    {
        var defins = GetDefine();
        defins.Add("DISABLE_ILRUNTIME_DEBUG");
        SetDefine(defins);
    }
#endif
}
