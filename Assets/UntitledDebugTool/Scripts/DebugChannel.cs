using System.Collections.Generic;
using UnityEngine;

namespace ModularDebugSystem.Debug
{
    [CreateAssetMenu(fileName = "ModularDebugChannel", menuName = "ModularDebugSystem/DebugChannel")]
    public class DebugChannel : ScriptableObject
    {
        [SerializeField] private bool canBeDebugged;
        [SerializeField] private string moduleName;
        [SerializeField] DebugDataWrapper[] debugData;

        private readonly Dictionary<DebugLogType, DebugDataWrapper> _debugDataDict = new();

        public string ModuleName => moduleName;
        public bool CanBeDebugged => canBeDebugged;

        public DebugDataWrapper GetDebugDataWrapper(DebugLogType logType)
        {
            if (_debugDataDict.TryGetValue(logType, out DebugDataWrapper wrapper)) return wrapper;

            Color color = GetColor(logType);
            DebugDataWrapper dataWrapper = new(logType, color, color);
            return dataWrapper;
        }

        void OnEnable()
        {
            ReInitializeDebugDataDictionary();
        }

        void OnValidate()
        {
            ReInitializeDebugDataDictionary();
        }

        private void ReInitializeDebugDataDictionary()
        {
            _debugDataDict.Clear();

            foreach (var wrapper in debugData)
            {
                _debugDataDict.TryAdd(wrapper.logType, wrapper);
            }
        }

        private Color GetColor(DebugLogType logType)
        {
            switch (logType)
            {
                case DebugLogType.Debug:
                    return Color.white;

                case DebugLogType.Warning:
                    return Color.yellowNice;

                case DebugLogType.Error:
                    return Color.red;

                case DebugLogType.None:
                default:
                    return Color.white;
            }
        }
    }
}