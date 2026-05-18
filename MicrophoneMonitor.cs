using Microsoft.Win32;

namespace WebCamControl;

internal sealed class MicrophoneMonitor : IDisposable
{
    private readonly System.Timers.Timer _timer;
    private bool   _wasInUse;
    private string _lastActiveApp = string.Empty;

    private const int DebounceCount = 2;
    private int  _inUseChangeTicks;
    private bool _pendingInUse;

    internal event Action<string>? MicrophoneInUse;    // appName
    internal event Action<string>? MicrophoneReleased; // appName

    internal MicrophoneMonitor(double intervalMs = 3000)
    {
        string? app   = GetActiveApp();
        _wasInUse     = app != null;
        if (app != null) _lastActiveApp = app;
        _pendingInUse = _wasInUse;

        _timer = new System.Timers.Timer(intervalMs) { AutoReset = false };
        _timer.Elapsed += OnTick;
    }

    internal void Start() => _timer.Start();
    internal void Stop()  => _timer.Stop();

    private void OnTick(object? sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            string? activeApp = GetActiveApp();
            bool nowInUse     = activeApp != null;
            if (nowInUse) _lastActiveApp = activeApp!;

            if (nowInUse != _pendingInUse)
            {
                _pendingInUse     = nowInUse;
                _inUseChangeTicks = 1;
            }
            else if (nowInUse != _wasInUse)
            {
                _inUseChangeTicks++;
                if (_inUseChangeTicks >= DebounceCount)
                {
                    _wasInUse         = nowInUse;
                    _inUseChangeTicks = 0;
                    if (nowInUse) MicrophoneInUse?.Invoke(_lastActiveApp);
                    else          MicrophoneReleased?.Invoke(_lastActiveApp);
                }
            }
            else
            {
                _inUseChangeTicks = 0;
            }
        }
        finally
        {
            _timer.Start();
        }
    }

    internal static string? GetActiveApp()
    {
        const string keyPath =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone";

        using var root = Registry.CurrentUser.OpenSubKey(keyPath);
        if (root == null) return null;

        foreach (var name in root.GetSubKeyNames())
        {
            using var sub = root.OpenSubKey(name);
            if (sub == null) continue;

            if (IsActiveEntry(sub))
                return FriendlyAppName(name);

            foreach (var nested in sub.GetSubKeyNames())
            {
                using var nestedKey = sub.OpenSubKey(nested);
                if (nestedKey != null && IsActiveEntry(nestedKey))
                    return FriendlyAppName(nested);
            }
        }
        return null;
    }

    private static bool IsActiveEntry(RegistryKey key)
    {
        var stop = key.GetValue("LastUsedTimeStop");
        return stop is long val && val == 0;
    }

    private static string FriendlyAppName(string keyName)
    {
        int lastHash = keyName.LastIndexOf('#');
        if (lastHash >= 0) return keyName[(lastHash + 1)..];
        int underscore = keyName.LastIndexOf('_');
        if (underscore > 0) return keyName[..underscore];
        return keyName;
    }

    public void Dispose() => _timer.Dispose();
}
