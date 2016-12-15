using NAudio.CoreAudioApi;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace AudioVisualizer.Utils.RealTimeAudioListener
{
  [PartCreationPolicy(CreationPolicy.Shared)] // Singleton
  [Export(typeof(IRealTimeAudioListener))]
  public class RealTimeAudioListener : IRealTimeAudioListener
  {
    public RealTimeAudioListener()
    {
      var enumerator = new MMDeviceEnumerator();
      CaptureDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
    }

    public List<MMDevice> CaptureDevices { get; set; }

    public MMDevice SelectedDevice { get; set; }
  }
}