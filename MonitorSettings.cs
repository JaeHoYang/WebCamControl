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
    internal string BotToken          { get; set; } = string.Empty;
    internal string ChatId            { get; set; } = string.Empty;
    internal bool   MonitorEnabled    { get; set; } = false;
    internal bool   FirstRunDone      { get; set; } = false;
    internal bool   TelegramEnabled   { get; set; } = true;
    internal bool   KakaoEnabled      { get; set; } = false;
    internal string KakaoRestApiKey   { get; set; } = string.Empty;
    internal string KakaoAccessToken  { get; set; } = string.Empty;
    internal string KakaoRefreshToken { get; set; } = string.Empty;
    internal bool   KakaoIsConfigured =>
        !string.IsNullOrEmpty(KakaoAccessToken) || !string.IsNullOrEmpty(KakaoRefreshToken);

    // JSON에 저장되는 DTO — 민감 문자열은 DPAPI 암호화된 Base64
    private sealed class Dto
    {
        public string Token          { get; set; } = string.Empty;
        public string Chat           { get; set; } = string.Empty;
        public bool   Monitor        { get; set; }
        public bool   First          { get; set; }
        public bool   TelegramOn     { get; set; } = true;
        public bool   KakaoOn        { get; set; }
        public string KakaoKey       { get; set; } = string.Empty;
        public string KakaoAccess    { get; set; } = string.Empty;
        public string KakaoRefresh   { get; set; } = string.Empty;
    }

    internal static MonitorSettings Load()
    {
        var s = new MonitorSettings();
        if (!File.Exists(FilePath)) return s;

        try
        {
            var dto = JsonSerializer.Deserialize<Dto>(File.ReadAllText(FilePath));
            if (dto == null) return s;

            s.BotToken          = Decrypt(dto.Token);
            s.ChatId            = Decrypt(dto.Chat);
            s.MonitorEnabled    = dto.Monitor;
            s.FirstRunDone      = dto.First;
            s.TelegramEnabled   = dto.TelegramOn;
            s.KakaoEnabled      = dto.KakaoOn;
            s.KakaoRestApiKey   = Decrypt(dto.KakaoKey);
            s.KakaoAccessToken  = Decrypt(dto.KakaoAccess);
            s.KakaoRefreshToken = Decrypt(dto.KakaoRefresh);
        }
        catch { /* 파일 손상 시 기본값 사용 */ }

        return s;
    }

    internal void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);

        var dto = new Dto
        {
            Token        = Encrypt(BotToken),
            Chat         = Encrypt(ChatId),
            Monitor      = MonitorEnabled,
            First        = FirstRunDone,
            TelegramOn   = TelegramEnabled,
            KakaoOn      = KakaoEnabled,
            KakaoKey     = Encrypt(KakaoRestApiKey),
            KakaoAccess  = Encrypt(KakaoAccessToken),
            KakaoRefresh = Encrypt(KakaoRefreshToken),
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
