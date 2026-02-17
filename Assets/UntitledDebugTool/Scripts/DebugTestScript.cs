using EventChannel.ScriptableEvents;
using UnityEngine;

namespace ModularDebugSystem.Debug
{
    public class DebugTestScript: MonoBehaviour
    {
        #region Event Functions
        
        void Start()
        {
            Invoke(nameof(TestLog), 0.25f);
        }
        
        #endregion

        void TestLog()
        {
            ModularDebugger.Log($"Debug System 1", "Hello World Log");
            ModularDebugger.LogWarning($"Debug System 1", "Hello World Warning");
            ModularDebugger.LogError($"Debug System 1", "Hello World Error");
            
            ModularDebugger.Log($"Debug System 2", "Hello World Log");
            ModularDebugger.LogWarning($"Debug System 2", "Hello World Warning");
            ModularDebugger.LogError($"Debug System 2", "Hello World Error");
        }
        
    }
}