using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Utils
{
    public static class Logger
    {
        [Conditional("DEBUG")]
        public static void Log(this object target, string message, [CallerFilePath] string callerPath = "", [CallerMemberName] string caller = "")
        {
            Log(message, callerPath, caller);
        }

        [Conditional("DEBUG")]
        public static void LogWarning(this object target, string message, [CallerFilePath] string callerPath = "", [CallerMemberName] string caller = "")
        {
            LogWarning(message, callerPath, caller);
        }

        [Conditional("DEBUG")]
        public static void LogError(this object target, string message, [CallerFilePath] string callerPath = "", [CallerMemberName] string caller = "")
        {
            LogError(message, callerPath, caller);
        }

        [Conditional("DEBUG")]
        public static void Log(string message, [CallerFilePath] string callerPath = "", [CallerMemberName] string caller = "")
        {
            UnityEngine.Debug.Log(FormatMessage(message, callerPath, caller));
        }

        [Conditional("DEBUG")]
        public static void LogWarning(string message,[CallerFilePath] string callerPath = "", [CallerMemberName] string caller = "")
        {
            UnityEngine.Debug.LogWarning(FormatMessage(message, callerPath, caller));
        }

        [Conditional("DEBUG")]
        public static void LogError(string message,[CallerFilePath] string callerPath = "", [CallerMemberName] string caller = "")
        {
            UnityEngine.Debug.LogError($"<color=red>Error</color> {FormatMessage(message, callerPath, caller)}");
        }

        private static string FormatMessage(string message, string callerPath, string caller)
        {
            var callerTypeName = Path.GetFileNameWithoutExtension(callerPath);
            
            return //$"{DateTime.Now:HH:mm:ss.fff} " +
                   $"[{callerTypeName}.{caller}] {message}" ;
        }
    }
}