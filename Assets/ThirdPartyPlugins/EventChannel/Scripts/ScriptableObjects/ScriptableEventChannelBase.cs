using System;
using UnityEngine;

namespace Systems.EventChannel.Scripts.ScriptableObjects
{
    public abstract class ScriptableEventChannelBase<T> : ScriptableObject
    {
        public Action<T> OnEventRaised;

        public virtual void RaiseEvent(T eventData)
        {
            OnEventRaised?.Invoke(eventData);
        }
    }
}