# WakeLock.Abstractions

Small abstractions for acquiring process-scoped wake locks.

```csharp
using Capjan.WakeLock;

using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleep);
```

Use `WakeLockLevel.PreventSleepAndDisplay` when the display should stay awake too.
