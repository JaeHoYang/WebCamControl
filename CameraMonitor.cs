using Microsoft.Win32;

namespace WebCamControl;

internal sealed class CameraMonitor : IDisposable
{
    private readonly System.Timers.Timer _timer;
    private string _deviceName;
    private bool _wasEnabled;
    private bool _wasInUse;

    // 상태 변화가 이 횟수만큼 연속으로 확인되어야 이벤트를 발생시킴 (깜빡임 방지)
    private const int DebounceCount = 2;
    private int _enabledChangeTicks;
    private int _inUseChangeTicks;
    private bool _pendingEnabled;
    private bool _pendingInUse;

    internal event Action<string>? CameraEnabled;
    internal event Action<string>? CameraDisabled;
    internal event Action<string>? CameraInUse;
    internal event Action<string>? CameraReleased;

    internal CameraMonitor(string deviceName, double intervalMs = 3000)
    {
        _deviceName  = deviceName;
        _wasEnabled  = CameraController.IsEnabled(deviceName);
        _wasInUse    = IsCurrentlyInUse();
        _pendingEnabled = _wasEnabled;
        _pendingInUse   = _wasInUse;

        // AutoReset=false: 핸들러 완료 후 직접 재시작 → 타이머 중복 실행 방지
        _timer = new System.Timers.Timer(intervalMs) { AutoReset = false };
        _timer.Elapsed += OnTick;
    }

    internal void Start() => _timer.Start();
    internal void Stop()  => _timer.Stop();

    internal void UpdateDevice(string deviceName)
    {
        _deviceName     = deviceName;
        _wasEnabled     = CameraController.IsEnabled(deviceName);
        _pendingEnabled = _wasEnabled;
        _enabledChangeTicks = 0;
    }

    private void OnTick(object? sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            bool nowEnabled = CameraController.IsEnabled(_deviceName);
            bool nowInUse   = IsCurrentlyInUse();

            // 활성화 상태 디바운스
            if (nowEnabled != _pendingEnabled)
            {
                _pendingEnabled     = nowEnabled;
                _enabledChangeTicks = 1;
            }
            else if (nowEnabled != _wasEnabled)
            {
                _enabledChangeTicks++;
                if (_enabledChangeTicks >= DebounceCount)
                {
                    _wasEnabled         = nowEnabled;
                    _enabledChangeTicks = 0;
                    if (nowEnabled) CameraEnabled?.Invoke(_deviceName);
                    else            CameraDisabled?.Invoke(_deviceName);
                }
            }
            else
            {
                _enabledChangeTicks = 0;
            }

            // 사용 상태 디바운스
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
                    if (nowInUse) CameraInUse?.Invoke(_deviceName);
                    else          CameraReleased?.Invoke(_deviceName);
                }
            }
            else
            {
                _inUseChangeTicks = 0;
            }
        }
        finally
        {
            // 핸들러가 완전히 끝난 뒤 타이머 재시작
            _timer.Start();
        }
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
