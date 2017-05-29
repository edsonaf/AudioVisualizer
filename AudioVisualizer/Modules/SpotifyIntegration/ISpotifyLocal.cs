using SpotifyAPI.Local.Models;

namespace AudioVisualizer.Modules.SpotifyIntegration
{
  public interface ISpotifyLocal
  {
    Track CurrentTrack { get; set; }
    bool IsRunningLocally { get; }
    void UpdateInfos();
  }
}