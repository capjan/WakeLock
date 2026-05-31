using Capjan.WakeLock;
using System.Reflection;

var parsed = ParseArguments(args);

if (parsed.ShowHelp)
{
    PrintHelp();
    return;
}

if (parsed.ShowVersion)
{
    PrintVersion();
    return;
}

if (parsed.UnknownOptions.Count > 0)
{
    Console.Error.WriteLine($"Unknown option(s): {string.Join(", ", parsed.UnknownOptions)}");
    Console.Error.WriteLine();
    PrintHelp();
    Environment.ExitCode = 1;
    return;
}

var keepDisplayAwake = parsed.KeepDisplayAwake;

var wakeLockLevel = keepDisplayAwake
    ? WakeLockLevel.PreventSleepAndDisplay
    : WakeLockLevel.PreventSleep;

var wakeLockService = CreateWakeLockService();
using var handle = wakeLockService.Acquire(wakeLockLevel);

Console.WriteLine($"Wake lock acquired at {DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss zzz}.");
Console.WriteLine($"Mode: {wakeLockLevel}.");
Console.WriteLine("Press any key to release the wake lock.");
Console.ReadKey(intercept: true);

Console.WriteLine($"Wake lock released at {DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss zzz}.");

static ParsedArguments ParseArguments(string[] args)
{
    var keepDisplayAwake = false;
    var showHelp = false;
    var showVersion = false;
    var unknownOptions = new List<string>();

    foreach (var arg in args)
    {
        if (string.Equals(arg, "--display", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(arg, "-d", StringComparison.OrdinalIgnoreCase))
        {
            keepDisplayAwake = true;
            continue;
        }

        if (string.Equals(arg, "--help", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(arg, "-h", StringComparison.OrdinalIgnoreCase))
        {
            showHelp = true;
            continue;
        }

        if (string.Equals(arg, "--version", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(arg, "-v", StringComparison.OrdinalIgnoreCase))
        {
            showVersion = true;
            continue;
        }

        unknownOptions.Add(arg);
    }

    return new ParsedArguments(keepDisplayAwake, showHelp, showVersion, unknownOptions);
}

static void PrintHelp()
{
    Console.WriteLine("Usage: wakelock [options]");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  -d, --display   Keep both system and display awake.");
    Console.WriteLine("  -h, --help      Show help information.");
    Console.WriteLine("  -v, --version   Show version information.");
}

static void PrintVersion()
{
    var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "unknown";
    Console.WriteLine($"Capjan.WakeLock.Tool {version}");
}

static IWakeLockService CreateWakeLockService()
    => true switch
    {
        _ when OperatingSystem.IsWindows() => new WindowsWakeLockService(),
        _ when OperatingSystem.IsMacOS() => new MacOSWakeLockService(),
        _ when OperatingSystem.IsLinux() => new LinuxWakeLockService(),
        _ => throw new PlatformNotSupportedException("WakeLock tool supports only Windows, macOS, and Linux.")
    };

file sealed record ParsedArguments(
    bool KeepDisplayAwake,
    bool ShowHelp,
    bool ShowVersion,
    IReadOnlyList<string> UnknownOptions);
