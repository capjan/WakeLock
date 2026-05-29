using System.Runtime.InteropServices;
using Xunit;

namespace Capjan.WakeLock.Windows.Tests;

public sealed class WindowsWakeLockServiceTests
{
    [Fact]
    public void Acquire_And_Dispose_Handle_Does_Not_Throw_On_Windows()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        using var service = new WindowsWakeLockService();
        using var handle = service.Acquire(WakeLockLevel.PreventSleep);
    }

    [Fact]
    public void Dispose_Handle_Twice_Does_Not_Throw_On_Windows()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        using var service = new WindowsWakeLockService();
        var handle = service.Acquire(WakeLockLevel.PreventSleep);

        handle.Dispose();
        handle.Dispose();
    }

    [Fact]
    public void Acquire_After_Dispose_Throws_ObjectDisposedException()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        var service = new WindowsWakeLockService();
        service.Dispose();

        Assert.Throws<ObjectDisposedException>(() => service.Acquire(WakeLockLevel.PreventSleep));
    }

    [Fact]
    public void Acquire_Throws_DllNotFoundException_Outside_Windows()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        var service = new WindowsWakeLockService();

        Assert.Throws<DllNotFoundException>(() => service.Acquire(WakeLockLevel.PreventSleep));
    }
}
