using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace WebCamControl;

internal sealed class MonitorSettings
{
    private static readonly string FilePath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WebCamControl", "settings.json");

    // 메모리상에서는 평문으로 보관
    internal string BotToken                { get; set; } = string.Empty;
    internal string ChatId                  { get; set; } = string.Empty;
    internal bool   MonitorEnabled          { get; set; } = false;
    internal bool   FirstRunDone            { get; set; } = false;
    internal bool   TelegramEnabled         { get; set; } = true;
    internal bool   DiscordEnabled          { get; set; } = false;
    internal string DiscordWebhookUrl       { get; set; } = string.Empty;
    internal int    MonitorIntervalSeconds  { get; set; } = 3;
    internal bool   LogEnabled              { get; set; } = false;
    internal bool   ShowBalloonTips         { get; set; } = true;
    internal bool   NotifyOnStartStop       { get; set; } = true;

    // JSON에 저장되는 DTO — 민감 문자열은 DPAPI 암호화된 Base64
    private sealed class Dto
    {
        public string Token          { get; set; } = string.Empty;
        public string Chat           { get; set; } = string.Empty;
        public bool   Monitor        { get; set; }
        public bool   First          { get; set; }
        public bool   TelegramOn     { get; set; } = true;
        public bool   DiscordOn      { get; set; }
        public string Discord        { get; set; } = string.Empty;
        public int    Interval       { get; set; } = 3;
        public bool   LogOn          { get; set; }
        public bool   BalloonOn      { get; set; } = true;
        public bool   StartStopOn    { get; set; } = true;
    }

    internal static MonitorSettings Load()
    {
        var s = new MonitorSettings();
        if (!File.Exists(FilePath)) return s;

        try
        {
            var dto = JsonSerializer.Deserialize<Dto>(File.ReadAllText(FilePath));
            if (dto == null) return s;

            s.BotToken               = Decrypt(dto.Token);
            s.ChatId                 = Decrypt(dto.Chat);
            s.MonitorEnabled         = dto.Monitor;
            s.FirstRunDone           = dto.First;
            s.TelegramEnabled        = dto.TelegramOn;
            s.DiscordEnabled         = dto.DiscordOn;
            s.DiscordWebhookUrl      = Decrypt(dto.Discord);
            s.MonitorIntervalSeconds = Math.Clamp(dto.Interval, 1, 10);
            s.LogEnabled             = dto.LogOn;
            s.ShowBalloonTips        = dto.BalloonOn;
            s.NotifyOnStartStop      = dto.StartStopOn;
        }
        catch { /* 파일 손상 시 기본값 사용 */ }

        return s;
    }

    internal void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);

        var dto = new Dto
        {
            Token       = Encrypt(BotToken),
            Chat        = Encrypt(ChatId),
            Monitor     = MonitorEnabled,
            First       = FirstRunDone,
            TelegramOn  = TelegramEnabled,
            DiscordOn   = DiscordEnabled,
            Discord     = Encrypt(DiscordWebhookUrl),
            Interval    = MonitorIntervalSeconds,
            LogOn       = LogEnabled,
            BalloonOn   = ShowBalloonTips,
            StartStopOn = NotifyOnStartStop,
        };

        File.WriteAllText(FilePath,
            JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true }));
    }

    private static string Encrypt(string plain)
    {
        if (string.IsNullOrEmpty(plain)) return string.Empty;
        byte[] cipher = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(plain), null, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(cipher);
    }

    private static string Decrypt(string cipher)
    {
        if (string.IsNullOrEmpty(cipher)) return string.Empty;
        try
        {
            byte[] plain = ProtectedData.Unprotect(
                Convert.FromBase64String(cipher), null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(plain);
        }
        catch { return string.Empty; }
    }
}
