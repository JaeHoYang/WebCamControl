using System.Runtime.InteropServices;
using System.Text;
using WebCamControl.Interop;

namespace WebCamControl;

internal enum CameraSetResult { Success, DeviceNotFound, SetParamsFailed, InstallerFailed }

/// <summary>
/// SetupAPI를 사용해 카메라 장치를 활성화/비활성화합니다.
/// 관리자 권한이 필요합니다.
/// </summary>
internal static class CameraController
{
    private static Guid ImageClassGuid  = new("6bdd1fc6-810f-11d0-bec7-08002be2092f");
    private static Guid CameraClassGuid = new("ca3e7ab9-b4c3-4ae6-8251-579ef933890f");

    /// <summary>현재 연결된 카메라 장치 이름 목록을 반환합니다.</summary>
    internal static List<string> GetAllCameraDeviceNames()
    {
        var names = new List<string>();

        foreach (var guid in ClassGuidsToSearch())
        {
            var classGuid = guid;
            IntPtr hDevInfo = SetupApi.SetupDiGetClassDevsByGuid(
                ref classGuid, null, IntPtr.Zero, SetupApi.DIGCF_PRESENT);

            if (hDevInfo == SetupApi.INVALID_HANDLE_VALUE)
                continue;

            try
            {
                var devInfo = NewDevInfoData();
                for (int i = 0; SetupApi.SetupDiEnumDeviceInfo(hDevInfo, i, ref devInfo); i++)
                {
                    string name = GetDeviceName(hDevInfo, ref devInfo);
                    if (!string.IsNullOrEmpty(name) && !names.Contains(name))
                        names.Add(name);
                }
            }
            finally
            {
                SetupApi.SetupDiDestroyDeviceInfoList(hDevInfo);
            }
        }

        return names;
    }

    /// <summary>지정한 카메라 장치가 현재 활성화 상태인지 반환합니다.</summary>
    internal static bool IsEnabled(string deviceName)
    {
        foreach (var guid in ClassGuidsToSearch())
        {
            var classGuid = guid;
            IntPtr hDevInfo = SetupApi.SetupDiGetClassDevsByGuid(
                ref classGuid, null, IntPtr.Zero, SetupApi.DIGCF_PRESENT);

            if (hDevInfo == SetupApi.INVALID_HANDLE_VALUE)
                continue;

            try
            {
                var devInfo = NewDevInfoData();
                for (int i = 0; SetupApi.SetupDiEnumDeviceInfo(hDevInfo, i, ref devInfo); i++)
                {
                    if (!IsTargetDevice(hDevInfo, ref devInfo, deviceName))
                        continue;

                    SetupApi.CM_Get_DevNode_Status(out _, out int problem, devInfo.DevInst, 0);
                    return problem != SetupApi.CM_PROB_DISABLED;
                }
            }
            finally
            {
                SetupApi.SetupDiDestroyDeviceInfoList(hDevInfo);
            }
        }

        return true;
    }

    /// <summary>지정한 카메라 장치를 활성화하거나 비활성화합니다.</summary>
    internal static (CameraSetResult result, int win32Error, string deviceName) SetEnabled(
        string deviceName, bool enable)
    {
        foreach (var guid in ClassGuidsToSearch())
        {
            var classGuid = guid;
            IntPtr hDevInfo = SetupApi.SetupDiGetClassDevsByGuid(
                ref classGuid, null, IntPtr.Zero, SetupApi.DIGCF_PRESENT);

            if (hDevInfo == SetupApi.INVALID_HANDLE_VALUE)
                continue;

            try
            {
                var devInfo = NewDevInfoData();
                for (int i = 0; SetupApi.SetupDiEnumDeviceInfo(hDevInfo, i, ref devInfo); i++)
                {
                    if (!IsTargetDevice(hDevInfo, ref devInfo, deviceName))
                        continue;

                    return ApplyStateChange(hDevInfo, ref devInfo, enable, deviceName);
                }
            }
            finally
            {
                SetupApi.SetupDiDestroyDeviceInfoList(hDevInfo);
            }
        }

        return (CameraSetResult.DeviceNotFound, 0, deviceName);
    }

    private static Guid[] ClassGuidsToSearch() => [ImageClassGuid, CameraClassGuid];

    private static SetupApi.SP_DEVINFO_DATA NewDevInfoData() => new()
    {
        cbSize = Marshal.SizeOf<SetupApi.SP_DEVINFO_DATA>()
    };

    private static bool IsTargetDevice(
        IntPtr hDevInfo, ref SetupApi.SP_DEVINFO_DATA devInfo, string targetName)
    {
        string name = GetDeviceName(hDevInfo, ref devInfo);
        return name.Equals(targetName, StringComparison.OrdinalIgnoreCase);
    }

    private static string GetDeviceName(IntPtr hDevInfo, ref SetupApi.SP_DEVINFO_DATA devInfo)
    {
        var buf = new StringBuilder(256);
        if (SetupApi.SetupDiGetDeviceRegistryProperty(
                hDevInfo, ref devInfo, SetupApi.SPDRP_FRIENDLYNAME,
                out _, buf, 256, out _))
            return buf.ToString();

        buf.Clear();
        SetupApi.SetupDiGetDeviceRegistryProperty(
            hDevInfo, ref devInfo, SetupApi.SPDRP_DEVICEDESC,
            out _, buf, 256, out _);
        return buf.ToString();
    }

    private static (CameraSetResult, int, string) ApplyStateChange(
        IntPtr hDevInfo, ref SetupApi.SP_DEVINFO_DATA devInfo, bool enable, string deviceName)
    {
        var propChange = new SetupApi.SP_PROPCHANGE_PARAMS
        {
            ClassInstallHeader = new SetupApi.SP_CLASSINSTALL_HEADER
            {
                cbSize          = Marshal.SizeOf<SetupApi.SP_CLASSINSTALL_HEADER>(),
                InstallFunction = SetupApi.DIF_PROPERTYCHANGE
            },
            StateChange = enable ? SetupApi.DICS_ENABLE : SetupApi.DICS_DISABLE,
            Scope       = SetupApi.DICS_FLAG_GLOBAL,
            HwProfile   = 0
        };

        if (!SetupApi.SetupDiSetClassInstallParams(
                hDevInfo, ref devInfo,
                ref propChange, Marshal.SizeOf<SetupApi.SP_PROPCHANGE_PARAMS>()))
            return (CameraSetResult.SetParamsFailed, Marshal.GetLastWin32Error(), deviceName);

        if (!SetupApi.SetupDiCallClassInstaller(
                SetupApi.DIF_PROPERTYCHANGE, hDevInfo, ref devInfo))
            return (CameraSetResult.InstallerFailed, Marshal.GetLastWin32Error(), deviceName);

        return (CameraSetResult.Success, 0, deviceName);
    }
}
