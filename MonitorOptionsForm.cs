using System.Diagnostics;

namespace WebCamControl;

internal partial class MonitorOptionsForm : Form
{
    private readonly MonitorSettings _settings;

    private List<VoskModelEntry> _voskModels  = new();
    private int                  _activeModelIdx = 0;

    private static readonly string[] SourceLangCodes   = { "auto", "KO", "EN", "JA", "ZH" };
    private static readonly string[] SourceLangDisplay = { "자동 감지", "한국어", "영어", "일본어", "중국어 (간체)" };
    private static readonly string[] TargetLangCodes   = { "KO", "EN-US", "JA", "ZH" };
    private static readonly string[] TargetLangDisplay = { "한국어", "영어 (미국)", "일본어", "중국어 (간체)" };

    internal MonitorOptionsForm(MonitorSettings settings)
    {
        InitializeComponent();
        _settings = settings;

        // 일반 탭
        nudInterval.Value      = Math.Clamp(settings.MonitorIntervalSeconds, 1, 10);
        chkBalloonTips.Checked = settings.ShowBalloonTips;
        chkStartStop.Checked   = settings.NotifyOnStartStop;
        chkLogEnabled.Checked  = settings.LogEnabled;
        lblLogPath.Text        = EventLogger.LogDirectory;
        UpdateLogButtons();

        // 화면 녹화 탭
        chkRecordScreen.Checked = settings.RecordScreenEnabled;

        PopulateMonitors();
        cmbRecordMonitor.SelectedIndex = Math.Clamp(settings.RecordMonitorIndex, 0,
            Math.Max(0, cmbRecordMonitor.Items.Count - 1));

        SetScreenQuality(settings.RecordScreenQuality);
        UpdateRecordScreenControls();

        // 저장 공간 경고
        chkStorageWarning.Checked = settings.StorageWarningEnabled;
        nudLogWarnMB.Value        = Math.Clamp(settings.LogSizeWarningMB, 10, 10000);
        nudRecWarnGB.Value        = Math.Clamp(settings.RecordSizeWarningMB / 1024, 1, 1000);
        UpdateStorageWarningControls();
        RefreshCurrentUsage();

        // 저장 폴더
        UpdateDataRootDisplay(settings.DataRootParent);

        // 번역 탭
        chkTransEnabled.Checked = settings.TranslationEnabled;
        _voskModels    = settings.VoskModels.Select(m => new VoskModelEntry { Name = m.Name, Path = m.Path }).ToList();
        _activeModelIdx = settings.ActiveVoskModelIndex;
        PopulateVoskList();
        chkDeepLOn.Checked      = settings.DeepLTranslationEnabled;
        txtDeepLKey.Text        = settings.DeepLApiKey;
        chkShowOrig.Checked     = settings.ShowOriginalText;
        chkLogTrans.Checked     = settings.LogTranslation;

        cmbSrcLang.Items.AddRange(SourceLangDisplay);
        int srcIdx = Array.IndexOf(SourceLangCodes, settings.TranslationSourceLang);
        cmbSrcLang.SelectedIndex = srcIdx >= 0 ? srcIdx : 0;

        cmbTgtLang.Items.AddRange(TargetLangDisplay);
        int tgtIdx = Array.IndexOf(TargetLangCodes, settings.TranslationTargetLang);
        cmbTgtLang.SelectedIndex = tgtIdx >= 0 ? tgtIdx : 0;

        UpdateTranslationControls();
    }

    // ── 일반 탭 ─────────────────────────────────────────────────────────

    private void UpdateLogButtons()
    {
        btnOpenFolder.Enabled   = chkLogEnabled.Checked;
        btnViewTodayLog.Enabled = chkLogEnabled.Checked;
    }

    private void ChkLogEnabled_CheckedChanged(object? sender, EventArgs e) =>
        UpdateLogButtons();

    // ── 저장 공간 경고 ────────────────────────────────────────────────

    private void UpdateStorageWarningControls()
    {
        bool on = chkStorageWarning.Checked;
        nudLogWarnMB.Enabled = on;
        nudRecWarnGB.Enabled = on;
    }

    private void ChkStorageWarning_CheckedChanged(object? sender, EventArgs e) =>
        UpdateStorageWarningControls();

    private void RefreshCurrentUsage()
    {
        long logMB = GetDirSizeMB(EventLogger.LogDirectory)
                   + GetDirSizeMB(EventLogger.SubtitleLogDirectory);
        long recMB = GetDirSizeMB(ScreenRecorder.SaveDirectory);
        lblCurrentUsage.Text = $"현재: 로그 {logMB:N0} MB / 영상 {FormatSizeMB(recMB)}";
    }

    private static long GetDirSizeMB(string path)
    {
        if (!Directory.Exists(path)) return 0;
        return new DirectoryInfo(path)
            .GetFiles("*", SearchOption.AllDirectories)
            .Sum(f => f.Length) / (1024 * 1024);
    }

