# WakeLock.Windows

Windows implementation of `WakeLock.Abstractions`.

```csharp
using Capjan.WakeLock;

var wakeLock = new WindowsWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleepAndDisplay);
```

`PreventSleep` maps to `ES_SYSTEM_REQUIRED`. `PreventSleepAndDisplay` also uses `ES_DISPLAY_REQUIRED`.
