using AudioVisualizer.Infrastructure;
using AudioVisualizer.Modules.Visualizer.SpectrumVisualizer;
using Prism.Regions;

namespace AudioVisualizer.Modules.Visualizer
{
  public class VisualizerModule
  {
    private readonly IRegionManager _regionManager;
    
    public VisualizerModule(IRegionManager regionManager)
    {
      _regionManager = regionManager;
    }

    public void Initialize()
    {
      _regionManager.RegisterViewWithRegion(RegionNames.VisualizerRegion, typeof(SpectrumVisualizerView));
      //_regionManager.RegisterViewWithRegion(RegionNames.VisualizerRegion, typeof(FastFourierTransformVisualizerView));
    }
  }
}