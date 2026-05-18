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
    internal bool   RecordScreenEnabled     { get; set; } = false;
    internal int    RecordMonitorIndex      { get; set; } = 0;
    internal int    RecordScreenQuality     { get; set; } = 1;
    internal bool   KakaoEnabled            { get; set; } = false;
    internal string KakaoAppKey             { get; set; } = string.Empty;
    internal string KakaoAccessToken        { get; set; } = string.Empty;
    internal string KakaoRefreshToken       { get; set; } = string.Empty;

    // 번역/자막
    internal bool   TranslationEnabled      { get; set; } = false;
    internal bool   DeepLTranslationEnabled { get; set; } = true;
    internal List<VoskModelEntry> VoskModels      { get; set; } = new();
    internal int                  ActiveVoskModelIndex { get; set; } = 0;
    internal VoskModelEntry? ActiveVoskModel =>
        VoskModels.Count > 0 && ActiveVoskModelIndex < VoskModels.Count
            ? VoskModels[ActiveVoskModelIndex] : null;
    internal string DeepLApiKey             { get; set; } = string.Empty;
    internal string TranslationSourceLang   { get; set; } = "auto";
    internal string TranslationTargetLang   { get; set; } = "KO";
    internal bool   ShowOriginalText        { get; set; } = true;
    internal bool   LogTranslation          { get; set; } = true;
    internal int    OverlayX                { get; set; } = 100;
    internal int    OverlayY                { get; set; } = 800;
    internal int    OverlayWidth            { get; set; } = 600;
    internal int    OverlayHeight           { get; set; } = 200;

    // 마이크 감지
    internal bool  MicMonitorEnabled   { get; set; } = false;

    // 오버레이 커스터마이징
    internal float  OverlayFontSize     { get; set; } = 11F;
    internal double OverlayOpacity      { get; set; } = 0.85;
    internal int    OverlayBgColorArgb  { get; set; } = -15461356; // Color.FromArgb(20,20,20)

    // 저장 공간 경고
    internal bool StorageWarningEnabled { get; set; } = true;
    internal int  LogSizeWarningMB      { get; set; } = 100;
    internal int  RecordSizeWarningMB   { get; set; } = 5120;  // 5 GB

    // 데이터 저장 루트 — 비어 있으면 %LocalAppData%\WebCamControl 사용
    internal string DataRootParent { get; set; } = string.Empty;

    internal string EffectiveDataRoot =>
        string.IsNullOrEmpty(DataRootParent)
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebCamControl")
            : Path.Combine(DataRootParent, "WebCamData");

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
        public bool   RecordScreen   { get; set; }
        public int    RecordMonitor  { get; set; }
        public int    RecordScreenQ  { get; set; } = 1;
        public bool   KakaoOn        { get; set; }
        public string KakaoKey       { get; set; } = string.Empty;
        public string KakaoAccess    { get; set; } = string.Empty;
        public string KakaoRefresh   { get; set; } = string.Empty;
        // 번역/자막
        public bool   TransOn        { get; set; }
        public bool   DeepLOn        { get; set; } = true;
        public string VoskPath       { get; set; } = string.Empty;  // 이전 버전 마이그레이션용
        public List<ModelEntryDto> VoskList { get; set; } = new();
        public int    VoskIdx        { get; set; } = 0;
        public string DeepLKey       { get; set; } = string.Empty;

        public sealed class ModelEntryDto
        {
            public string N { get; set; } = string.Empty;
            public string P { get; set; } = string.Empty;
        }
        public string SrcLang        { get; set; } = "auto";
        public string TgtLang        { get; set; } = "KO";
        public bool   ShowOrig       { get; set; } = true;
        public bool   LogTrans       { get; set; } = true;
        public int    OvX            { get; set; } = 100;
        public int    OvY            { get; set; } = 800;
        public int    OvW            { get; set; } = 600;
        public int    OvH            { get; set; } = 200;
        // 마이크 감지
        public bool   MicOn      { get; set; }
        // 오버레이
        public float  OvFont     { get; set; } = 11F;
        public double OvAlpha    { get; set; } = 0.85;
        public int    OvBg       { get; set; } = -15461356;
        // 저장 공간 경고
        public bool   StoreWarn  { get; set; } = true;
        public int    LogWarnMB  { get; set; } = 100;
        public int    RecWarnMB  { get; set; } = 5120;
        // 데이터 저장 위치
        public string DataRoot   { get; set; } = string.Empty;
    }

    internal static MonitorSettings Load()
    {
        var s = new MonitorSettings();

        if (File.Exists(FilePath))
        {
            try
            {
                var dto = JsonSerializer.Deserialize<Dto>(File.ReadAllText(FilePath));
                if (dto != null)
                {
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
                    s.RecordScreenEnabled    = dto.RecordScreen;
                    s.RecordMonitorIndex     = dto.RecordMonitor;
                    s.RecordScreenQuality    = dto.RecordScreenQ;
                    s.KakaoEnabled           = dto.KakaoOn;
                    s.KakaoAppKey            = Decrypt(dto.KakaoKey);
                    s.KakaoAccessToken       = Decrypt(dto.KakaoAccess);
                    s.KakaoRefreshToken      = Decrypt(dto.KakaoRefresh);
                    s.TranslationEnabled      = dto.TransOn;
                    s.DeepLTranslationEnabled = dto.DeepLOn;
                    s.VoskModels = dto.VoskList
                        .Select(e => new VoskModelEntry { Name = e.N, Path = e.P })
                        .ToList();
                    if (s.VoskModels.Count == 0 && !string.IsNullOrEmpty(dto.VoskPath))
                        s.VoskModels.Add(new VoskModelEntry { Name = "기본", Path = dto.VoskPath });
                    s.ActiveVoskModelIndex    = Math.Clamp(dto.VoskIdx, 0, Math.Max(0, s.VoskModels.Count - 1));
                    s.DeepLApiKey             = Decrypt(dto.DeepLKey);
                    s.TranslationSourceLang   = dto.SrcLang;
                    s.TranslationTargetLang   = dto.TgtLang;
                    s.ShowOriginalText        = dto.ShowOrig;
                    s.LogTranslation          = dto.LogTrans;
                    s.OverlayX                = dto.OvX;
                    s.OverlayY                = dto.OvY;
                    s.OverlayWidth            = dto.OvW;
                    s.OverlayHeight           = dto.OvH;
                    s.MicMonitorEnabled       = dto.MicOn;
                    s.OverlayFontSize         = Math.Clamp(dto.OvFont, 8F, 24F);
                    s.OverlayOpacity          = Math.Clamp(dto.OvAlpha, 0.2, 1.0);
                    s.OverlayBgColorArgb      = dto.OvBg;
                    s.StorageWarningEnabled   = dto.StoreWarn;
                    s.LogSizeWarningMB        = Math.Max(1, dto.LogWarnMB);
                    s.RecordSizeWarningMB     = Math.Max(1, dto.RecWarnMB);
                    s.DataRootParent          = dto.DataRoot;
                }
            }
            catch { /* 파일 손상 시 기본값 사용 */ }
        }

        // 설치 시 Inno Setup이 기록한 부트스트랩 파일 — settings.json 존재 여부와 무관하게 항상 확인
        string bootstrapFile = Path.Combine(Path.GetDirectoryName(FilePath)!, "dataroot_bootstrap.txt");
        if (File.Exists(bootstrapFile))
        {
            try
            {
                string root = File.ReadAllText(bootstrapFile, Encoding.UTF8).Trim();
                File.Delete(bootstrapFile);
                if (!string.IsNullOrEmpty(root))
                {
                    s.DataRootParent = root;
                    s.Save();
                }
            }
            catch { }
        }

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
            RecordScreen  = RecordScreenEnabled,
            RecordMonitor = RecordMonitorIndex,
            RecordScreenQ = RecordScreenQuality,
            KakaoOn       = KakaoEnabled,
            KakaoKey    = Encrypt(KakaoAppKey),
            KakaoAccess = Encrypt(KakaoAccessToken),
            KakaoRefresh= Encrypt(KakaoRefreshToken),
            TransOn  = TranslationEnabled,
            DeepLOn  = DeepLTranslationEnabled,
            VoskList = VoskModels.Select(m => new Dto.ModelEntryDto { N = m.Name, P = m.Path }).ToList(),
            VoskIdx  = ActiveVoskModelIndex,
            DeepLKey = Encrypt(DeepLApiKey),
            SrcLang   = TranslationSourceLang,
            TgtLang   = TranslationTargetLang,
            ShowOrig  = ShowOriginalText,
            LogTrans  = LogTranslation,
            OvX       = OverlayX,
            OvY       = OverlayY,
            OvW       = OverlayWidth,
            OvH       = OverlayHeight,
            MicOn    = MicMonitorEnabled,
            OvFont   = OverlayFontSize,
            OvAlpha  = OverlayOpacity,
            OvBg     = OverlayBgColorArgb,
            StoreWarn = StorageWarningEnabled,
            LogWarnMB = LogSizeWarningMB,
            RecWarnMB = RecordSizeWarningMB,
            DataRoot  = DataRootParent,
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
