using UnityEngine;

public static class HotfixApp
{
    private static void Init()
    {
        ILLog.Log("hahahah");

        LogicContext.Creat();
        try
        {
            throw new System.Exception("hhh");
        }
        catch (System.Exception ex)
        {
            ILLog.LogException(ex);
        }
    }

    //根据需要实现下面的函数，如果没有对应的函数则不会调用
    //没有使用到的update函数删除以优化性能 
    private static void Update()
    {
    }
    //private static void LateUpdate() { }
    //private static void FixedUpdate() { }
    //private static void OnApplicationQuit() { }
    //private static void OnApplicationFocus(bool focus) { }
    //private static void OnApplicationPause(bool pause) { }
}
