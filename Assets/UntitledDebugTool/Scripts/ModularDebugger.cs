using UnityEngine;

namespace ModularDebugSystem.Debug
{
    public static class ModularDebugger
    {
        private static DebugManager _debugManager;

        public static void Initialize(DebugManager debugManager)
        {
            _debugManager = debugManager;
        }

        public static void Log(string channelName, string message, GameObject go = null)
        {
            var debugChannel = _debugManager.GetDebugChannel(channelName);

            if (!debugChannel.CanBeDebugged) return;

             debugChannel.DebugDataDict.TryGetValue(DebugLogType.Debug, out Color clr);
             
             var color = ColorUtility.ToHtmlStringRGBA(clr);
             var clrString = ColorUtility.ToHtmlStringRGBA(Color.wheat);
             
            UnityEngine.Debug.Log($"[<b><i><color=#{clrString}>{channelName}</color></i></b>]: <color=#{color}>{message}</color>", go);
        }

        public static void LogWarning(string channelName, string message, GameObject go = null)
        {
            var debugChannel = _debugManager.GetDebugChannel(channelName);
            
            if (!debugChannel.CanBeDebugged) return;
            
            debugChannel.DebugDataDict.TryGetValue(DebugLogType.Warning, out Color clr);
            var color = ColorUtility.ToHtmlStringRGBA(clr);
            string clrString = ColorUtility.ToHtmlStringRGBA(Color.yellowNice);
            
            UnityEngine.Debug.LogWarning($"[<b><i><color=#{clrString}>{channelName}</color></i></b>]: <color=#{color}>{message}</color>", go);
        }

        public static void LogError(string channelName, string message, GameObject go = null)
        {
            var debugChannel = _debugManager.GetDebugChannel(channelName);
            if (!debugChannel.CanBeDebugged) return;
            
            debugChannel.DebugDataDict.TryGetValue(DebugLogType.Warning, out Color clr);
            
            string color = ColorUtility.ToHtmlStringRGBA(clr);
            string clrString = ColorUtility.ToHtmlStringRGBA(Color.red);
            
            UnityEngine.Debug.LogError($"[<b><i><color=#{clrString}>{channelName}</color></i></b>]: <color=#{color}>{message}</color>", go);
        }
    }
}