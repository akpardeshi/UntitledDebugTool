using System.Collections.Generic;
using UnityEngine;

namespace ModularDebugSystem.Debug
{
    public class DebugManager : MonoBehaviour
    {
        #region Variables
        
        Dictionary<string, DebugChannel> _debugChannels = new();
        
        [SerializeField] DebugChannel []  debugChannels;
        
        [SerializeField] DebugChannel defaultDebugChannel;
        
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
            DebugChannel debugChannel = _debugChannels.GetValueOrDefault(channelName);
            return debugChannel == null ? defaultDebugChannel : debugChannel;
        }
        
        #endregion
    }
}