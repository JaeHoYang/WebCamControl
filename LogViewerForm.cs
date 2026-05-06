using System.Text;

namespace WebCamControl;

internal partial class LogViewerForm : Form
{
    internal LogViewerForm()
    {
        InitializeComponent();
        Text = $"로그 — {DateTime.Now:yyyy-MM-dd}";
        LoadLog();
    }

    private void LoadLog()
    {
        string path = EventLogger.TodayLogPath;
        rtbLog.Text = File.Exists(path)
            ? File.ReadAllText(path, Encoding.UTF8)
            : "(오늘 기록된 로그가 없습니다.)";

        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.ScrollToCaret();
    }

    private void BtnRefresh_Click(object? sender, EventArgs e) => LoadLog();
    private void BtnClose_Click(object? sender, EventArgs e)   => Close();
}
