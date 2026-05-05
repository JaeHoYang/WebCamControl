namespace WebCamControl;

public partial class MainForm : Form
{
    private bool _cameraEnabled;
    private bool _micMuted;

    private readonly MonitorSettings  _settings;
    private readonly TelegramNotifier _telegram;
    private readonly KakaoNotifier    _kakao;
    private CameraMonitor? _monitor;

    private string SelectedCamera => cmbCamera.SelectedItem as string ?? string.Empty;
    private string SelectedMic    => cmbMic.SelectedItem    as string ?? string.Empty;

    public MainForm(bool startMinimized = false)
    {
        InitializeComponent();

        _settings = MonitorSettings.Load();
        _telegram = new TelegramNotifier { BotToken = _settings.BotToken, ChatId = _settings.ChatId };
        _kakao    = new KakaoNotifier
        {
            RestApiKey   = _settings.KakaoRestApiKey,
            AccessToken  = _settings.KakaoAccessToken,
            RefreshToken = _settings.KakaoRefreshToken,
        };

        LoadDeviceLists();

        if (!_settings.FirstRunDone)
            HandleFirstRun();

        if (_settings.MonitorEnabled && btnVideo.Enabled)
            StartMonitoring();

        UpdateMonitorButton();

        if (startMinimized && _settings.MonitorEnabled)
            Load += (_, _) => HideToTray();
    }

    // ── Device lists ─────────────────────────────────────────────────────────

    private void LoadDeviceLists()
    {
        var cameras = CameraController.GetAllCameraDeviceNames();
        cmbCamera.Items.Clear();
        if (cameras.Count == 0)
        {
            cmbCamera.Items.Add("(연결된 카메라 없음)");
            btnVideo.Enabled                  = false;
            btnMonitorToggle.Enabled          = false;
            btnNotificationSettings.Enabled   = false;
        }
        else
        {
            cameras.ForEach(c => cmbCamera.Items.Add(c));
            btnVideo.Enabled                  = true;
            btnMonitorToggle.Enabled          = true;
            btnNotificationSettings.Enabled   = true;
        }
        cmbCamera.SelectedIndex = 0;

        var mics = MicController.GetAllMicDeviceNames();
        cmbMic.Items.Clear();
        if (mics.Count == 0)
        {
            cmbMic.Items.Add("(연결된 마이크 없음)");
            btnMic.Enabled = false;
        }
        else
        {
            mics.ForEach(m => cmbMic.Items.Add(m));
            btnMic.Enabled = true;
        }
        cmbMic.SelectedIndex = 0;

        RefreshButtonStates();
    }

    private void RefreshButtonStates()
    {
        if (btnVideo.Enabled)
        {
            _cameraEnabled = CameraController.IsEnabled(SelectedCamera);
            UpdateVideoButton();
        }

        if (btnMic.Enabled)
        {
            _micMuted = MicController.IsMuted(SelectedMic);
            UpdateMicButton();
        }
    }

    // ── Button state updates ─────────────────────────────────────────────────

    private void UpdateVideoButton()
    {
        if (_cameraEnabled)
        {
            btnVideo.Text      = "비디오 On";
            btnVideo.ForeColor = Color.Blue;
        }
        else
        {
            btnVideo.Text      = "비디오 Off";
            btnVideo.ForeColor = Color.Red;
        }
    }

    private void UpdateMicButton()
    {
        if (!_micMuted)
        {
            btnMic.Text      = "마이크 On";
            btnMic.ForeColor = Color.Blue;
        }
        else
        {
            btnMic.Text      = "마이크 Off";
            btnMic.ForeColor = Color.Red;
        }
    }

    private void UpdateMonitorButton()
    {
        bool running = _monitor != null;
        btnMonitorToggle.Text      = running ? "감시 중지" : "감시 시작";
        btnMonitorToggle.ForeColor = running ? Color.Red : Color.Green;
        notifyIcon.Text            = running ? "WebCam Monitor - 감시 중" : "WebCam Monitor";
    }

    // ── Combo events ─────────────────────────────────────────────────────────

