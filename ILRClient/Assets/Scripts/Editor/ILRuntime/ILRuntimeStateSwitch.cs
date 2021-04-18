using UnityEditor;

public static class ILRuntimeStateSwitch
{

#if ILRuntime
    [MenuItem("ILRuntime/模式切换/切换到Mono模式", false, 10)]
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
    [MenuItem("ILRuntime/模式切换/切换到ILRuntime模式", false, 10)]
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

#if DISABLE_ILRUNTIME_DEBUG
    [MenuItem("ILRuntime/模式切换/Enable IL Debug", false, 10)]
    static void DisableDebug()
    {
        var old = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (old.Contains("DISABLE_ILRUNTIME_DEBUG"))
        {
            string newDefine = old.Replace("DISABLE_ILRUNTIME_DEBUG;", "").Replace("DISABLE_ILRUNTIME_DEBUG", "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefine);
        }
    }
#else
    [MenuItem("ILRuntime/模式切换/Disable IL Debug", false, 10)]
    static void EnableDebug()
    {
        var old = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (!old.Contains("DISABLE_ILRUNTIME_DEBUG"))
        {
            string newDefine;
            if (old.EndsWith(";"))
            {
                newDefine = old + "DISABLE_ILRUNTIME_DEBUG";
            }
            else
            {
                newDefine = old + ";DISABLE_ILRUNTIME_DEBUG";
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefine);
        }
    }
#endif
}
