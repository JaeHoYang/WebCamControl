using System.Diagnostics;
using System.Security.Principal;

namespace WebCamControl;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // 인스톨러: 시작 프로그램 등록
        if (args.Contains("--register-startup"))
        {
            StartupManager.Register();
            return;
        }

        // 인스톨러: 신규 설치 또는 업데이트 후 해시 갱신
        if (args.Contains("--update-hash"))
        {
            FileIntegrityChecker.UpdateHash();
            return;
        }

        ApplicationConfiguration.Initialize();

        if (!IsRunningAsAdministrator())
        {
            RequestElevation();
            return;
        }

        // 파일 무결성 검증
        if (!FileIntegrityChecker.Verify())
        {
            HandleTampering();
            return;
        }

        bool startMinimized = args.Contains("--minimized");
        Application.Run(new MainForm(startMinimized));
    }

    private static void HandleTampering()
    {
        // 텔레그램 알림 (설정이 있을 경우)
        var settings = MonitorSettings.Load();
        if (!string.IsNullOrEmpty(settings.BotToken) && !string.IsNullOrEmpty(settings.ChatId))
        {
            using var notifier = new TelegramNotifier
            {
                BotToken = settings.BotToken,
                ChatId   = settings.ChatId
            };
            notifier.SendAsync(
                "🚨 WebCamControl 실행 파일 변조 감지!\n" +
                "즉시 확인이 필요합니다.").GetAwaiter().GetResult();
        }

        MessageBox.Show(
            "실행 파일이 변조된 것으로 감지되었습니다.\n" +
            "보안상 프로그램을 종료합니다.\n\n" +
            "정상적인 파일로 재설치 후 실행하세요.",
            "🚨 보안 경고",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private static bool IsRunningAsAdministrator()
    {
        using var identity = WindowsIdentity.GetCurrent();
        return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
    }

    private static void RequestElevation()
    {
        var answer = MessageBox.Show(
            "카메라 장치를 제어하려면 관리자 권한이 필요합니다.\n관리자 권한으로 다시 시작하시겠습니까?",
            "관리자 권한 필요",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (answer != DialogResult.Yes)
            return;

        string? exePath = Environment.ProcessPath;
        if (string.IsNullOrEmpty(exePath) ||
            !exePath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(
                "빌드된 .exe 파일을 마우스 오른쪽 버튼 → '관리자 권한으로 실행'으로 시작해주세요.",
                "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName        = exePath,
                UseShellExecute = true,
                Verb            = "runas"
            });
        }
        catch
        {
            // 사용자가 UAC 프롬프트에서 취소한 경우
        }
    }
}
