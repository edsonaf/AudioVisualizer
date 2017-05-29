using AudioVisualizer.Infrastructure;
using AudioVisualizer.Utils.RealTimeAudioListener;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using System.ComponentModel.Composition;
using AudioVisualizer.Modules.SpotifyIntegration;

namespace AudioVisualizer.Modules.AudioControl
{
  [ModuleExport(typeof(AudioControlModule), InitializationMode = InitializationMode.WhenAvailable)]
  public class AudioControlModule : IModule
  {
    private readonly IRegionManager _regionManager;

    [ImportingConstructor]
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