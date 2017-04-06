using System.Collections.Generic;

namespace AudioVisualizer.Modules.GameSenseControl
{
  public interface IGameSenseModule
  {
    /// <summary>
    /// Looks for the coreProps.json of SteelSeries containing the local address of the keyboard and saves it.
    /// </summary>
    /// <returns>Returns true if address found</returns>
    bool InitializeGameSenseConnection();

    void SendInfoToGameSense(List<byte> data);
  }
}
