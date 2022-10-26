using SpotifyAPI.Web; //Local.Models;

namespace AudioVisualizer.Modules.SpotifyIntegration
{
  public interface ISpotifyLocal
  {
    FullTrack CurrentTrack { get; set; }
    bool IsRunningLocally { get; }
    void UpdateInfos();
  }
}