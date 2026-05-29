namespace Capjan.WakeLock;

public sealed class LinuxWakeLockService : IWakeLockService, IDisposable
{
    private readonly Lock _gate = new();
    private System.Diagnostics.Process? _process;
    private WakeLockLevel? _level;
    private int _preventSleepCount;
    private int _preventSleepAndDisplayCount;
    private bool _disposed;

    public IWakeLockHandle Acquire(WakeLockLevel level)
    {
        lock (_gate)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            UpdateCounts(level, 1);
            ApplyCurrentLevel();
        }

        return new WakeLockHandle(this, level);
    }

    public void Dispose()
    {
        lock (_gate)
        {
            if (_disposed) return;

            _disposed = true;
            _preventSleepCount = 0;
            _preventSleepAndDisplayCount = 0;
            ReleasePlatform();
        }
    }

    private void Release(WakeLockLevel level)
    {
        lock (_gate)
        {
            if (_disposed) return;

            UpdateCounts(level, -1);
            ApplyCurrentLevel();
        }
    }

    private void UpdateCounts(WakeLockLevel level, int delta)
    {
        switch (level)
        {
            case WakeLockLevel.PreventSleep:
                _preventSleepCount += delta;
                break;
            case WakeLockLevel.PreventSleepAndDisplay:
                _preventSleepAndDisplayCount += delta;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(level));
        }
    }

    private void ApplyCurrentLevel()
    {
        var level =
            _preventSleepAndDisplayCount > 0 ? WakeLockLevel.PreventSleepAndDisplay :
            _preventSleepCount > 0 ? WakeLockLevel.PreventSleep :
            (WakeLockLevel?)null;

        if (level is { } heldLevel)
            AcquirePlatform(heldLevel);
        else
            ReleasePlatform();
    }

    private void AcquirePlatform(WakeLockLevel level)
    {
        if (_process is { HasExited: false } && _level == level)
            return;

        ReleasePlatform();

        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "/usr/bin/systemd-inhibit",
            UseShellExecute = false,
            CreateNoWindow = true
        };

        startInfo.ArgumentList.Add("--what");
        startInfo.ArgumentList.Add(level == WakeLockLevel.PreventSleepAndDisplay ? "sleep:idle" : "sleep");
        startInfo.ArgumentList.Add("--why");
        startInfo.ArgumentList.Add("Capjan.WakeLock active");
        startInfo.ArgumentList.Add("--mode");
        startInfo.ArgumentList.Add("block");
        startInfo.ArgumentList.Add("sleep");
        startInfo.ArgumentList.Add("infinity");

        _process = System.Diagnostics.Process.Start(startInfo);

        if (_process is null || _process.HasExited)
            throw new InvalidOperationException("Could not start systemd-inhibit.");

        _level = level;
    }

    private void ReleasePlatform()
    {
        if (_process is { HasExited: false })
        {
            _process.Kill();
            _process.Dispose();
        }

        _process = null;
        _level = null;
    }

    private sealed class WakeLockHandle(LinuxWakeLockService service, WakeLockLevel level) : IWakeLockHandle
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            service.Release(level);
        }
    }
}
