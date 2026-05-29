# WakeLock.MacOS

macOS implementation of `WakeLock.Abstractions`.

```csharp
using Capjan.WakeLock;

var wakeLock = new MacOSWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleepAndDisplay);
```

`PreventSleep` runs `caffeinate -i`. `PreventSleepAndDisplay` runs `caffeinate -i -d`.
