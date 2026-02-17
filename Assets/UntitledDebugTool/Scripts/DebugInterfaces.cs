using EventChannel.ScriptableEvents;

namespace ModularDebugSystem.Debug
{
    public interface IDebuggerable
    {
        DebugChannel DebugChannel { get; }

        void DebugLog(string msg);
        void DebugLogWarning(string msg);
        void DebugLogError(string msg);
    }

    public interface IDebuggable
    {
        DebugChannel DebugChannel { get; }
        
        RegisterDebugChannel RegisterDebugChannel { get; }

        void RegisterSelf();
        void UnregisterSelf();
    }
}