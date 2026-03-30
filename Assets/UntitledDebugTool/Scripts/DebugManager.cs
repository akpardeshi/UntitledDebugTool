using System.Collections.Generic;
using UnityEngine;

namespace ModularDebugSystem.Debug
{
    public class DebugManager : MonoBehaviour
    {
        #region Variables

        private readonly Dictionary<string, DebugChannel> _debugChannels = new();

        [SerializeField] DebugChannel[] debugChannels;

        [SerializeField] DebugChannel defaultDebugChannel;

        private bool _isInitialized;

        #endregion


        #region Event Functions

        void Awake()
        {
            InitializeDebugChannels();
        }

        #endregion


        #region Event Handlers

        void InitializeDebugChannels()
        {
            if (_isInitialized) return;

            var color = ColorUtility.ToHtmlStringRGBA(Color.yellowNice);
            
            foreach (DebugChannel debugChannel in debugChannels)
            {
                if (debugChannel == null)
                {
                    UnityEngine.Debug.LogWarning("[DebugManager] Null entry found in debugChannels array.");
                    continue;
                }
                
                if (_debugChannels.TryAdd(debugChannel.ModuleName, debugChannel)) continue;

                UnityEngine.Debug.Log(
                    $"[<b><i><color=#{color}>Debug Channel</color></i></b>]: <color=#{color}>The <b><i>Debug Channel</i></b> with name <b><i>{debugChannel.name}</i></b> already exist</color>");
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