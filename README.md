# WakeLock

[![CI](https://github.com/capjan/WakeLock/actions/workflows/ci.yml/badge.svg)](https://github.com/capjan/WakeLock/actions/workflows/ci.yml)
[![Abstractions](https://img.shields.io/nuget/v/Capjan.WakeLock.Abstractions?label=Abstractions)](https://www.nuget.org/packages/Capjan.WakeLock.Abstractions)
[![Windows](https://img.shields.io/nuget/v/Capjan.WakeLock.Windows?label=Windows)](https://www.nuget.org/packages/Capjan.WakeLock.Windows)
[![macOS](https://img.shields.io/nuget/v/Capjan.WakeLock.MacOS?label=macOS)](https://www.nuget.org/packages/Capjan.WakeLock.MacOS)
[![Linux](https://img.shields.io/nuget/v/Capjan.WakeLock.Linux?label=Linux)](https://www.nuget.org/packages/Capjan.WakeLock.Linux)
[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

Cross-platform .NET wake lock libraries for preventing system sleep and, optionally, display sleep.

## Packages

- [`Capjan.WakeLock.Abstractions`](https://www.nuget.org/packages/Capjan.WakeLock.Abstractions): shared API (`IWakeLockService`, `WakeLockLevel`)
- [`Capjan.WakeLock.Windows`](https://www.nuget.org/packages/Capjan.WakeLock.Windows): Windows implementation via `SetThreadExecutionState`
- [`Capjan.WakeLock.MacOS`](https://www.nuget.org/packages/Capjan.WakeLock.MacOS): macOS implementation via `caffeinate`
- [`Capjan.WakeLock.Linux`](https://www.nuget.org/packages/Capjan.WakeLock.Linux): Linux implementation via `systemd-inhibit`

## Wake Lock Levels

- `WakeLockLevel.PreventSleep`: keep system awake
- `WakeLockLevel.PreventSleepAndDisplay`: keep system and display awake (platform-dependent behavior)

## Examples

<details>
<summary><strong>Windows</strong></summary>

```csharp
using Capjan.WakeLock;

using var wakeLock = new WindowsWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleep);
```

</details>

<details>
<summary><strong>macOS</strong></summary>

```csharp
using Capjan.WakeLock;

using var wakeLock = new MacOSWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleep);
```

</details>

<details>
<summary><strong>Linux</strong></summary>

```csharp
using Capjan.WakeLock;

using var wakeLock = new LinuxWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleep);
```

</details>

## Notes

- Keep the returned handle alive as long as you want the wake lock active.
- Dispose the handle to release one acquisition.
- Dispose the service to release all active acquisitions.
- Use `WakeLockLevel.PreventSleepAndDisplay` if display sleep should be prevented too.

## Building

```bash
dotnet build WakeLock.slnx
dotnet pack WakeLock.slnx --configuration Release
```
