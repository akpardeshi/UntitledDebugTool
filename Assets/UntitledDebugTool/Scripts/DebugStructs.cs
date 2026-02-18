using UnityEngine;
using UnityEngine.Serialization;

namespace ModularDebugSystem.Debug
{
    [System.Serializable]
    public struct DebugDataWrapper
    {
        public DebugLogType logType;
        public Color messageColor;
        public Color headingColor;

        public DebugDataWrapper(DebugLogType logType, Color messageColor, Color headingColor)
        {
            this.logType = logType;
            this.messageColor = messageColor;
            this.headingColor = headingColor;
        }
    }
}