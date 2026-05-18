using System.Diagnostics;

namespace WebCamControl;

internal partial class RecordingListForm : Form
{
    internal RecordingListForm()
    {
        InitializeComponent();
        LoadRecordings();
    }

    private void LoadRecordings()
    {
        lvRecordings.Items.Clear();
        string dir = ScreenRecorder.SaveDirectory;

        if (!Directory.Exists(dir))
        {
            lblCount.Text = "총 0개";
            return;
        }

        var files = new DirectoryInfo(dir)
            .GetFiles("*.mp4", SearchOption.TopDirectoryOnly)
            .OrderByDescending(f => f.LastWriteTime)
            .ToList();

        foreach (var f in files)
        {
            var item = new ListViewItem(f.Name);
            item.SubItems.Add(f.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
            item.SubItems.Add(FormatSize(f.Length));
            item.Tag = f.FullName;
            lvRecordings.Items.Add(item);
        }

        lblCount.Text = $"총 {files.Count}개";
    }

    private static string FormatSize(long bytes)
    {
        if (bytes >= 1024L * 1024 * 1024) return $"{bytes / (1024.0 * 1024 * 1024):F1} GB";
        if (bytes >= 1024 * 1024)         return $"{bytes / (1024.0 * 1024):F1} MB";
        return $"{bytes / 1024.0:F0} KB";
    }

    private void LvRecordings_DoubleClick(object? sender, EventArgs e)
    {
        if (lvRecordings.SelectedItems.Count == 0) return;
        string path = (string)lvRecordings.SelectedItems[0].Tag!;
        if (File.Exists(path))
            Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
    }

    private void BtnOpenFolder_Click(object? sender, EventArgs e)
    {
        Directory.CreateDirectory(ScreenRecorder.SaveDirectory);
        Process.Start("explorer.exe", ScreenRecorder.SaveDirectory);
    }

    private void BtnRefresh_Click(object? sender, EventArgs e) => LoadRecordings();
    private void BtnClose_Click(object? sender, EventArgs e)   => Close();
}
