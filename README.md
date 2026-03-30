# Modular Debug System

A channelled logging system for Unity that lets you enable or silence debug output per-module from the Inspector — without touching code. Built as the foundation for a full runtime debug console.

> **Unity version:** 6000.3.8f1 · **Language:** C# · **Status:** Phase 1 — portfolio showcase

---

## The problem it solves

In any active Unity project the console fills up fast. Logs from audio, UI, gameplay, and networking mix together — and when something breaks you are scrolling through hundreds of messages trying to find the relevant one.

The standard fix is commenting out `Debug.Log` calls or wrapping them in preprocessor directives, both of which create maintenance overhead and get forgotten. This system solves it differently: each module owns a `DebugChannel` ScriptableObject asset. Flip `canBeDebugged` off in the Inspector and that module goes silent — no code changes, no recompilation, no forgotten logs left in a build.

---

## Architecture

The system is built around a static façade that decouples every consumer from the channel registry. A consumer calls `ModularDebugger.Log("ModuleName", "message")` and knows nothing about how channels are stored or resolved.

```
DebugBootstrapper (MonoBehaviour)
        │
        │  Initialize(manager)
        ▼
ModularDebugger (static façade)          ◄──── Any MonoBehaviour calls Log / LogWarning / LogError
        │
        │  GetDebugChannel(name)
        ▼
DebugManager (MonoBehaviour)
        │
        │  Dictionary<string, DebugChannel> lookup
        ▼
DebugChannel (ScriptableObject)          ──── per-module: canBeDebugged, colors, logType config
        │
        ▼
DebugDataWrapper (struct)                ──── logType · messageColor · headingColor
        │
        ▼
UnityEngine.Debug                        ──── final output, or fallback if manager is missing
```

### Key design decisions

**Static façade pattern** — `ModularDebugger` is a static class. Any script calls it directly with zero setup. This mirrors the Unity `Debug` API intentionally — adoption friction is near zero.

**ScriptableObject channels** — each module references a `DebugChannel` asset rather than a string constant registered at runtime. Channels are configured in the Inspector, survive scene transitions, and are independently toggleable without touching code.

**Graceful fallback** — if `DebugBootstrapper` is missing or initialisation has not run yet, `ModularDebugger` falls back to `UnityEngine.Debug` directly. Log type is preserved in the fallback — errors stay errors, warnings stay warnings.

**Domain reload safety** — `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]` resets the static `_debugManager` reference on every domain reload. The bootstrapper reassigns it at the start of each Play Mode session.

---

## Project structure

```
ModularDebugSystem/
├── Debug/
│   ├── ModularDebugger.cs          Static façade — public API
│   ├── DebugManager.cs             Channel registry (MonoBehaviour)
│   ├── DebugBootstrapper.cs        Scene entry point — wires façade to manager
│   ├── DebugChannel.cs             Per-module ScriptableObject asset
│   ├── DebugEnums.cs               DebugLogType enum
│   ├── DebugStructs.cs             DebugDataWrapper struct
│   └── DebugTestScript.cs          Usage example
└── EventChannel/
    └── RegisterDebugChannel.cs     Optional SO event channel integration
                                    (requires ScriptableEventChannelBase<T>)
```

---

## Setup

**1. Add to your scene**

Create a GameObject and attach both `DebugManager` and `DebugBootstrapper`. Assign the `DebugManager` reference on `DebugBootstrapper` in the Inspector.

**2. Create a DebugChannel asset**

Right-click in the Project window:
`ModularDebugSystem > DebugChannel`

Set the `Module Name` field — this is the string key used in log calls.

**3. Register the channel**

Assign the `DebugChannel` asset to the `DebugManager`'s channel list in the Inspector.

**4. Log from any script**

```csharp
ModularDebugger.Log("AudioSystem", "Clip started playing");
ModularDebugger.LogWarning("AudioSystem", "Clip reference is null — using fallback");
ModularDebugger.LogError("AudioSystem", "AudioSource is missing");
```

**5. Silence a module**

Open the `DebugChannel` asset for that module and uncheck `Can Be Debugged`. No code changes required.

---

## How the channel lookup works

```csharp
// ModularDebugger.LogInternal — simplified
if (_debugManager)
{
    var channel = _debugManager.GetDebugChannel(channelName);

    if (!channel.CanBeDebugged) return;          // silenced — exit early

    var wrapper = channel.GetDebugDataWrapper(logType);
    // format with channel colors and log via UnityEngine.Debug
    return;
}

// fallback — manager not initialised
// log type preserved: Warning → LogWarning, Error → LogError
```

If a channel name is not registered, `DebugManager` returns the `defaultDebugChannel` rather than throwing — the log still appears, just with default formatting.

---

## DebugChannel configuration

Each `DebugChannel` ScriptableObject exposes:

| Field | Type | Description |
|---|---|---|
| `Module Name` | string | Key used in `ModularDebugger.Log(channelName, ...)` |
| `Can Be Debugged` | bool | Toggle off to silence this module at runtime |
| `Debug Data` | DebugDataWrapper[] | Per-logType color configuration |

`DebugDataWrapper` holds a `DebugLogType`, a `messageColor`, and a `headingColor`. If no entry exists for a given log type, the channel falls back to sensible defaults — white for debug, yellow for warning, red for error.

Duplicate `logType` entries in the array are detected and warned in the console during `OnValidate` — the second entry is ignored.

---

## Dependencies

**Core system** — no external dependencies. Requires Unity 6000.3.8f1 or compatible.

**RegisterDebugChannel** — the `RegisterDebugChannel.cs` file requires `ScriptableEventChannelBase<T>` from a separate event channel system. This file is optional and only needed if you are wiring debug channel registration through a ScriptableObject event architecture. Remove it safely if unused.

---

## Known limitations

This is a Phase 1 implementation — a structured logging infrastructure, not yet a full debug tool.

What it does not yet include:

- Runtime overlay console (in-game log viewer without the Editor)
- Build stripping — `ModularDebugger` calls are not stripped from release builds. A `[System.Diagnostics.Conditional("UNITY_EDITOR")]` or `#if DEBUG` wrapper is planned for Phase 2 to eliminate any runtime overhead in shipped builds
- Log history buffer — no queryable record of past log entries
- Pause-on-error flag — no automatic `Debug.Break()` on `LogError` calls

---

## Roadmap

**Phase 2**
- Runtime overlay console window — see logs on device without the Editor connected
- Build stripping via `[Conditional]` attribute — zero cost in release builds
- Log history buffer with channel-filtered query API
- Pause-on-error toggle per channel

**Phase 3**
- UPM package release
- Unity Asset Store submission

---

## Why I built this

In any active project the console fills up with logs from every system simultaneously. I came across Console Pro, which solves this by letting you toggle logs per module from the Editor. That was the right mental model — logs should be configurable per system, not hardcoded on or off.

I built my own version to understand the architecture behind that kind of system and to have something I could extend. The core design question was how to make each module's log configuration independent without tight coupling to a central manager. ScriptableObject channels solve it — each module references an asset, the manager does a dictionary lookup, and new modules are added without modifying any existing code.

Building it surfaced the real edge cases: domain reload clearing static references, initialisation ordering between the bootstrapper and manager, and making sure the fallback path preserved log severity rather than silently downgrading errors to plain log messages.

---
