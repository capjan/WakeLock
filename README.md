# WakeLock

Small .NET wake lock packages for preventing system sleep or display sleep.

## Packages

- `WakeLock.Abstractions`: shared API
- `WakeLock.Linux`: Linux implementation using `systemd-inhibit`
- `WakeLock.Windows`: Windows implementation using `SetThreadExecutionState`
- `WakeLock.MacOS`: macOS implementation using `caffeinate`

## Usage

```csharp
using Capjan.WakeLock;

var wakeLock = new WindowsWakeLockService();

using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleep);
```

On macOS, use `MacOSWakeLockService` instead.
On Linux, use `LinuxWakeLockService` instead.

Use `WakeLockLevel.PreventSleepAndDisplay` when the display should stay awake too.

## Building

```bash
dotnet build WakeLock.slnx
dotnet pack WakeLock.slnx --configuration Release
```
