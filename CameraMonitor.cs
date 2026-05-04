using Microsoft.Win32;

namespace WebCamControl;

internal sealed class CameraMonitor : IDisposable
{
    private readonly System.Timers.Timer _timer;
    private string _deviceName;
    private bool _wasEnabled;
    private bool _wasInUse;

    internal event Action<string>? CameraEnabled;
    internal event Action<string>? CameraDisabled;
    internal event Action<string>? CameraInUse;
    internal event Action<string>? CameraReleased;

    internal CameraMonitor(string deviceName, double intervalMs = 3000)
    {
        _deviceName = deviceName;
        _wasEnabled = CameraController.IsEnabled(deviceName);
        _wasInUse   = IsCurrentlyInUse();

        _timer = new System.Timers.Timer(intervalMs) { AutoReset = true };
        _timer.Elapsed += OnTick;
    }

    internal void Start() => _timer.Start();
    internal void Stop()  => _timer.Stop();

    internal void UpdateDevice(string deviceName)
    {
        _deviceName = deviceName;
        _wasEnabled = CameraController.IsEnabled(deviceName);
    }

    private void OnTick(object? sender, System.Timers.ElapsedEventArgs e)
    {
        bool nowEnabled = CameraController.IsEnabled(_deviceName);
        bool nowInUse   = IsCurrentlyInUse();

        if (!_wasEnabled && nowEnabled)
            CameraEnabled?.Invoke(_deviceName);

        if (_wasEnabled && !nowEnabled)
            CameraDisabled?.Invoke(_deviceName);

        if (!_wasInUse && nowInUse)
            CameraInUse?.Invoke(_deviceName);

        if (_wasInUse && !nowInUse)
            CameraReleased?.Invoke(_deviceName);

        _wasEnabled = nowEnabled;
        _wasInUse   = nowInUse;
    }

    /// <summary>
    /// Windows 레지스트리의 카메라 동의 저장소를 폴링하여 현재 사용 중인 앱이 있는지 확인합니다.
    /// LastUsedTimeStop == 0 이면 해당 앱이 카메라를 열고 있는 상태입니다.
    /// </summary>
    internal static bool IsCurrentlyInUse()
    {
        const string keyPath =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam";

        using var root = Registry.CurrentUser.OpenSubKey(keyPath);
        if (root == null) return false;

        return ScanForActiveUse(root);
    }

    private static bool ScanForActiveUse(RegistryKey key)
    {
        if (IsActiveEntry(key)) return true;

        foreach (var name in key.GetSubKeyNames())
        {
            using var sub = key.OpenSubKey(name);
            if (sub == null) continue;

            if (IsActiveEntry(sub)) return true;

            foreach (var nested in sub.GetSubKeyNames())
            {
                using var nestedKey = sub.OpenSubKey(nested);
                if (nestedKey != null && IsActiveEntry(nestedKey)) return true;
            }
        }

        return false;
    }

    private static bool IsActiveEntry(RegistryKey key)
    {
        var stop = key.GetValue("LastUsedTimeStop");
        return stop is long val && val == 0;
    }

    public void Dispose() => _timer.Dispose();
}
