using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

public static class AKDebug
{
    private static bool isDebug = true;

    public static event Action<bool> OnDebugStateSet = b => { };

    public static bool IsDebug => isDebug;

    public static void SetDebug(bool? _isDebug)
    {
        if (_isDebug == null) return;
        isDebug = _isDebug.Value;
        OnDebugStateSet?.Invoke(_isDebug.Value);
    }

    public static void Break()
    {
        if (!IsDebug)
            return;
        Debug.Break();
    }

    public static void Clear()
    {
        Debug.ClearDeveloperConsole();
    }

    public static void Log(object message)
    {
        if (!IsDebug)
            return;
        Debug.Log(message);
    }

    public static void Log(object message, Object context)
    {
        if (!IsDebug)
            return;
        Debug.Log(message, context);
    }

    [StringFormatMethod("format")]
    public static void Log(string format, params object[] args)
    {
        if (!IsDebug)
            return;

        if (args == null || args.Length == 0)
        {
            Debug.Log(format);
            return;
        }

        Debug.LogFormat(format, args);
    }

    [StringFormatMethod("format")]
    public static void Log(Object context, string format, params object[] args)
    {
        if (!IsDebug)
            return;

        if (args == null || args.Length == 0)
        {
            Debug.Log(format, context);
            return;
        }

        Debug.LogFormat(context, format, args);
    }

    public static void LogWarning(object message)
    {
        if (!IsDebug)
            return;
        Debug.LogWarning(message);
    }

    public static void LogWarning(object message, Object context)
    {
        if (!IsDebug)
            return;
        Debug.LogWarning(message, context);
    }

    [StringFormatMethod("format")]
    public static void LogWarning(string format, params object[] args)
    {
        if (!IsDebug)
            return;

        if (args == null || args.Length == 0)
        {
            Debug.LogWarning(format);
            return;
        }

        Debug.LogWarningFormat(format, args);
    }

    [StringFormatMethod("format")]
    public static void LogWarning(Object context, string format, params object[] args)
    {
        if (!IsDebug)
            return;

        if (args == null || args.Length == 0)
        {
            Debug.LogWarning(format, context);
            return;
        }

        Debug.LogWarningFormat(context, format, args);
    }

    public static void LogError(object message)
    {
        if (!IsDebug)
            return;
        Debug.LogError(message);
    }

    public static void LogError(object message, Object context)
    {
        if (!IsDebug)
            return;
        Debug.LogError(message, context);
    }

    [StringFormatMethod("format")]
    public static void LogError(string format, params object[] args)
    {
        if (!IsDebug)
            return;

        if (args == null || args.Length == 0)
        {
            Debug.LogError(format);
            return;
        }

        Debug.LogErrorFormat(format, args);
    }

    [StringFormatMethod("format")]
    public static void LogError(Object context, string format, params object[] args)
    {
        if (!IsDebug)
            return;

        if (args == null || args.Length == 0)
        {
            Debug.LogError(format, context);
            return;
        }

        Debug.LogErrorFormat(context, format, args);
    }

    public static void LogException(Exception exception)
    {
        if (!IsDebug)
            return;
        Debug.LogException(exception);
    }

    public static void LogException(Exception exception, Object context)
    {
        if (!IsDebug)
            return;
        Debug.LogException(exception, context);
    }

    public static void DrawRay(Vector3 start, Vector3 dir)
    {
        if (!IsDebug)
            return;
        Debug.DrawRay(start, dir);
    }

    public static void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        if (!IsDebug)
            return;
        Debug.DrawRay(start, dir, color);
    }
}