    private static string FormatSizeMB(long mb)
        => mb >= 1024 ? $"{mb / 1024.0:F1} GB" : $"{mb:N0} MB";

    // ── 저장 폴더 ─────────────────────────────────────────────────────────

    private void UpdateDataRootDisplay(string parentPath)
    {
        if (string.IsNullOrEmpty(parentPath))
        {
            txtDataRootParent.Text        = string.Empty;
            txtDataRootParent.PlaceholderText = $"기본값 ({EventLogger.DataRoot})";
            lblDataRootPreview.Text       = $"→ {EventLogger.DataRoot}";
        }
        else
        {
            txtDataRootParent.Text  = parentPath;
            lblDataRootPreview.Text = $"→ {Path.Combine(parentPath, "WebCamData")}";
        }
    }

    private void BtnBrowseDataRoot_Click(object? sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description            = "WebCamData 폴더가 생성될 상위 폴더를 선택하세요",
            UseDescriptionForTitle = true,
            ShowNewFolderButton    = true,
        };
        if (!string.IsNullOrEmpty(txtDataRootParent.Text))
            dlg.InitialDirectory = txtDataRootParent.Text;

        if (dlg.ShowDialog(this) == DialogResult.OK)
            UpdateDataRootDisplay(dlg.SelectedPath);
    }

    private void BtnResetDataRoot_Click(object? sender, EventArgs e) =>
        UpdateDataRootDisplay(string.Empty);

    private void BtnOpenFolder_Click(object? sender, EventArgs e)
    {
        Directory.CreateDirectory(EventLogger.LogDirectory);
        Process.Start("explorer.exe", EventLogger.LogDirectory);
    }

    private void BtnViewTodayLog_Click(object? sender, EventArgs e)
    {
        using var dlg = new LogViewerForm();
        dlg.ShowDialog(this);
    }

    // ── 화면 녹화 탭 ────────────────────────────────────────────────────

    private void PopulateMonitors()
    {
        cmbRecordMonitor.Items.Clear();
        foreach (var s in Screen.AllScreens)
        {
            string label = s.Primary
                ? $"주 모니터 ({s.Bounds.Width}×{s.Bounds.Height})"
                : $"모니터 ({s.Bounds.Width}×{s.Bounds.Height})";
            cmbRecordMonitor.Items.Add(label);
        }
        if (cmbRecordMonitor.Items.Count == 0)
            cmbRecordMonitor.Items.Add("(모니터 없음)");
    }

    private void SetScreenQuality(int q)
    {
        rdoScreenLow.Checked  = q == 0;
        rdoScreenMid.Checked  = q == 1;
        rdoScreenHigh.Checked = q == 2;
    }

    private int GetScreenQuality() => rdoScreenLow.Checked ? 0 : rdoScreenHigh.Checked ? 2 : 1;

    private void UpdateRecordScreenControls()
    {
        bool on = chkRecordScreen.Checked;
        cmbRecordMonitor.Enabled = on;
        rdoScreenLow.Enabled     = on;
        rdoScreenMid.Enabled     = on;
        rdoScreenHigh.Enabled    = on;
    }

    private void ChkRecordScreen_CheckedChanged(object? sender, EventArgs e) =>
        UpdateRecordScreenControls();

    private void BtnOpenRecordFolder_Click(object? sender, EventArgs e)
    {
        Directory.CreateDirectory(ScreenRecorder.SaveDirectory);
        Process.Start("explorer.exe", ScreenRecorder.SaveDirectory);
    }

    // ── 번역 탭 ─────────────────────────────────────────────────────────

    private void UpdateTranslationControls()
    {
        bool featureOn = chkTransEnabled.Checked;
        lstVoskModels.Enabled  = featureOn;
        btnAddModel.Enabled    = featureOn;
        btnRemoveModel.Enabled = featureOn;
        lnkVoskDl.Enabled      = featureOn;
        lblSecDeepL.Enabled    = featureOn;
        chkDeepLOn.Enabled     = featureOn;
        UpdateDeepLControls();
    }

    private void UpdateDeepLControls()
    {
        bool deepLOn = chkTransEnabled.Checked && chkDeepLOn.Checked;
        txtDeepLKey.Enabled   = deepLOn;
        btnShowKey.Enabled    = deepLOn;
        lnkDeepLGet.Enabled   = deepLOn;
        lblSrcLang.Enabled    = deepLOn;
        cmbSrcLang.Enabled    = deepLOn;
        lblTgtLang.Enabled    = deepLOn;
        cmbTgtLang.Enabled    = deepLOn;
        btnCheckUsage.Enabled = deepLOn;
    }

    private void ChkTransEnabled_CheckedChanged(object? sender, EventArgs e) =>
        UpdateTranslationControls();

    private void ChkDeepLOn_CheckedChanged(object? sender, EventArgs e) =>
        UpdateDeepLControls();

    private void PopulateVoskList()
    {
        lstVoskModels.Items.Clear();
        for (int i = 0; i < _voskModels.Count; i++)
        {
            var item = new ListViewItem(_voskModels[i].Name);
            item.SubItems.Add(_voskModels[i].Path);
            if (i == _activeModelIdx)
            {
                item.BackColor = Color.LightBlue;
                item.Font      = new Font(lstVoskModels.Font, FontStyle.Bold);
            }
            lstVoskModels.Items.Add(item);
        }
    }

    private void LstVoskModels_Click(object? sender, EventArgs e)
    {
        if (lstVoskModels.SelectedIndices.Count == 0) return;
        _activeModelIdx = lstVoskModels.SelectedIndices[0];
        PopulateVoskList();
    }

    private void BtnAddModel_Click(object? sender, EventArgs e)
    {
        using var dlg = new VoskModelEntryForm();
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        _voskModels.Add(new VoskModelEntry { Name = dlg.ModelName, Path = dlg.ModelPath });
        if (_voskModels.Count == 1) _activeModelIdx = 0;
        PopulateVoskList();
    }

    private void BtnRemoveModel_Click(object? sender, EventArgs e)
    {
        if (lstVoskModels.SelectedIndices.Count == 0) return;
        int idx = lstVoskModels.SelectedIndices[0];
        _voskModels.RemoveAt(idx);
        _activeModelIdx = Math.Clamp(_activeModelIdx, 0, Math.Max(0, _voskModels.Count - 1));
        PopulateVoskList();
    }

    private void BtnShowKey_Click(object? sender, EventArgs e)
    {
        txtDeepLKey.PasswordChar = txtDeepLKey.PasswordChar == '\0' ? '*' : '\0';
        btnShowKey.Text          = txtDeepLKey.PasswordChar == '\0' ? "🔒" : "👁";
    }

    private void LnkVoskDl_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName        = "https://alphacephei.com/vosk/models",
            UseShellExecute = true,
        });
    }

    private void LnkDeepLGet_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName        = "https://www.deepl.com/pro-api",
            UseShellExecute = true,
        });
    }

    private async void BtnCheckUsage_Click(object? sender, EventArgs e)
    {
        string key = txtDeepLKey.Text.Trim();
        if (string.IsNullOrEmpty(key))
        {
            lblUsage.Text      = "API Key를 입력하세요";
            lblUsage.ForeColor = Color.Red;
            return;
        }

        btnCheckUsage.Enabled = false;
        lblUsage.Text         = "조회 중...";
        lblUsage.ForeColor    = Color.DimGray;

        try
        {
            using var client = new DeepLClient { ApiKey = key };
            var (used, limit) = await client.GetUsageAsync();
            long pct = limit > 0 ? used * 100 / limit : 0;
            lblUsage.Text      = $"이번 달: {used:N0} / {limit:N0} 자 ({pct}%)";
            lblUsage.ForeColor = pct >= 90 ? Color.DarkOrange : Color.Black;
        }
        catch
        {
            lblUsage.Text      = "조회 실패 (API Key 확인)";
            lblUsage.ForeColor = Color.Red;
        }

        btnCheckUsage.Enabled = true;
    }

    private void BtnOpenSubtitleFolder_Click(object? sender, EventArgs e)
    {
        Directory.CreateDirectory(EventLogger.SubtitleLogDirectory);
        Process.Start("explorer.exe", EventLogger.SubtitleLogDirectory);
    }

    // ── 확인 / 취소 ─────────────────────────────────────────────────────

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        _settings.MonitorIntervalSeconds = (int)nudInterval.Value;
        _settings.ShowBalloonTips        = chkBalloonTips.Checked;
        _settings.NotifyOnStartStop      = chkStartStop.Checked;
        _settings.LogEnabled             = chkLogEnabled.Checked;

        _settings.RecordScreenEnabled = chkRecordScreen.Checked;
        _settings.RecordMonitorIndex  = Math.Max(0, cmbRecordMonitor.SelectedIndex);
        _settings.RecordScreenQuality = GetScreenQuality();

        _settings.StorageWarningEnabled = chkStorageWarning.Checked;
        _settings.LogSizeWarningMB      = (int)nudLogWarnMB.Value;
        _settings.RecordSizeWarningMB   = (int)nudRecWarnGB.Value * 1024;
        _settings.DataRootParent        = txtDataRootParent.Text.Trim();

        _settings.TranslationEnabled      = chkTransEnabled.Checked;
        _settings.VoskModels              = _voskModels;
        _settings.ActiveVoskModelIndex    = _activeModelIdx;
        _settings.DeepLTranslationEnabled = chkDeepLOn.Checked;
        _settings.DeepLApiKey             = txtDeepLKey.Text.Trim();
        _settings.TranslationSourceLang   = SourceLangCodes[Math.Max(0, cmbSrcLang.SelectedIndex)];
        _settings.TranslationTargetLang   = TargetLangCodes[Math.Max(0, cmbTgtLang.SelectedIndex)];
        _settings.ShowOriginalText        = chkShowOrig.Checked;
        _settings.LogTranslation          = chkLogTrans.Checked;

        _settings.Save();
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
