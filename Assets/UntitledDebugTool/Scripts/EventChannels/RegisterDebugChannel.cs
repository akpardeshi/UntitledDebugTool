using ModularDebugSystem.Debug;
using Systems.EventChannel.Scripts.ScriptableObjects;
using UnityEngine;

namespace EventChannel.ScriptableEvents
{
    [CreateAssetMenu(fileName = "RegisterDebugChannel",
        menuName = "Systems/ScriptableEventChannels/RegisterDebugChannel")]
    public class RegisterDebugChannel : ScriptableEventChannelBase<DebugChannel>
    {
        
    }
}