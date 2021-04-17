using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ILAppAssembly : IAppAssembly
{
    public static ILRuntime.Runtime.Enviorment.AppDomain Domain { get; private set; }
    private MemoryStream dllStream;
    private MemoryStream pdbStream;
    
    public IStaticMethod GetStaticMethod(string typeName, string methodName, int paramCount)
    {
        var type = Domain.GetType(typeName);
        if (type != null)
        {
            var method = type.GetMethod(methodName, paramCount);
            return new ILStaticMethod(method, paramCount);
        }
        return null;
    }

    public List<Type> GetTypes()
    {
        return Domain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();
    }

    public void Load(byte[] assBytes, byte[] pdbBytes)
    {
        Domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        dllStream = new MemoryStream(assBytes);
        if (pdbBytes != null)
        {
            pdbStream = new MemoryStream(pdbBytes);
            Domain.LoadAssembly(dllStream, pdbStream, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        }
        else
        {
            Domain.LoadAssembly(dllStream);
        }
#if ENABLE_PROFILER && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS)
        //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
        Domain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
        ILRegister.InitILRuntime(Domain);
#if DEBUG
        Domain.DebugService.StartDebugService(56000);
#endif
        Debug.Log($"当前使用的是ILRuntime模式");
    }
    
    public void Destroy()
    {
        Domain = null;
        if (dllStream != null)
        {
            dllStream.Close();
            dllStream = null;
        }
        if (pdbStream != null)
        {
            pdbStream.Close();
            pdbStream = null;
        }
    }
}
