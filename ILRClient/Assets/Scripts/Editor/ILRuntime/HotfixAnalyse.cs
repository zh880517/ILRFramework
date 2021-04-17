using ILRuntime.Mono.Cecil;
using ILRuntime.Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class HotfixAnalyse
{
    [MenuItem("ILRuntime/分析")]
    public static void Analyse()
    {
        AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(HotfixBuild.DllFullPath);
        foreach (var module in assembly.Modules)
        {
            foreach (var typeref in module.GetTypeReferences())
            {
                Debug.Log("引用类型" + typeref.FullName);
            }
            HashSet<string> opcodes = new HashSet<string>();
            foreach (var type in module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    foreach (var il in method.Body.Instructions)
                    {
                        if (il.Operand != null)
                        {
                            string code = string.Format("{0} - {1}", il.OpCode, il.Operand.GetType().FullName);
                            opcodes.Add(code);
                        }
                    }
                    var calls = method.Body.Instructions.Where(it => it.OpCode == OpCodes.Call).ToList();
                    foreach (var call in calls)
                    {
                        var mRef = call.Operand as MethodReference;
                        if (mRef != null)
                        {
                            Debug.Log(mRef.FullName);
                        }
                    }
                }
            }
            Debug.Log(string.Join("\n", opcodes));
        }
    }
}
