using System.Collections.Generic;

namespace AudioVisualizer.Controls
{
  public interface IVisualizerBarControl
  {
    void Set(List<byte> spectrumData);
  }
}