using System.Runtime.InteropServices;
using WebCamControl.Interop;

namespace WebCamControl;

/// <summary>
/// Windows Core Audio API를 사용해 마이크를 음소거/해제합니다.
/// </summary>
internal static class MicController
{
    private const int STGM_READ    = 0;
    private const int ACTIVE_STATE = 1;
    private const int CLSCTX_ALL   = 23;
    private const int VT_LPWSTR    = 31;

    private static readonly Guid AudioEndpointVolumeIid =
        new("5CDF2C82-841E-4546-9722-0CF74078229A");

    private static readonly PROPERTYKEY FriendlyNameKey = new()
    {
        FormatId   = new Guid("A45C254E-DF1C-4EFD-8020-67D146A850E0"),
        PropertyId = 14
    };

    /// <summary>현재 활성화된 마이크 장치 이름 목록을 반환합니다.</summary>
    internal static List<string> GetAllMicDeviceNames()
    {
        var names = new List<string>();
        var enumerator = (IMMDeviceEnumerator)new MMDeviceEnumeratorComObject();
        try
        {
            enumerator.EnumAudioEndpoints(EDataFlow.eCapture, ACTIVE_STATE, out var collection);
            collection.GetCount(out int count);

            for (int i = 0; i < count; i++)
            {
                collection.Item(i, out IMMDevice device);
                string? name = GetFriendlyName(device);
                Marshal.ReleaseComObject(device);

                if (!string.IsNullOrEmpty(name))
                    names.Add(name!);
            }

            Marshal.ReleaseComObject(collection);
        }
        finally
        {
            Marshal.ReleaseComObject(enumerator);
        }

        return names;
    }

    /// <summary>지정한 마이크가 현재 음소거 상태인지 반환합니다.</summary>
    internal static bool IsMuted(string deviceName)
    {
        var volume = FindEndpointVolume(deviceName);
        if (volume is null)
            return false;

        try
        {
            volume.GetMute(out bool muted);
            return muted;
        }
        finally
        {
            Marshal.ReleaseComObject(volume);
        }
    }

    /// <summary>지정한 마이크를 음소거하거나 해제합니다.</summary>
    internal static void SetMuted(string deviceName, bool mute)
    {
        var volume = FindEndpointVolume(deviceName);
        if (volume is null)
            return;

        try
        {
            var empty = Guid.Empty;
            volume.SetMute(mute, ref empty);
        }
        finally
        {
            Marshal.ReleaseComObject(volume);
        }
    }

    private static IAudioEndpointVolume? FindEndpointVolume(string deviceName)
    {
        var enumerator = (IMMDeviceEnumerator)new MMDeviceEnumeratorComObject();
        try
        {
            enumerator.EnumAudioEndpoints(EDataFlow.eCapture, ACTIVE_STATE, out var collection);
            collection.GetCount(out int count);

            for (int i = 0; i < count; i++)
            {
                collection.Item(i, out IMMDevice device);
                string? name = GetFriendlyName(device);

                if (!deviceName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    Marshal.ReleaseComObject(device);
                    continue;
                }

                var iid = AudioEndpointVolumeIid;
                device.Activate(ref iid, CLSCTX_ALL, IntPtr.Zero, out object volumeObj);
                Marshal.ReleaseComObject(device);
                Marshal.ReleaseComObject(collection);
                return (IAudioEndpointVolume)volumeObj;
            }

            Marshal.ReleaseComObject(collection);
            return null;
        }
        finally
        {
            Marshal.ReleaseComObject(enumerator);
        }
    }

    private static string? GetFriendlyName(IMMDevice device)
    {
        device.OpenPropertyStore(STGM_READ, out var props);
        try
        {
            var key = FriendlyNameKey;
            props.GetValue(ref key, out PROPVARIANT variant);

            if (variant.vt != VT_LPWSTR || variant.p == IntPtr.Zero)
                return null;

            return Marshal.PtrToStringUni(variant.p);
        }
        finally
        {
            Marshal.ReleaseComObject(props);
        }
    }
}
