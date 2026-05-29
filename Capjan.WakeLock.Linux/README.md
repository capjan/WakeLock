# WakeLock.Linux

Linux implementation of `WakeLock.Abstractions`.

```csharp
using Capjan.WakeLock;

var wakeLock = new LinuxWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleepAndDisplay);
```

`PreventSleep` maps to `systemd-inhibit --what sleep`. `PreventSleepAndDisplay` maps to `systemd-inhibit --what sleep:idle`.
