using System.Diagnostics;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace RealTimeAudioListener;

/// <summary>
/// https://stackoverflow.com/questions/6163119/handling-changed-audio-device-event-in-c-sharp
/// </summary>
public class NotificationClientImplementation : IMMNotificationClient
{
    public void OnDefaultDeviceChanged(DataFlow dataFlow, Role deviceRole, string defaultDeviceId)
    {
        //Do some Work
        Debug.WriteLine("OnDefaultDeviceChanged --> {0}", dataFlow.ToString());
    }

    void IMMNotificationClient.OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
    {
        OnPropertyValueChanged(pwstrDeviceId, key);
    }

    // public void OnDeviceStateChanged(string deviceId, DeviceState newState)
    // {
    //     throw new NotImplementedException();
    // }

    public void OnDeviceAdded(string deviceId)
    {
        //Do some Work
        Debug.WriteLine($"OnDeviceAdded --> {deviceId}");
    }

    public void OnDeviceRemoved(string deviceId)
    {

        Debug.WriteLine($"OnDeviceRemoved --> {deviceId}");
        //Do some Work
    }

    void IMMNotificationClient.OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
    {
        OnDefaultDeviceChanged(flow, role, defaultDeviceId);
    }

    public void OnDeviceStateChanged(string deviceId, DeviceState newState)
    {
        Debug.WriteLine("OnDeviceStateChanged\n Device Id -->{0} : Device State {1}", deviceId, newState);
        //Do some Work
    }

    public NotificationClientImplementation()
    {
        //_realEnumerator.RegisterEndpointNotificationCallback();
        if (System.Environment.OSVersion.Version.Major < 6)
        {
            throw new NotSupportedException("This functionality is only supported on Windows Vista or newer.");
        }
    }

    public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
    {
        //Do some Work
        //fmtid & pid are changed to formatId and propertyId in the latest version NAudio
        Debug.WriteLine("OnPropertyValueChanged: formatId --> {0}  propertyId --> {1}", propertyKey.formatId.ToString(), propertyKey.propertyId.ToString());
    }

}