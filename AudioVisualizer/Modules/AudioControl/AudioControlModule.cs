using AudioVisualizer.Infrastructure;
using AudioVisualizer.Utils.RealTimeAudioListener;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using System.ComponentModel.Composition;

namespace AudioVisualizer.Modules.AudioControl
{
  [ModuleExport(typeof(AudioControlModule), InitializationMode = InitializationMode.WhenAvailable)]
  public class AudioControlModule : IModule
  {
    private readonly IRegionManager _regionManager;
    private readonly IRealTimeAudioListener _realTimeAudioListener;

    [ImportingConstructor]
    public AudioControlModule(IRegionManager regionManager, IRealTimeAudioListener audioListener)
    {
      _regionManager = regionManager;
      _realTimeAudioListener = audioListener;
    }

    public void Initialize()
    {
      _regionManager.RegisterViewWithRegion(RegionNames.AudioControlRegion, typeof(AudioControlView));
    }
  }
}