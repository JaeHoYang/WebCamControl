using System.Diagnostics;

namespace WebCamControl;

internal partial class MonitorOptionsForm : Form
{
    private readonly MonitorSettings _settings;

    internal MonitorOptionsForm(MonitorSettings settings)
    {
        InitializeComponent();
        _settings = settings;

        nudInterval.Value      = Math.Clamp(settings.MonitorIntervalSeconds, 1, 10);
        chkBalloonTips.Checked = settings.ShowBalloonTips;
        chkStartStop.Checked   = settings.NotifyOnStartStop;
        chkLogEnabled.Checked  = settings.LogEnabled;
        lblLogPath.Text        = EventLogger.LogDirectory;

        UpdateLogButtons();
    }

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

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        _settings.MonitorIntervalSeconds = (int)nudInterval.Value;
        _settings.ShowBalloonTips        = chkBalloonTips.Checked;
        _settings.NotifyOnStartStop      = chkStartStop.Checked;
        _settings.LogEnabled             = chkLogEnabled.Checked;
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
