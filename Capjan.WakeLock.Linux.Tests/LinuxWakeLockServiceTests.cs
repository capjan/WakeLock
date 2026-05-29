using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Xunit;

namespace Capjan.WakeLock.Linux.Tests;

public sealed class LinuxWakeLockServiceTests
{
    [Fact]
    [SupportedOSPlatform("linux")]
    public void Acquire_And_Dispose_Handle_Does_Not_Throw_On_Linux_When_SystemdInhibit_Exists()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return;

        if (!File.Exists("/usr/bin/systemd-inhibit"))
            return;

        using var service = new LinuxWakeLockService();
        using var handle = service.Acquire(WakeLockLevel.PreventSleep);
    }

    [Fact]
    [SupportedOSPlatform("linux")]
    public void Dispose_Handle_Twice_Does_Not_Throw_On_Linux_When_SystemdInhibit_Exists()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return;

        if (!File.Exists("/usr/bin/systemd-inhibit"))
            return;

        using var service = new LinuxWakeLockService();
        var handle = service.Acquire(WakeLockLevel.PreventSleep);

        handle.Dispose();
        handle.Dispose();
    }

    [Fact]
    [SupportedOSPlatform("linux")]
    public void Acquire_After_Dispose_Throws_ObjectDisposedException()
    {
        var service = new LinuxWakeLockService();
        service.Dispose();

        Assert.Throws<ObjectDisposedException>(() => service.Acquire(WakeLockLevel.PreventSleep));
    }

}
