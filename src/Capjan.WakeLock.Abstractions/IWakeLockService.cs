namespace Capjan.WakeLock;

public interface IWakeLockService
{
    IWakeLockHandle Acquire(WakeLockLevel level = WakeLockLevel.PreventSleep);
}

public interface IWakeLockHandle : IDisposable
{
}

public enum WakeLockLevel
{
    PreventSleep,
    PreventSleepAndDisplay
}
