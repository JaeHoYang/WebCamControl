using Microsoft.Win32;

namespace WebCamControl;

internal sealed class CameraMonitor : IDisposable
{
    private readonly System.Timers.Timer _timer;
    private string _deviceName;
    private bool _wasEnabled;
    private bool _wasInUse;
    private string _lastActiveApp = string.Empty;

    // 상태 변화가 이 횟수만큼 연속으로 확인되어야 이벤트를 발생시킴 (깜빡임 방지)
    private const int DebounceCount = 2;
    private int _enabledChangeTicks;
    private int _inUseChangeTicks;
    private bool _pendingEnabled;
    private bool _pendingInUse;

    internal event Action<string>?         CameraEnabled;
    internal event Action<string>?         CameraDisabled;
    internal event Action<string, string>? CameraInUse;     // (deviceName, appName)
    internal event Action<string, string>? CameraReleased;  // (deviceName, appName)

    internal CameraMonitor(string deviceName, double intervalMs = 3000)
    {
        _deviceName  = deviceName;
        _wasEnabled  = CameraController.IsEnabled(deviceName);
        string? app  = GetActiveApp();
        _wasInUse    = app != null;
        if (app != null) _lastActiveApp = app;
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
        _deviceName         = deviceName;
        _wasEnabled         = CameraController.IsEnabled(deviceName);
        _pendingEnabled     = _wasEnabled;
        _enabledChangeTicks = 0;
    }

    private void OnTick(object? sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            bool nowEnabled   = CameraController.IsEnabled(_deviceName);
            string? activeApp = GetActiveApp();
            bool nowInUse     = activeApp != null;
            if (nowInUse) _lastActiveApp = activeApp!;

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
                    if (nowInUse) CameraInUse?.Invoke(_deviceName, _lastActiveApp);
                    else          CameraReleased?.Invoke(_deviceName, _lastActiveApp);
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

    /// <summary>
    /// 현재 웹캠을 사용 중인 앱 이름을 반환합니다. 사용 중인 앱이 없으면 null을 반환합니다.
    /// Windows 레지스트리의 CapabilityAccessManager를 폴링하며,
    /// LastUsedTimeStop == 0 인 항목이 현재 카메라를 열고 있는 앱입니다.
    /// </summary>
    internal static string? GetActiveApp()
    {
        const string keyPath =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam";

        using var root = Registry.CurrentUser.OpenSubKey(keyPath);
        if (root == null) return null;

        foreach (var name in root.GetSubKeyNames())
        {
            using var sub = root.OpenSubKey(name);
            if (sub == null) continue;

            // 패키지 앱 (예: Microsoft.ZoomVideoMeetings_xxx)
            if (IsActiveEntry(sub))
                return FriendlyAppName(name);

            // 비패키지 앱 (NonPackaged 하위, 예: C:#Program Files#Zoom.exe)
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
        // 비패키지 앱: C:#Program Files#Zoom#bin#Zoom.exe → Zoom.exe
        int lastHash = keyName.LastIndexOf('#');
        if (lastHash >= 0)
            return keyName[(lastHash + 1)..];

        // 패키지 앱: Microsoft.ZoomVideoMeetings_abcxyz → Microsoft.ZoomVideoMeetings
        int underscore = keyName.LastIndexOf('_');
        if (underscore > 0)
            return keyName[..underscore];

        return keyName;
    }

    public void Dispose() => _timer.Dispose();
}
