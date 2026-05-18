using System.Text;

namespace WebCamControl;

internal partial class LogViewerForm : Form
{
    internal LogViewerForm()
    {
        InitializeComponent();
        dtpLogDate.Value         = DateTime.Today;
        cmbLogType.SelectedIndex = 0;
        LoadLog();
    }

    private void LoadLog()
    {
        DateTime date       = dtpLogDate.Value.Date;
        bool     isSubtitle = cmbLogType.SelectedIndex == 1;
        string   dir        = isSubtitle ? EventLogger.SubtitleLogDirectory : EventLogger.LogDirectory;
        string   path       = Path.Combine(dir, $"{date:yyyy-MM-dd}.txt");

        Text        = $"{(isSubtitle ? "자막 로그" : "감시 로그")} — {date:yyyy-MM-dd}";
        rtbLog.Text = File.Exists(path)
            ? File.ReadAllText(path, Encoding.UTF8)
            : $"({date:yyyy-MM-dd} 기록된 로그가 없습니다.)";

        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.ScrollToCaret();
    }

    private void BtnRefresh_Click(object? sender, EventArgs e)                       => LoadLog();
    private void DtpLogDate_ValueChanged(object? sender, EventArgs e)                => LoadLog();
    private void CmbLogType_SelectedIndexChanged(object? sender, EventArgs e)        => LoadLog();
    private void BtnClose_Click(object? sender, EventArgs e)                         => Close();
}
