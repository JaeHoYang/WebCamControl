namespace WebCamControl;

internal partial class MainForm : Form
{
    private bool _cameraEnabled;
    private bool _micMuted;

    private readonly MonitorSettings    _settings;
    private readonly TelegramNotifier   _telegram;
    private readonly DiscordNotifier    _discord;
    private readonly KakaoNotifier      _kakao;
    private readonly ScreenRecorder     _screenRecorder;
    private readonly RealtimeTranslator _translator = new();
    private CameraMonitor?              _monitor;
    private TranslationOverlayForm?     _overlay;

    private string SelectedCamera => cmbCamera.SelectedItem as string ?? string.Empty;
    private string SelectedMic    => cmbMic.SelectedItem    as string ?? string.Empty;

    public MainForm(bool startMinimized = false)
    {
        InitializeComponent();

        _settings = MonitorSettings.Load();
        EventLogger.DataRoot = _settings.EffectiveDataRoot;
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

        Load += (_, _) =>
        {
            if (startMinimized && _settings.MonitorEnabled)
                HideToTray();
            StartStorageMonitor();
        };
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
            if (EventLogger.DiskSpaceLow)
            {
                MessageBox.Show("디스크 여유 공간 부족으로 녹화를 시작할 수 없습니다.",
                    "저장 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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

    private async void BtnSubtitleToggle_Click(object? sender, EventArgs e)
    {
        if (_overlay != null)
        {
            StopSubtitle();
            return;
        }
        await StartSubtitleAsync();
    }

    private void BtnMonitorOptions_Click(object? sender, EventArgs e)
    {
        string oldRoot = EventLogger.DataRoot;
        using var dlg  = new MonitorOptionsForm(_settings);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        _screenRecorder.MonitorIndex = _settings.RecordMonitorIndex;
        _screenRecorder.Quality      = _settings.RecordScreenQuality;

        string newRoot = _settings.EffectiveDataRoot;
        if (newRoot == oldRoot) return;

        EventLogger.DataRoot = newRoot;
        var ans = MessageBox.Show(
            $"데이터 저장 폴더가 변경되었습니다.\n\n기존: {oldRoot}\n변경: {newRoot}\n\n기존 파일을 새 위치로 이동하시겠습니까?",
            "저장 폴더 변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (ans == DialogResult.Yes)
            TryMoveDataRoot(oldRoot, newRoot);
    }

    private static void TryMoveDataRoot(string src, string dst)
    {
        try
        {
            Directory.CreateDirectory(dst);
            foreach (string sub in new[] { "logs", "subtitle_logs", "recordings" })
            {
                string srcSub = Path.Combine(src, sub);
                string dstSub = Path.Combine(dst, sub);
                if (!Directory.Exists(srcSub)) continue;
                if (!Directory.Exists(dstSub))
                {
                    Directory.Move(srcSub, dstSub);
                }
                else
                {
                    foreach (string f in Directory.GetFiles(srcSub, "*", SearchOption.AllDirectories))
                    {
                        string rel     = Path.GetRelativePath(srcSub, f);
                        string dstFile = Path.Combine(dstSub, rel);
                        Directory.CreateDirectory(Path.GetDirectoryName(dstFile)!);
                        File.Move(f, dstFile, overwrite: false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"파일 이동 중 오류가 발생했습니다:\n{ex.Message}",
                "이동 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

    private void OnCameraInUse(string deviceName, string appName)
    {
        string notifyBody = string.IsNullOrEmpty(appName)
            ? $"장치: {deviceName}"
            : $"앱: {appName}\n장치: {deviceName}";
        string logMsg = string.IsNullOrEmpty(appName)
            ? $"📷 웹캠 사용 감지! — {deviceName}"
            : $"📷 웹캠 사용 감지! — {appName} ({deviceName})";
        Notify("📷 웹캠 사용 감지!", notifyBody);
        Log(logMsg);
        if (_settings.RecordScreenEnabled && !EventLogger.DiskSpaceLow)
        {
            try   { _screenRecorder.Start("webcam"); }
            catch (Exception ex) { Log($"⚠ 녹화 시작 실패: {ex.Message}"); }
        }
        UpdateRecordButtonThreadSafe();
        string statusApp = string.IsNullOrEmpty(appName) ? "사용 중!" : appName;
        SetMonitorStatusThreadSafe($"감시 중... [{statusApp}]");
    }

    private void OnCameraReleased(string deviceName, string appName)
    {
        string notifyBody = string.IsNullOrEmpty(appName)
            ? $"장치: {deviceName}"
            : $"앱: {appName}\n장치: {deviceName}";
        string logMsg = string.IsNullOrEmpty(appName)
            ? $"⏹️ 웹캠 사용 종료 — {deviceName}"
            : $"⏹️ 웹캠 사용 종료 — {appName} ({deviceName})";
        Notify("⏹️ 웹캠 사용 종료", notifyBody);
        Log(logMsg);
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

    // ── Subtitle / Translation ───────────────────────────────────────────

    private async Task StartSubtitleAsync()
    {
        var activeModel = _settings.ActiveVoskModel;
        if (activeModel == null)
        {
            MessageBox.Show("설정 > 번역 탭에서 Vosk 모델을 추가해주세요.",
                "모델 미설정", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnSubtitleToggle.Enabled = false;
        btnSubtitleToggle.Text    = "초기화 중...";

        _translator.VoskModelPath      = activeModel.Path;
        _translator.DeepLApiKey        = _settings.DeepLApiKey;
        _translator.TranslationEnabled = _settings.DeepLTranslationEnabled;
        _translator.SourceLang         = _settings.TranslationSourceLang;
        _translator.TargetLang         = _settings.TranslationTargetLang;

        try
        {
            await _translator.StartAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"자막 시작 실패:\n{ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnSubtitleToggle.Enabled = true;
            UpdateSubtitleButton();
            return;
        }

        var bounds = new Rectangle(_settings.OverlayX, _settings.OverlayY,
                                   _settings.OverlayWidth, _settings.OverlayHeight);
        _overlay = new TranslationOverlayForm(_settings.ShowOriginalText, bounds);
        _overlay.UserClosed += OnOverlayClosed;
        _overlay.Show();

        _translator.PartialReceived += OnPartialReceived;
        _translator.TextReceived   += OnTextReceived;
        _translator.QuotaExceeded  += OnQuotaExceeded;

        btnSubtitleToggle.Enabled = true;
        UpdateSubtitleButton();
    }

    private void StopSubtitle()
    {
        _translator.PartialReceived -= OnPartialReceived;
        _translator.TextReceived   -= OnTextReceived;
        _translator.QuotaExceeded  -= OnQuotaExceeded;
        _translator.Stop();

        if (_overlay != null)
        {
            _overlay.UserClosed -= OnOverlayClosed;
            SaveOverlayBounds();
            _overlay.Close();
            _overlay.Dispose();
            _overlay = null;
        }

        if (!IsDisposed && IsHandleCreated)
            UpdateSubtitleButton();
    }

    private void SaveOverlayBounds()
    {
        if (_overlay == null || _overlay.IsDisposed) return;
        _settings.OverlayX      = _overlay.Bounds.X;
        _settings.OverlayY      = _overlay.Bounds.Y;
        _settings.OverlayWidth  = _overlay.Bounds.Width;
        _settings.OverlayHeight = _overlay.Bounds.Height;
        _settings.Save();
    }

    private void OnOverlayClosed()
    {
        if (_overlay == null) return;

        _translator.PartialReceived -= OnPartialReceived;
        _translator.TextReceived   -= OnTextReceived;
        _translator.QuotaExceeded  -= OnQuotaExceeded;
        _translator.Stop();
        SaveOverlayBounds();
        _overlay = null;

        RunOnUiThread(UpdateSubtitleButton);
    }

    private void OnPartialReceived(string partial)
    {
        if (_overlay == null || _overlay.IsDisposed) return;
        _overlay.ShowPartial(partial);
    }

    private void OnTextReceived(string original, string translated)
    {
        if (_overlay == null || _overlay.IsDisposed) return;

        if (_settings.DeepLTranslationEnabled)
            _overlay.ShowText(original, translated);
        else
            _overlay.ShowSubtitle(original);

        if (_settings.LogTranslation)
        {
            EventLogger.WriteSubtitle($"원문: {original}");
            if (_settings.DeepLTranslationEnabled && original != translated)
                EventLogger.WriteSubtitle($"번역: {translated}");
        }
    }

    private void OnQuotaExceeded()
    {
        if (_overlay != null && !_overlay.IsDisposed)
            _overlay.ShowWarning("⚠ DeepL 무료 한도(50만 자) 초과 — 자막만 계속 표시됩니다");
        Notify("DeepL 무료 한도 초과", "번역이 중단되었습니다. 자막은 계속 표시됩니다.");
        EventLogger.WriteSubtitle("DeepL 무료 한도 초과 — 번역 중단, 자막 모드로 전환");
        _translator.TranslationEnabled = false;
    }

    private void UpdateSubtitleButton()
    {
        bool running = _overlay != null;
        btnSubtitleToggle.Text      = running ? "자막 중지" : "자막 시작";
        btnSubtitleToggle.ForeColor = running ? Color.Red : Color.DarkSlateBlue;
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

    // ── Storage monitoring ───────────────────────────────────────────────

    private void StartStorageMonitor()
    {
        CheckDiskSpace();    // 드라이브 여유 공간 10% 미만 체크 (알림 + 저장 차단)
        CheckStorageSize();  // 로그·영상 누적 용량 체크 (로그만 기록)
    }

    private void CheckDiskSpace()
    {
        try
        {
            string root = Path.GetPathRoot(EventLogger.DataRoot)!;
            var drive = new DriveInfo(root);
            double freePercent = (double)drive.AvailableFreeSpace / drive.TotalSize * 100;
            if (freePercent >= 10.0) return;

            double freeGB  = drive.AvailableFreeSpace / (1024.0 * 1024 * 1024);
            double totalGB = drive.TotalSize          / (1024.0 * 1024 * 1024);
            string detail  = $"{drive.Name} 여유 {freeGB:F1} GB / 전체 {totalGB:F0} GB ({freePercent:F1}%)";

            // 플래그 설정 전에 먼저 로그 기록
            EventLogger.Write($"⚠ 디스크 공간 부족 — {detail} — 로그·영상 저장 비활성화");
            EventLogger.DiskSpaceLow = true;

            string msg = $"디스크 여유 공간이 10% 미만입니다.\n{detail}\n\n로그 및 영상 저장이 비활성화됩니다.";
            RunOnUiThread(() =>
            {
                if (notifyIcon.Visible)
                    notifyIcon.ShowBalloonTip(10000, "⚠ 디스크 공간 부족", msg, ToolTipIcon.Warning);
                else
                    MessageBox.Show(msg, "⚠ 디스크 공간 부족", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            });
        }
        catch { }
    }

    private void CheckStorageSize()
    {
        if (!_settings.StorageWarningEnabled) return;

        long logMB = GetDirSizeMB(EventLogger.LogDirectory)
                   + GetDirSizeMB(EventLogger.SubtitleLogDirectory);
        long recMB = GetDirSizeMB(ScreenRecorder.SaveDirectory);

        var warnings = new List<string>();
        if (logMB > _settings.LogSizeWarningMB)
            warnings.Add($"로그: {logMB:N0} MB (기준 {_settings.LogSizeWarningMB:N0} MB 초과)");
        if (recMB > _settings.RecordSizeWarningMB)
            warnings.Add($"영상: {FormatSizeMB(recMB)} (기준 {FormatSizeMB(_settings.RecordSizeWarningMB)} 초과)");

        if (warnings.Count == 0) return;
        Log($"⚠ 저장 용량 경고 — {string.Join(" / ", warnings)} — 설정에서 폴더를 열어 정리하세요.");
    }

    private static long GetDirSizeMB(string path)
    {
        if (!Directory.Exists(path)) return 0;
        try
        {
            return new DirectoryInfo(path)
                .GetFiles("*", SearchOption.AllDirectories)
                .Sum(f => f.Length) / (1024 * 1024);
        }
        catch { return 0; }
    }

    private static string FormatSizeMB(long mb)
        => mb >= 1024 ? $"{mb / 1024.0:F1} GB" : $"{mb:N0} MB";

    private void RunOnUiThread(Action action)
    {
        if (InvokeRequired) Invoke(action);
        else action();
    }

    private void UpdateRecordButtonThreadSafe() => RunOnUiThread(UpdateRecordButton);

    private void SetMonitorStatusThreadSafe(string text) => RunOnUiThread(() => SetMonitorStatus(text));

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
        if (_overlay != null) StopSubtitle();
        _translator.Dispose();
        _telegram.Dispose();
        _discord.Dispose();
        _kakao.Dispose();
        _screenRecorder.Dispose();
        notifyIcon.Visible = false;
        base.OnFormClosing(e);
    }
}
