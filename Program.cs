using System.Diagnostics;
using System.Security.Principal;

namespace WebCamControl;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Contains("--register-startup"))
        {
            StartupManager.Register();
            return;
        }

        if (args.Contains("--update-hash"))
        {
            FileIntegrityChecker.UpdateHash();
            return;
        }

        ApplicationConfiguration.Initialize();

        using var self = Process.GetCurrentProcess();
        var existing = Process.GetProcessesByName(self.ProcessName)
            .FirstOrDefault(p => p.Id != self.Id && !p.HasExited);
        if (existing != null)
        {
            var ans = MessageBox.Show(
                "WebCam Controller가 이미 실행 중입니다.\n실행 중인 프로세스를 종료하고 새로 시작하시겠습니까?",
                "이미 실행 중", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ans == DialogResult.Yes)
            { try { existing.Kill(); existing.WaitForExit(3000); } catch { } }
            else return;
        }

        if (!IsRunningAsAdministrator())
        {
            RequestElevation();
            return;
        }

#if !DEBUG
        if (!FileIntegrityChecker.Verify())
        {
            HandleTampering();
            return;
        }
#endif

        bool startMinimized = args.Contains("--minimized");
        Application.Run(new MainForm(startMinimized));
    }

    private static void HandleTampering()
    {
        var settings = MonitorSettings.Load();
        string message =
            "🚨 WebCamControl 실행 파일 변조 감지!\n" +
            "즉시 확인이 필요합니다.";

        if (!string.IsNullOrEmpty(settings.BotToken) && !string.IsNullOrEmpty(settings.ChatId)
            && settings.TelegramEnabled)
        {
            using var telegram = new TelegramNotifier
            {
                BotToken = settings.BotToken,
                ChatId   = settings.ChatId
            };
            telegram.SendAsync(message).GetAwaiter().GetResult();
        }

        if (settings.DiscordEnabled && !string.IsNullOrWhiteSpace(settings.DiscordWebhookUrl))
        {
            using var discord = new DiscordNotifier { WebhookUrl = settings.DiscordWebhookUrl };
            discord.SendAsync(message).GetAwaiter().GetResult();
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
