using NAudio.CoreAudioApi;
using System.Collections.Generic;

namespace AudioVisualizer.Utils.RealTimeAudioListener
{
  public interface IRealTimeAudioListener
  {
    List<MMDevice> CaptureDevices { get; }
    MMDevice SelectedDevice { get; set; }
  }
}