using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

[InitializeOnLoad]
public class HotfixBuild
{
    private const string ScriptAssembliesDir = "Library/ScriptAssemblies/";
    private const string Name = "Hotfix";
    private const string HotfixDll = "Hotfix.dll";
    private const string ScriptsPath = "Assets/Scripts/Hotfix";
    public const string BuildOutputPath = "BuildOutput/";

    public static string DllFullPath { get { return string.Format("{0}{1}.dll", BuildOutputPath, Name); } }
    public static string UnityTmpDllFullPath { get { return string.Format("{0}{1}", ScriptAssembliesDir, HotfixDll); } }
    static HotfixBuild()
    {
        if (IsDisableAutoBuild() || Application.isPlaying)
            return;
        do
        {
            FileInfo logicInfo = new FileInfo(string.Format("{0}{1}.Logic.dll", ScriptAssembliesDir, Name));
            if (!logicInfo.Exists)
                break;
            FileInfo viewInfo = new FileInfo(string.Format("{0}{1}.View.dll", ScriptAssembliesDir, Name));
            if (!viewInfo.Exists)
                break;
            FileInfo dllInfo = new FileInfo(DllFullPath);
            if (!dllInfo.Exists)
                break;
            if (dllInfo.LastWriteTime > logicInfo.LastWriteTime && dllInfo.LastWriteTime > viewInfo.LastWriteTime)
                return;
        } while (false);
        DoBuild(IsDebugBuild(), false);
    }

    private static void CheckOutputPath()
    {
        if (!Directory.Exists(BuildOutputPath))
        {
            Directory.CreateDirectory(BuildOutputPath);
        }

    }

    [MenuItem("ILRuntime/编译/Debug Mode")]
    public static void BuildHotFixDebug()
    {
        DoBuild(true);
    }
    [MenuItem("ILRuntime/编译/Release Mode")]
    public static void BuildHotFixRelease()
    {
        DoBuild(false);
    }
    [MenuItem("ILRuntime/禁用自动编译", true)]
    public static bool CheckDisableAutoBuild()
    {
        Menu.SetChecked("ILRuntime/禁用自动编译", IsDisableAutoBuild());
        return true;
    }

    [MenuItem("ILRuntime/禁用自动编译")]
    public static void DistableAutoBuild()
    {
        EditorPrefs.SetBool("_disableirlbuild_", !IsDisableAutoBuild());
        if (!IsDisableAutoBuild())
        {
            DoBuild(IsDebugBuild());
        }
    }

    public static bool IsDisableAutoBuild()
    {
        return EditorPrefs.GetBool("_disableirlbuild_", false);
    }

    [MenuItem("ILRuntime/DebudBuild", true)]
    public static bool CheckDebugMode()
    {
        Menu.SetChecked("ILRuntime/DebudBuild", IsDebugBuild());
        return !IsDisableAutoBuild();
    }

    [MenuItem("ILRuntime/DebudBuild")]
    public static void SwitchDebugMode()
    {
        EditorPrefs.SetBool("_enabledebugbuild_", !IsDebugBuild());
        if (!IsDisableAutoBuild())
        {
            DoBuild(IsDebugBuild());
        }
    }

    public static bool IsDebugBuild()
    {
        return EditorPrefs.GetBool("_enabledebugbuild_", false);
    }


    public static void DoBuild(bool debugMode, bool waitComplete = true)
    {
        CheckOutputPath();
        var csFiles = Directory.GetFiles(ScriptsPath, "*.cs", SearchOption.AllDirectories);
        List<string> files = new List<string>(csFiles.Length);
        foreach (var file in csFiles)
        {
            files.Add(file.Replace("\\", "/"));
        }
        var builder = new AssemblyBuilder(DllFullPath, files.ToArray())
        {
            flags = debugMode ? AssemblyBuilderFlags.DevelopmentBuild : AssemblyBuilderFlags.None
        };
        builder.excludeReferences = new string[]
        {
            "Library/ScriptAssemblies/Hotfix.dll",
            "Library/ScriptAssemblies/Hotfix.Framework.dll",
            "Library/ScriptAssemblies/Hotfix.Logic.dll",
            "Library/ScriptAssemblies/Hotfix.View.dll",
        };
        if (!debugMode)
        {
            var buildOption = builder.compilerOptions;
            buildOption.CodeOptimization = CodeOptimization.Release;
            builder.compilerOptions = buildOption;
        }
        builder.buildFinished += OnBuildFinished;
        builder.buildStarted += delegate (string path) { Debug.Log("开始编译Hotfix"); };
        
        builder.Build();
        while (waitComplete && builder.status != AssemblyBuilderStatus.Finished)
        {
            EditorUtility.DisplayProgressBar("编译", "等待编译结束", 0.5f);
        }
        EditorUtility.ClearProgressBar();
    }

    private static void OnBuildFinished(string assemblyPath, CompilerMessage[] compilerMessages)
    {
        int errorCount = 0;
        foreach (var msg in compilerMessages)
        {
            if (msg.type == CompilerMessageType.Error)
            {
                Debug.LogError(CompilerMessageToString(msg));
            }
            else
            {
                Debug.Log(CompilerMessageToString(msg));
            }
        }
        if (errorCount > 0)
        {
            Debug.LogError("Hotfix 模块编译失败");
        }
        else
        {
            Debug.Log("Hotfix 模块编译完成");
            File.Copy(DllFullPath, string.Format("Assets/Resources/{0}.bytes", HotfixDll), true);
        }
    }

    private static string CompilerMessageToString(CompilerMessage message)
    {
        string filePath = message.file.Replace("\\", "/");
        return string.Format("{0}\nHotfixBuild() (at {1}:{2})\r\n", message.message, filePath, message.line);
    }
}
