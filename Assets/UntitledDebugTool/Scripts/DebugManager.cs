using System.Collections.Generic;
using UnityEngine;

namespace ModularDebugSystem.Debug
{
    public class DebugManager : MonoBehaviour
    {
        #region Variables
        
        Dictionary<string, DebugChannel> _debugChannels = new();
        
        [SerializeField] DebugChannel []  debugChannels;
        
        private bool _isInitialized;
        
        #endregion

        
        #region Event Functions
        
        void OnEnable()
        {
            InitializeDebugChannels();
        }
        
        #endregion
        
        
        #region Event Handlers

        void InitializeDebugChannels()
        {
            if(_isInitialized) return; 
            
            foreach (DebugChannel debugChannel in debugChannels)
            {
                _debugChannels.Add(debugChannel.ModuleName, debugChannel);
            }

            _isInitialized = true;
        }
        
        #endregion
        
        
        #region Getter Methods

        public DebugChannel GetDebugChannel(string channelName)
        {
            return _debugChannels.GetValueOrDefault(channelName);
        }
        
        #endregion
    }
}