using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Xunit;

namespace Capjan.WakeLock.MacOS.Tests;

[SupportedOSPlatform("macos")]
public sealed class MacOSWakeLockServiceTests
{
    [Fact]
    public void Acquire_And_Dispose_Handle_Does_Not_Throw()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return;

        using var service = new MacOSWakeLockService();
        using var handle = service.Acquire(WakeLockLevel.PreventSleep);
    }

    [Fact]
    public void Dispose_Handle_Twice_Does_Not_Throw()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return;

        using var service = new MacOSWakeLockService();
        var handle = service.Acquire(WakeLockLevel.PreventSleep);

        handle.Dispose();
        handle.Dispose();
    }

    [Fact]
    public void Multiple_Handles_Release_In_Any_Order_Does_Not_Throw()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return;

        using var service = new MacOSWakeLockService();
        var first = service.Acquire(WakeLockLevel.PreventSleep);
        var second = service.Acquire(WakeLockLevel.PreventSleepAndDisplay);

        first.Dispose();
        second.Dispose();
    }

    [Fact]
    public void Acquire_After_Dispose_Throws_ObjectDisposedException()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return;

        var service = new MacOSWakeLockService();
        service.Dispose();

        Assert.Throws<ObjectDisposedException>(() => service.Acquire(WakeLockLevel.PreventSleep));
    }
}
