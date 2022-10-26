using AudioVisualizer.Infrastructure;
using AudioVisualizer.Utils.RealTimeAudioListener;
using Prism.Regions;
using AudioVisualizer.Modules.SpotifyIntegration;

namespace AudioVisualizer.Modules.AudioControl
{
  public class AudioControlModule
  {
    private readonly IRegionManager _regionManager;

    public AudioControlModule(IRegionManager regionManager, IRealTimeAudioListener audioListener, ISpotifyLocal localSpotify)
    {
      _regionManager = regionManager;
    }

    public void Initialize()
    {
      _regionManager.RegisterViewWithRegion(RegionNames.AudioControlRegion, typeof(AudioControlView));
    }
  }
}