using System.Text;

namespace WebCamControl;

internal static class EventLogger
{
    private static readonly string LogDir =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WebCamControl", "logs");

    internal static string LogDirectory => LogDir;

    internal static string TodayLogPath =>
        Path.Combine(LogDir, $"{DateTime.Now:yyyy-MM-dd}.txt");

    internal static void Write(string message)
    {
        try
        {
            Directory.CreateDirectory(LogDir);
            string line = $"[{DateTime.Now:HH:mm:ss}] {message}";
            File.AppendAllText(TodayLogPath, line + Environment.NewLine, Encoding.UTF8);
        }
        catch { /* best-effort */ }
    }
}
