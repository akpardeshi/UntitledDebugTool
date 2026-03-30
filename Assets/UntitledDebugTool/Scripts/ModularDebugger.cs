using UnityEngine;

namespace ModularDebugSystem.Debug
{
    public static class ModularDebugger
    {
        private static DebugManager _debugManager;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _debugManager = null;
        }

        public static void Initialize(DebugManager debugManager)
        {
            _debugManager = debugManager;
        }

        public static void Log(string channelName, string message, GameObject go = null)
        {
            LogInternal(channelName, message, DebugLogType.Debug, go);
        }

        public static void LogWarning(string channelName, string message, GameObject go = null)
        {
            LogInternal(channelName, message, DebugLogType.Warning, go);
        }

        public static void LogError(string channelName, string message, GameObject go = null)
        {
            LogInternal(channelName, message, DebugLogType.Error, go);
        }

        private static void LogInternal(string channelName, string message, DebugLogType logType, GameObject go)
        {
            string formatted;
            
            // Manager exists
            if (_debugManager)
            {
                var debugChannel = _debugManager.GetDebugChannel(channelName);

                if (!debugChannel.CanBeDebugged) return;

                var wrapper = debugChannel.GetDebugDataWrapper(logType);
                var messageColor = ColorUtility.ToHtmlStringRGBA(wrapper.messageColor);
                var headingColor = ColorUtility.ToHtmlStringRGBA(wrapper.headingColor);

                formatted =
                    $"[<b><i><color=#{headingColor}>{channelName}</color></i></b>]: <color=#{messageColor}>{message}</color>";

                switch (logType)
                {
                    case DebugLogType.Debug:
                        UnityEngine.Debug.Log(formatted, go);
                        break;

                    case DebugLogType.Warning:
                        UnityEngine.Debug.LogWarning(formatted, go);
                        break;

                    case DebugLogType.Error:
                        UnityEngine.Debug.LogError(formatted, go);
                        break;
                }

                return;
            }

            // Fallback if manager missing
            Color fallbackColor = GetFallbackColor(logType);
            var colorHex = ColorUtility.ToHtmlStringRGBA(fallbackColor);

            formatted =
                $"[<b><i><color=#{colorHex}>{channelName}</color></i></b>]: <color=#{colorHex}>{message}</color>"; 
            
            switch (logType)
            {
                case DebugLogType.Warning:
                    UnityEngine.Debug.LogWarning(formatted, go);
                    break;
                case DebugLogType.Error:
                    UnityEngine.Debug.LogError(formatted, go);
                    break;
                default:
                    UnityEngine.Debug.Log(formatted, go);
                    break;
            }
        }

        private static Color GetFallbackColor(DebugLogType logType)
        {
            switch (logType)
            {
                case DebugLogType.Warning:
                    return Color.yellowNice;

                case DebugLogType.Error:
                    return Color.red;

                case DebugLogType.Debug:
                default:
                    return Color.white;
            }
        }
    }
}