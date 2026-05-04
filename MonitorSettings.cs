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
    internal string BotToken       { get; set; } = string.Empty;
    internal string ChatId         { get; set; } = string.Empty;
    internal bool   MonitorEnabled { get; set; } = false;
    internal bool   FirstRunDone   { get; set; } = false;

    // JSON에 저장되는 DTO — BotToken/ChatId는 DPAPI 암호화된 Base64 문자열
    private sealed class Dto
    {
        public string Token   { get; set; } = string.Empty;
        public string Chat    { get; set; } = string.Empty;
        public bool   Monitor { get; set; }
        public bool   First   { get; set; }
    }

    internal static MonitorSettings Load()
    {
        var s = new MonitorSettings();
        if (!File.Exists(FilePath)) return s;

        try
        {
            var dto = JsonSerializer.Deserialize<Dto>(File.ReadAllText(FilePath));
            if (dto == null) return s;

            s.BotToken       = Decrypt(dto.Token);
            s.ChatId         = Decrypt(dto.Chat);
            s.MonitorEnabled = dto.Monitor;
            s.FirstRunDone   = dto.First;
        }
        catch { /* 파일 손상 시 기본값 사용 */ }

        return s;
    }

    internal void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);

        var dto = new Dto
        {
            Token   = Encrypt(BotToken),
            Chat    = Encrypt(ChatId),
            Monitor = MonitorEnabled,
            First   = FirstRunDone,
        };

        File.WriteAllText(FilePath,
            JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true }));
    }

    /// <summary>Windows DPAPI로 현재 사용자 범위 암호화 → Base64 반환</summary>
    private static string Encrypt(string plain)
    {
        if (string.IsNullOrEmpty(plain)) return string.Empty;

        byte[] cipher = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(plain),
            null,
            DataProtectionScope.CurrentUser);

        return Convert.ToBase64String(cipher);
    }

    /// <summary>Base64 → DPAPI 복호화. 실패 시 빈 문자열 반환</summary>
    private static string Decrypt(string cipher)
    {
        if (string.IsNullOrEmpty(cipher)) return string.Empty;

        try
        {
            byte[] plain = ProtectedData.Unprotect(
                Convert.FromBase64String(cipher),
                null,
                DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(plain);
        }
        catch { return string.Empty; }
    }
}
