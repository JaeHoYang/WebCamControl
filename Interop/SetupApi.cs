using System.Runtime.InteropServices;
using System.Text;

namespace WebCamControl.Interop;

/// <summary>
/// SetupAPI 및 CfgMgr32 P/Invoke 선언.
/// 장치 열거, 상태 조회, 활성화/비활성화에 사용됩니다.
/// </summary>
internal static class SetupApi
{
    internal const int DIGCF_PRESENT     = 0x00000002;
    internal const int DIGCF_ALLCLASSES  = 0x00000004;
    internal const int DICS_ENABLE       = 1;
    internal const int DICS_DISABLE      = 2;
    internal const int DICS_FLAG_GLOBAL  = 0x00000001;
    internal const int DIF_PROPERTYCHANGE = 0x00000012;
    internal const int SPDRP_DEVICEDESC  = 0x00000000;
    internal const int SPDRP_FRIENDLYNAME = 0x0000000C;
    internal const int CM_PROB_DISABLED  = 22;

    internal static readonly IntPtr INVALID_HANDLE_VALUE = new(-1);

    [StructLayout(LayoutKind.Sequential)]
    internal struct SP_DEVINFO_DATA
    {
        public int    cbSize;
        public Guid   ClassGuid;
        public uint   DevInst;
        public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SP_CLASSINSTALL_HEADER
    {
        public int cbSize;
        public int InstallFunction;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SP_PROPCHANGE_PARAMS
    {
        public SP_CLASSINSTALL_HEADER ClassInstallHeader;
        public int StateChange;
        public int Scope;
        public int HwProfile;
    }

    // DIGCF_ALLCLASSES 용 (classGuid = IntPtr.Zero)
    [DllImport("setupapi.dll", EntryPoint = "SetupDiGetClassDevsW",
               SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern IntPtr SetupDiGetClassDevs(
        IntPtr classGuid,
        string? enumerator,
        IntPtr hwndParent,
        int flags);

    // 특정 클래스 GUID 검색 용
    [DllImport("setupapi.dll", EntryPoint = "SetupDiGetClassDevsW",
               SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern IntPtr SetupDiGetClassDevsByGuid(
        ref Guid classGuid,
        string? enumerator,
        IntPtr hwndParent,
        int flags);

    [DllImport("setupapi.dll", SetLastError = true)]
    internal static extern bool SetupDiEnumDeviceInfo(
        IntPtr deviceInfoSet,
        int memberIndex,
        ref SP_DEVINFO_DATA deviceInfoData);

    [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern bool SetupDiGetDeviceInstanceId(
        IntPtr deviceInfoSet,
        ref SP_DEVINFO_DATA deviceInfoData,
        StringBuilder deviceInstanceId,
        int deviceInstanceIdSize,
        out int requiredSize);

    [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern bool SetupDiGetDeviceRegistryProperty(
        IntPtr deviceInfoSet,
        ref SP_DEVINFO_DATA deviceInfoData,
        int property,
        out int propertyRegDataType,
        StringBuilder propertyBuffer,
        int propertyBufferSize,
        out int requiredSize);

    [DllImport("setupapi.dll", SetLastError = true)]
    internal static extern bool SetupDiSetClassInstallParams(
        IntPtr deviceInfoSet,
        ref SP_DEVINFO_DATA deviceInfoData,
        ref SP_PROPCHANGE_PARAMS classInstallParams,
        int classInstallParamsSize);

    [DllImport("setupapi.dll", SetLastError = true)]
    internal static extern bool SetupDiCallClassInstaller(
        int installFunction,
        IntPtr deviceInfoSet,
        ref SP_DEVINFO_DATA deviceInfoData);

    [DllImport("setupapi.dll", SetLastError = true)]
    internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

    [DllImport("cfgmgr32.dll")]
    internal static extern int CM_Get_DevNode_Status(
        out int status,
        out int problemNumber,
        uint devInst,
        int flags);
}
