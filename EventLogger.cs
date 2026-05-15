using System.Text;

namespace WebCamControl;

internal static class EventLogger
{
    // 디스크 공간 부족 시 true — Write/WriteSubtitle 호출을 모두 무시
    internal static bool DiskSpaceLow { get; set; } = false;

    internal static string DataRoot { get; set; } =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WebCamControl");

    internal static string LogDirectory         => Path.Combine(DataRoot, "logs");
    internal static string SubtitleLogDirectory => Path.Combine(DataRoot, "subtitle_logs");

    internal static string TodayLogPath =>
        Path.Combine(LogDirectory, $"{DateTime.Now:yyyy-MM-dd}.txt");

    internal static string TodaySubtitleLogPath =>
        Path.Combine(SubtitleLogDirectory, $"{DateTime.Now:yyyy-MM-dd}.txt");

    internal static void Write(string message)
    {
        if (DiskSpaceLow) return;
        try
        {
            Directory.CreateDirectory(LogDirectory);
            string line = $"[{DateTime.Now:HH:mm:ss}] {message}";
            File.AppendAllText(TodayLogPath, line + Environment.NewLine, Encoding.UTF8);
        }
        catch { /* best-effort */ }
    }

    internal static void WriteSubtitle(string message)
    {
        if (DiskSpaceLow) return;
        try
        {
            Directory.CreateDirectory(SubtitleLogDirectory);
            string line = $"[{DateTime.Now:HH:mm:ss}] {message}";
            File.AppendAllText(TodaySubtitleLogPath, line + Environment.NewLine, Encoding.UTF8);
        }
        catch { /* best-effort */ }
    }
}
