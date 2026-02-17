using UnityEngine;

namespace ModularDebugSystem.Debug
{
    public static class ModularDebugger
    {
        private static DebugManager _debugManager;

        private static readonly string WhiteClrString = ColorUtility.ToHtmlStringRGBA(Color.wheat);
        private static readonly string YellowClrString = ColorUtility.ToHtmlStringRGBA(Color.yellowNice);
        private static readonly string RedClrString = ColorUtility.ToHtmlStringRGBA(Color.red);
        
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

            UnityEngine.Debug.Log(
                $"[<b><i><color=#{WhiteClrString}>{channelName}</color></i></b>]: <color=#{color}>{message}</color>", go);
        }

        public static void LogWarning(string channelName, string message, GameObject go = null)
        {
            var debugChannel = _debugManager.GetDebugChannel(channelName);

            if (!debugChannel.CanBeDebugged) return;

            debugChannel.DebugDataDict.TryGetValue(DebugLogType.Warning, out Color clr);
            var color = ColorUtility.ToHtmlStringRGBA(clr);

            UnityEngine.Debug.LogWarning(
                $"[<b><i><color=#{YellowClrString}>{channelName}</color></i></b>]: <color=#{color}>{message}</color>", go);
        }

        public static void LogError(string channelName, string message, GameObject go = null)
        {
            var debugChannel = _debugManager.GetDebugChannel(channelName);
            if (!debugChannel.CanBeDebugged) return;

            debugChannel.DebugDataDict.TryGetValue(DebugLogType.Error, out Color clr);

            string color = ColorUtility.ToHtmlStringRGBA(clr);

            UnityEngine.Debug.LogError(
                $"[<b><i><color=#{RedClrString}>{channelName}</color></i></b>]: <color=#{color}>{message}</color>", go);
        }
    }
}