    private void CmbCamera_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!btnVideo.Enabled) return;
        _cameraEnabled = CameraController.IsEnabled(SelectedCamera);
        UpdateVideoButton();
        _monitor?.UpdateDevice(SelectedCamera);
    }

    private void CmbMic_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!btnMic.Enabled) return;
        _micMuted = MicController.IsMuted(SelectedMic);
        UpdateMicButton();
    }

    // ── Button click events ──────────────────────────────────────────────────

    private void BtnVideo_Click(object? sender, EventArgs e)
    {
        bool target = !_cameraEnabled;
        var (result, win32Error, deviceName) = CameraController.SetEnabled(SelectedCamera, target);

        if (result == CameraSetResult.Success)
        {
            _cameraEnabled = target;
            UpdateVideoButton();
            return;
        }

        string detail = result switch
        {
            CameraSetResult.DeviceNotFound  => $"장치를 찾을 수 없습니다: {SelectedCamera}",
            CameraSetResult.SetParamsFailed => $"장치 파라미터 설정 실패\nWin32 오류: {win32Error}",
            CameraSetResult.InstallerFailed => $"장치 상태 변경 실패\nWin32 오류: {win32Error}",
            _                               => "알 수 없는 오류"
        };

        MessageBox.Show(detail, "카메라 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void BtnMic_Click(object? sender, EventArgs e)
    {
        bool target = !_micMuted;
        MicController.SetMuted(SelectedMic, target);
        _micMuted = target;
        UpdateMicButton();
    }

    private void BtnExit_Click(object? sender, EventArgs e)
    {
        using var dlg = new ExitConfirmForm();
        var result = dlg.ShowDialog(this);

        if (result == DialogResult.No)
            HideToTray();
        else if (result == DialogResult.Yes)
            Application.Exit();
    }

    private void BtnMonitorToggle_Click(object? sender, EventArgs e)
    {
        if (_monitor == null)
            StartMonitoring();
        else
            StopMonitoring();
    }

    private void BtnNotificationSettings_Click(object? sender, EventArgs e)
    {
        using var dlg = new NotificationSettingsForm(_settings, _kakao);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        // 텔레그램 노티파이어 갱신
        _telegram.BotToken = _settings.BotToken;
        _telegram.ChatId   = _settings.ChatId;

        // 카카오 노티파이어는 NotificationSettingsForm에서 이미 갱신됨
    }

    // ── Monitoring ───────────────────────────────────────────────────────────

    private void StartMonitoring()
    {
        if (_monitor != null || string.IsNullOrEmpty(SelectedCamera)) return;

        _monitor = new CameraMonitor(SelectedCamera);
        _monitor.CameraEnabled  += OnCameraEnabled;
        _monitor.CameraDisabled += OnCameraDisabled;
        _monitor.CameraInUse    += OnCameraInUse;
        _monitor.CameraReleased += OnCameraReleased;
        _monitor.Start();

        _settings.MonitorEnabled = true;
        _settings.Save();

        SetMonitorStatus("감시 중...");
        UpdateMonitorButton();
    }

    private void StopMonitoring()
    {
        _monitor?.Stop();
        _monitor?.Dispose();
        _monitor = null;

        _settings.MonitorEnabled = false;
        _settings.Save();

        SetMonitorStatus("꺼짐");
        UpdateMonitorButton();
    }

    private void OnCameraEnabled(string deviceName)
    {
        Notify("⚠️ 웹캠 활성화 감지!", $"장치: {deviceName}");
        SetMonitorStatusThreadSafe("감시 중... [카메라 켜짐!]");
    }

    private void OnCameraDisabled(string deviceName)
    {
        Notify("🔴 웹캠 비활성화 감지!", $"장치: {deviceName}");
        SetMonitorStatusThreadSafe("감시 중... [카메라 꺼짐]");
    }

    private void OnCameraInUse(string deviceName)
    {
        Notify("📷 웹캠 사용 감지!", $"장치: {deviceName}");
        SetMonitorStatusThreadSafe("감시 중... [사용 중!]");
    }

    private void OnCameraReleased(string deviceName)
    {
        Notify("⏹️ 웹캠 사용 종료", $"장치: {deviceName}");
        SetMonitorStatusThreadSafe("감시 중...");
    }

    private void Notify(string title, string body)
    {
        notifyIcon.BalloonTipTitle = title;
        notifyIcon.BalloonTipText  = body;
        notifyIcon.ShowBalloonTip(6000);

        string message = $"{title}\n{body}";
        if (_settings.TelegramEnabled)
            Task.Run(() => _telegram.SendAsync(message));
        if (_settings.KakaoEnabled)
            Task.Run(() => _kakao.SendAsync(message));
    }

    private void SetMonitorStatusThreadSafe(string text)
    {
        if (InvokeRequired)
            Invoke(() => SetMonitorStatus(text));
        else
            SetMonitorStatus(text);
    }

    private void SetMonitorStatus(string text) => lblMonitorStatus.Text = $"상태: {text}";

    // ── First run ────────────────────────────────────────────────────────────

    private void HandleFirstRun()
    {
        _settings.FirstRunDone = true;
        _settings.Save();

        if (StartupManager.IsRegistered()) return;

        var answer = MessageBox.Show(
            "Windows 시작 시 WebCam Controller를 자동으로 실행하시겠습니까?\n\n" +
            "(관리자 권한으로 자동 실행되며, 작업 스케줄러에 등록됩니다.)",
            "시작 프로그램 등록",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (answer == DialogResult.Yes)
            StartupManager.Register();
    }

    // ── System tray ──────────────────────────────────────────────────────────

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (WindowState == FormWindowState.Minimized)
            HideToTray();
    }

    private void HideToTray()
    {
        Hide();
        ShowInTaskbar      = false;
        notifyIcon.Visible = true;
        notifyIcon.ShowBalloonTip(2000, "WebCam Monitor",
            "트레이에서 실행 중입니다. 아이콘을 더블클릭하면 창이 열립니다.", ToolTipIcon.Info);
    }

    private void ShowFromTray()
    {
        ShowInTaskbar      = true;
        notifyIcon.Visible = _monitor != null;
        Show();
        WindowState = FormWindowState.Normal;
        Activate();
    }

    private void NotifyIcon_DoubleClick(object? sender, EventArgs e) => ShowFromTray();
    private void TrayMenuOpen_Click(object? sender, EventArgs e)     => ShowFromTray();
    private void TrayMenuExit_Click(object? sender, EventArgs e)     => Application.Exit();
    private void LnkHelp_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        using var dlg = new HelpForm();
        dlg.ShowDialog(this);
    }

    // ── Cleanup ──────────────────────────────────────────────────────────────

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            HideToTray();
            return;
        }

        _monitor?.Stop();
        _monitor?.Dispose();
        _telegram.Dispose();
        _kakao.Dispose();
        notifyIcon.Visible = false;
        base.OnFormClosing(e);
    }
}
