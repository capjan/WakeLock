# WakeLock

[![Build](https://github.com/capjan/WakeLock/actions/workflows/build.yml/badge.svg)](https://github.com/capjan/WakeLock/actions/workflows/build.yml)
[![Test](https://github.com/capjan/WakeLock/actions/workflows/test.yml/badge.svg)](https://github.com/capjan/WakeLock/actions/workflows/test.yml)
[![Abstractions](https://img.shields.io/nuget/v/Capjan.WakeLock.Abstractions?label=Abstractions)](https://www.nuget.org/packages/Capjan.WakeLock.Abstractions)
[![Windows](https://img.shields.io/nuget/v/Capjan.WakeLock.Windows?label=Windows)](https://www.nuget.org/packages/Capjan.WakeLock.Windows)
[![macOS](https://img.shields.io/nuget/v/Capjan.WakeLock.MacOS?label=macOS)](https://www.nuget.org/packages/Capjan.WakeLock.MacOS)
[![Linux](https://img.shields.io/nuget/v/Capjan.WakeLock.Linux?label=Linux)](https://www.nuget.org/packages/Capjan.WakeLock.Linux)
[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

Cross-platform .NET wake lock libraries for preventing system sleep and, optionally, display sleep.

![WakeLock Use Case (AI Generated)](./assets/wakelock-use-case-doodle.jpg)  
AI Generated: WakeLock Use Case Doodle

## Examples

<details>
<summary><strong>Windows</strong></summary>

Install:

```bash
dotnet add package Capjan.WakeLock.Windows
```

Use:

```csharp
using Capjan.WakeLock;

using var wakeLock = new WindowsWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleep);
```

</details>

<details>
<summary><strong>macOS</strong></summary>

Install:

```bash
dotnet add package Capjan.WakeLock.MacOS
```

Use:

```csharp
using Capjan.WakeLock;

using var wakeLock = new MacOSWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleep);
```

</details>

<details>
<summary><strong>Linux</strong></summary>

Install:

```bash
dotnet add package Capjan.WakeLock.Linux
```

Use:

```csharp
using Capjan.WakeLock;

using var wakeLock = new LinuxWakeLockService();
using var handle = wakeLock.Acquire(WakeLockLevel.PreventSleep);
```

</details>

> [!NOTE]
> - Keep the returned handle alive as long as you want the wake lock active.
> - Dispose the handle to release one acquisition.
> - Dispose the service to release all active acquisitions.
> - Use `WakeLockLevel.PreventSleepAndDisplay` if display sleep should be prevented too.
> - Use [`Capjan.WakeLock.Abstractions`](https://www.nuget.org/packages/Capjan.WakeLock.Abstractions) if you only want the shared API (`IWakeLockService`, `WakeLockLevel`).

## Samples

<details>
<summary><strong>.NET Tool</strong></summary>

A simple cross-platform dotnet tool that prevents your machine from sleeping until you press a key.

Source: [`samples/Capjan.WakeLock.Tool`](./samples/Capjan.WakeLock.Tool)

Install:

```bash
dotnet tool install --global Capjan.WakeLock.Tool
```

Run:

```bash
wakelock
```

Add --help to see all options.

</details>

## Building

```bash
dotnet build WakeLock.slnx
dotnet pack WakeLock.slnx --configuration Release
```

## Release Policy

- All packable projects in this repository (libraries and `Capjan.WakeLock.Tool`) must share one identical version.
- Versioning is derived from the Git tag via MinVer (`v<semver>` tags).
- Releases are published from one repository release so all NuGet artifacts stay aligned.
- CI enforces version consistency across all packable projects.

## License

Permissive MIT license (see [LICENSE](./LICENSE)) that allows commercial and open-source use with minimal obligations.
