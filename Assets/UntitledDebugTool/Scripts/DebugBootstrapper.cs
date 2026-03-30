using UnityEngine;

namespace ModularDebugSystem.Debug
{
    public class DebugBootstrapper : MonoBehaviour
    {
        [SerializeField] private DebugManager manager;

        private void Awake()
        {
            if (!manager)
            {
                string color = ColorUtility.ToHtmlStringRGBA(Color.red);

                UnityEngine.Debug.LogError(
                    $"[<b><i><color=#{color}>Debug Bootstrapper</color></i></b>]: <color=#{color}>The Debug Manager component is missing, please assign it</color>", gameObject);
                return;
            }

            ModularDebugger.Initialize(manager);
        }
    }
}