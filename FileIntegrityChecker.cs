using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace WebCamControl;

internal static class FileIntegrityChecker
{
    private const string RegistryPath = @"SOFTWARE\WebCamControl";
    private const string ValueName    = "FileHash";

    /// <summary>
    /// 현재 실행 파일의 SHA-256 해시를 레지스트리 저장값과 비교합니다.
    /// 최초 실행이면 해시를 저장하고 true를 반환합니다.
    /// </summary>
    internal static bool Verify()
    {
        string current = ComputeHash(Application.ExecutablePath);
        string? stored = LoadHash();

        if (stored == null)
        {
            // 최초 실행 또는 해시 미등록 — 현재 해시를 신뢰하고 저장
            SaveHash(current);
            return true;
        }

        return string.Equals(current, stored, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>현재 실행 파일의 해시를 레지스트리에 새로 저장합니다. (업데이트 후 호출)</summary>
    internal static void UpdateHash()
    {
        SaveHash(ComputeHash(Application.ExecutablePath));
    }

    private static string ComputeHash(string path)
    {
        using var sha = SHA256.Create();
        using var fs  = File.OpenRead(path);
        return Convert.ToHexString(sha.ComputeHash(fs));
    }

    private static void SaveHash(string hash)
    {
        // DPAPI — LocalMachine 범위: 이 PC의 모든 프로세스가 복호화 가능
        // 단, HKLM 쓰기는 관리자 권한 필요 → 무단 변경 차단
        byte[] encrypted = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(hash), null, DataProtectionScope.LocalMachine);

        using var key = Registry.LocalMachine.CreateSubKey(RegistryPath);
        key?.SetValue(ValueName, Convert.ToBase64String(encrypted), RegistryValueKind.String);
    }

    private static string? LoadHash()
    {
        using var key = Registry.LocalMachine.OpenSubKey(RegistryPath);
        if (key?.GetValue(ValueName) is not string stored || string.IsNullOrEmpty(stored))
            return null;

        try
        {
            byte[] decrypted = ProtectedData.Unprotect(
                Convert.FromBase64String(stored), null, DataProtectionScope.LocalMachine);
            return Encoding.UTF8.GetString(decrypted);
        }
        catch { return null; }
    }
}
