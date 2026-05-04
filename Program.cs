using System.Diagnostics;
using System.Security.Principal;

namespace WebCamControl;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // 인스톨러에서 시작 프로그램 등록 요청 시 조용히 등록 후 종료
        if (args.Contains("--register-startup"))
        {
            StartupManager.Register();
            return;
        }

        ApplicationConfiguration.Initialize();

        if (!IsRunningAsAdministrator())
        {
            RequestElevation();
            return;
        }

        bool startMinimized = args.Contains("--minimized");
        Application.Run(new MainForm(startMinimized));
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
            // dotnet run 으로 실행된 경우 — 빌드된 exe가 없어 runas 불가
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
                Verb            = "runas"   // UAC 다이얼로그 표시
            });
        }
        catch (Exception)
        {
            // 사용자가 UAC 프롬프트에서 취소한 경우 — 아무것도 하지 않음
        }
    }
}
