using UnityEngine;

public static class ILLog
{
    public static void Log(object message)
    {
        Debug.Log(message.ToString());
    }

    public static void Log(string message)
    {
        Debug.Log(message);
    }

    public static void LogFormat(string format, params object[] args)
    {
        Debug.Log(string.Format(format, args));
    }

    public static void LogWarning(object message)
    {
        Debug.LogWarning(message.ToString());
    }

    public static void LogWarning(string message)
    {
        Debug.LogWarning(message);
    }

    public static void LogWarningFormat(string format, params object[] args)
    {
        Debug.LogWarning(string.Format(format, args));
    }

    public static void LogException(System.Exception exception)
    {
        Debug.LogException(exception);
    }

    public static void LogError(object message)
    {
        Debug.LogError(message.ToString());
    }

    public static void LogError(string message)
    {
        Debug.LogError(message);
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        Debug.LogError(string.Format(format, args));
    }
}
