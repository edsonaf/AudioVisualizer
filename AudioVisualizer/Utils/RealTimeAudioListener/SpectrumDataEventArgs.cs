using System.Collections.Generic;

namespace AudioVisualizer.Utils.RealTimeAudioListener
{
  public class SpectrumDataEventArgs
  {
    public List<byte> SpectrumData { get; }

    public SpectrumDataEventArgs(List<byte> spectrumData)
    {
      SpectrumData = spectrumData;
    }
  }
}