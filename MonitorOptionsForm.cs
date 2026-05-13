using System.Diagnostics;

namespace WebCamControl;

internal partial class MonitorOptionsForm : Form
{
    private readonly MonitorSettings _settings;

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
    }

    // ── 일반 탭 ─────────────────────────────────────────────────────────

    private void UpdateLogButtons()
    {
        btnOpenFolder.Enabled   = chkLogEnabled.Checked;
        btnViewTodayLog.Enabled = chkLogEnabled.Checked;
    }

    private void ChkLogEnabled_CheckedChanged(object? sender, EventArgs e) =>
        UpdateLogButtons();

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
