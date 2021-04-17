using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Compilation;
using System.Collections.Generic;
using System;

[InitializeOnLoad]
public class HotfixBuild
{
    private const string ScriptAssembliesDir = "Library/ScriptAssemblies/";
    private const string HotfixDll = "Hotfix.dll";
    private const string HotfixPdb = "Hotfix.pdb";
    private const string ScriptsPath = "Assets/Scripts/Hotfix";
    public const string BuildOutputPath = "BuildOutput/";

    public static string DllFullPath { get { return string.Format("{0}{1}", BuildOutputPath, HotfixDll); } }
    public static string UnityTmpDllFullPath { get { return string.Format("{0}{1}", ScriptAssembliesDir, HotfixDll); } }
    static HotfixBuild()
    {
        if (IsDisableAutoBuild())
            return;
        FileInfo info = new FileInfo(UnityTmpDllFullPath);
        do 
        {
            if (!info.Exists)
                break;
            FileInfo dllInfo = new FileInfo(DllFullPath);
            if (!dllInfo.Exists)
                break;
            if (dllInfo.LastWriteTime > info.LastWriteTime)
                return;
        } while (false);
        DoBuild(true);
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
    }

    public static bool IsDisableAutoBuild()
    {
        return EditorPrefs.GetBool("_disableirlbuild_", false);
    }

    public static void DoBuild(bool debugMode)
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
            "Library/ScriptAssemblies/Hotfix.dll"
        };
        builder.buildFinished += OnBuildFinished;
        builder.buildStarted += delegate (string path) { Debug.Log("开始编译Hotfix"); };
        
        builder.Build();
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
        }
    }

    private static string CompilerMessageToString(CompilerMessage message)
    {
        return string.Format("{0}\n(at{1}:{2})", message.message, message.file, message.line);
    }
}
