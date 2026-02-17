using UnityEngine;

namespace ModularDebugSystem.Debug
{
    public class DebugBootstrapper : MonoBehaviour
    {
        [SerializeField] private DebugManager manager;

        private void Awake()
        {
            UnityEngine.Debug.Log($"manager: {manager == null}");
            ModularDebugger.Initialize(manager);
        }
    }
}