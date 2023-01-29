using AudioVisualizer.Infrastructure;
using Prism.Regions;
using Prism.Ioc;
using Prism.Modularity;

namespace AudioVisualizer.Modules.AudioControl
{
  public class AudioControlModule : IModule
  {
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
      var regionManager = containerProvider.Resolve<IRegionManager>();
      regionManager.RegisterViewWithRegion(RegionNames.AudioControlRegion, typeof(Views.AudioControl));
    }
  }
}