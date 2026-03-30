using UnityEngine;

namespace Systems.EventChannel.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameObjectEventChannel", menuName = "Systems/ScriptableEventChannels/GameObjectEventChannel")]
    public class GameObjectEventChannel : ScriptableEventChannelBase<GameObject>
    {

    }
}