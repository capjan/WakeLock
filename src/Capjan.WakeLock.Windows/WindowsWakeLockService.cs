namespace Capjan.WakeLock;

public sealed class WindowsWakeLockService : IWakeLockService, IDisposable
{
    private readonly Lock _gate = new();
    private int _preventSleepCount;
    private int _preventSleepAndDisplayCount;
    private bool _disposed;

    [Flags]
    private enum ExecutionState : uint
    {
        ES_CONTINUOUS = 0x80000000,
        ES_SYSTEM_REQUIRED = 0x00000001,
        ES_DISPLAY_REQUIRED = 0x00000002
    }

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

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
        var executionState =
            ExecutionState.ES_CONTINUOUS |
            ExecutionState.ES_SYSTEM_REQUIRED;

        if (level == WakeLockLevel.PreventSleepAndDisplay)
            executionState |= ExecutionState.ES_DISPLAY_REQUIRED;

        var result = SetThreadExecutionState(executionState);

        if (result == 0)
            throw new InvalidOperationException("Could not acquire Windows wake lock.");
    }

    private void ReleasePlatform()
    {
        SetThreadExecutionState(ExecutionState.ES_CONTINUOUS);
    }

    private sealed class WakeLockHandle(WindowsWakeLockService service, WakeLockLevel level) : IWakeLockHandle
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
