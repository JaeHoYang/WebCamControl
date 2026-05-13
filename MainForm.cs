namespace WebCamControl;

public partial class MainForm : Form
{
    private bool _cameraEnabled;
    private bool _micMuted;

    private readonly MonitorSettings  _settings;
    private readonly TelegramNotifier _telegram;
    private readonly DiscordNotifier  _discord;
    private readonly KakaoNotifier    _kakao;
    private readonly ScreenRecorder   _screenRecorder;
    private CameraMonitor? _monitor;

    private string SelectedCamera => cmbCamera.SelectedItem as string ?? string.Empty;
    private string SelectedMic    => cmbMic.SelectedItem    as string ?? string.Empty;

    public MainForm(bool startMinimized = false)
    {
        InitializeComponent();

        _settings = MonitorSettings.Load();
        _telegram = new TelegramNotifier { BotToken = _settings.BotToken, ChatId = _settings.ChatId };
        _discord  = new DiscordNotifier  { WebhookUrl = _settings.DiscordWebhookUrl };
        _kakao    = new KakaoNotifier
        {
            IsEnabled    = _settings.KakaoEnabled,
            RestApiKey   = _settings.KakaoAppKey,
            AccessToken  = _settings.KakaoAccessToken,
            RefreshToken = _settings.KakaoRefreshToken,
        };
        _screenRecorder = new ScreenRecorder
        {
            MonitorIndex = _settings.RecordMonitorIndex,
            Quality      = _settings.RecordScreenQuality,
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

    private void BtnRecordToggle_Click(object? sender, EventArgs e)
    {
        if (IsAnyRecording)
        {
            _screenRecorder.Stop();
        }
        else
        {
            if (!_settings.RecordScreenEnabled)
            {
                MessageBox.Show("설정에서 화면 녹화를 활성화해주세요.",
                    "녹화 설정 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _screenRecorder.Start("manual");
        }
        UpdateRecordButton();
    }

    private void BtnMonitorOptions_Click(object? sender, EventArgs e)
    {
        using var dlg = new MonitorOptionsForm(_settings);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _screenRecorder.MonitorIndex = _settings.RecordMonitorIndex;
            _screenRecorder.Quality      = _settings.RecordScreenQuality;
        }
    }

    private void BtnNotificationSettings_Click(object? sender, EventArgs e)
    {
        using var dlg = new NotificationSettingsForm(_settings, _discord, _kakao);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        _telegram.BotToken = _settings.BotToken;
        _telegram.ChatId   = _settings.ChatId;
        // _discord.WebhookUrl, _kakao 속성은 NotificationSettingsForm에서 이미 갱신됨
    }

    // ── Monitoring ───────────────────────────────────────────────────────────

    private void StartMonitoring()
    {
        if (_monitor != null || string.IsNullOrEmpty(SelectedCamera)) return;

        _monitor = new CameraMonitor(SelectedCamera, _settings.MonitorIntervalSeconds * 1000.0);
        _monitor.CameraEnabled  += OnCameraEnabled;
        _monitor.CameraDisabled += OnCameraDisabled;
        _monitor.CameraInUse    += OnCameraInUse;
        _monitor.CameraReleased += OnCameraReleased;
        _monitor.Start();

        _settings.MonitorEnabled = true;
        _settings.Save();

        if (_settings.NotifyOnStartStop)
            Notify("🟢 웹캠 감시 시작", "WebCam Monitor 감시를 시작합니다.");
        Log("🟢 웹캠 감시 시작");

        SetMonitorStatus("감시 중...");
        UpdateMonitorButton();
    }

    private void StopMonitoring()
    {
        if (_settings.NotifyOnStartStop)
            Notify("⏹️ 웹캠 감시 종료", "WebCam Monitor 감시를 중지합니다.");
        Log("⏹️ 웹캠 감시 종료");

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
        Log($"⚠️ 웹캠 활성화 감지! — {deviceName}");
        SetMonitorStatusThreadSafe("감시 중... [카메라 켜짐!]");
    }

    private void OnCameraDisabled(string deviceName)
    {
        Notify("🔴 웹캠 비활성화 감지!", $"장치: {deviceName}");
        Log($"🔴 웹캠 비활성화 감지! — {deviceName}");
        SetMonitorStatusThreadSafe("감시 중... [카메라 꺼짐]");
    }

    private void OnCameraInUse(string deviceName)
    {
        Notify("📷 웹캠 사용 감지!", $"장치: {deviceName}");
        Log($"📷 웹캠 사용 감지! — {deviceName}");
        if (_settings.RecordScreenEnabled) _screenRecorder.Start("webcam");
        UpdateRecordButtonThreadSafe();
        SetMonitorStatusThreadSafe("감시 중... [사용 중!]");
    }

    private void OnCameraReleased(string deviceName)
    {
        Notify("⏹️ 웹캠 사용 종료", $"장치: {deviceName}");
        Log($"⏹️ 웹캠 사용 종료 — {deviceName}");
        _screenRecorder.Stop();
        UpdateRecordButtonThreadSafe();
        SetMonitorStatusThreadSafe("감시 중...");
    }

    private void Notify(string title, string body)
    {
        if (_settings.ShowBalloonTips)
        {
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText  = body;
            notifyIcon.ShowBalloonTip(6000);
        }

        string message = $"{title}\n{body}";
        if (_settings.TelegramEnabled) Task.Run(() => _telegram.SendAsync(message));
        if (_settings.DiscordEnabled)  Task.Run(() => _discord.SendAsync(message));
        if (_kakao.IsConfigured)       Task.Run(() => _kakao.SendAsync(message));
    }

    // 프로그램 종료 시처럼 Task.Run이 완료되기 전에 프로세스가 끝날 수 있는 경우 사용
    private void NotifySync(string title, string body)
    {
        string message = $"{title}\n{body}";
        var tasks = new List<Task>();
        if (_settings.TelegramEnabled) tasks.Add(_telegram.SendAsync(message));
        if (_settings.DiscordEnabled)  tasks.Add(_discord.SendAsync(message));
        if (_kakao.IsConfigured)       tasks.Add(_kakao.SendAsync(message));
        if (tasks.Count > 0)
            Task.WhenAll(tasks).Wait(TimeSpan.FromSeconds(3));
    }

    private void Log(string message)
    {
        if (_settings.LogEnabled)
            EventLogger.Write(message);
    }

    private bool IsAnyRecording => _screenRecorder.IsRecording;

    private void UpdateRecordButton()
    {
        bool rec = IsAnyRecording;
        btnRecordToggle.Text      = rec ? "녹화 중지" : "녹화 시작";
        btnRecordToggle.ForeColor = rec ? Color.Red : Color.DarkRed;
        lblRecordStatus.Text      = $"상태: {(rec ? "녹화 중..." : "꺼짐")}";
        lblRecordStatus.ForeColor = rec ? Color.Red : Color.Gray;
    }

    private void UpdateRecordButtonThreadSafe()
    {
        if (InvokeRequired) Invoke(UpdateRecordButton);
        else UpdateRecordButton();
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

        if (_monitor != null)
        {
            Log("🔴 WebCam Monitor 종료");
            if (_settings.NotifyOnStartStop)
                NotifySync("🔴 WebCam Monitor 종료", "프로그램 종료로 감시가 중단되었습니다.");
        }

        _monitor?.Stop();
        _monitor?.Dispose();
        _screenRecorder.Stop();
        _telegram.Dispose();
        _discord.Dispose();
        _kakao.Dispose();
        _screenRecorder.Dispose();
        notifyIcon.Visible = false;
        base.OnFormClosing(e);
    }
}
