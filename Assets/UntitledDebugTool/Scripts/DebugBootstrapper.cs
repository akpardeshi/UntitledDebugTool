using UnityEngine;

namespace ModularDebugSystem.Debug
{
    public class DebugBootstrapper : MonoBehaviour
    {
        [SerializeField] private DebugManager manager;

        private void Awake()
        {
            ModularDebugger.Initialize(manager);
        }
    }
}