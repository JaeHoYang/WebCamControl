using System.Diagnostics;

namespace WebCamControl;

internal static class StartupManager
{
    private const string TaskName = "WebCamControl";

    internal static bool IsRegistered()
    {
        var code = RunSchtasks($"/query /tn \"{TaskName}\"");
        return code == 0;
    }

    internal static void Register()
    {
        string exe = Application.ExecutablePath;
        RunSchtasks($"/create /tn \"{TaskName}\" /tr \"\\\"{exe}\\\" --minimized\" /sc ONLOGON /rl HIGHEST /f");
    }

    internal static void Unregister()
    {
        RunSchtasks($"/delete /tn \"{TaskName}\" /f");
    }

    private static int RunSchtasks(string args)
    {
        var psi = new ProcessStartInfo("schtasks", args)
        {
            CreateNoWindow  = true,
            UseShellExecute = false,
        };

        using var p = Process.Start(psi);
        p?.WaitForExit();
        return p?.ExitCode ?? -1;
    }
}
