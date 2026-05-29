# WakeLock

Cross-platform .NET wake lock libraries for preventing system sleep and, optionally, display sleep.

## Packages

- `Capjan.WakeLock.Abstractions`: shared API (`IWakeLockService`, `WakeLockLevel`)
- `Capjan.WakeLock.Windows`: Windows implementation via `SetThreadExecutionState`
- `Capjan.WakeLock.MacOS`: macOS implementation via `caffeinate`
- `Capjan.WakeLock.Linux`: Linux implementation via `systemd-inhibit`

## Wake Lock Levels

- `WakeLockLevel.PreventSleep`: keep system awake
- `WakeLockLevel.PreventSleepAndDisplay`: keep system and display awake (platform-dependent behavior)

## Examples

### Windows

```csharp
using Capjan.WakeLock;

var wakeLock = new WindowsWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleep);
```

### macOS

```csharp
using Capjan.WakeLock;

var wakeLock = new MacOSWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleepAndDisplay);
```

### Linux

```csharp
using Capjan.WakeLock;

var wakeLock = new LinuxWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleepAndDisplay);
```

## Notes

- Keep the returned handle alive as long as you want the wake lock active.
- Dispose the handle to release one acquisition.
- Dispose the service to release all active acquisitions.

## Building

```bash
dotnet build WakeLock.slnx
dotnet pack WakeLock.slnx --configuration Release
```
