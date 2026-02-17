using System.Collections.Generic;
using UnityEngine;

namespace ModularDebugSystem.Debug
{
    [CreateAssetMenu(fileName = "ModularDebugChannel", menuName = "ModularDebugSystem/DebugChannel")]
    public class DebugChannel : ScriptableObject
    {
        [SerializeField] private bool canBeDebugged;
        [SerializeField] private string moduleName;
        [SerializeField] DebugDataWrapper [] debugData;

        private readonly Dictionary<DebugLogType, Color> _debugDataDict = new();
        
        public string ModuleName => moduleName;
        public bool CanBeDebugged => canBeDebugged;
        
        public Dictionary<DebugLogType, Color> DebugDataDict => _debugDataDict;

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
                _debugDataDict.Add(wrapper.logType, wrapper.LOGColor);
            }
        }
    }
}