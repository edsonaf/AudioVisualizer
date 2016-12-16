using AudioVisualizer.Infrastructure;
using AudioVisualizer.Modules.Visualizer.SpectrumVisualizer;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using System.ComponentModel.Composition;

namespace AudioVisualizer.Modules.Visualizer
{
  [ModuleExport(typeof(VisualizerModule), InitializationMode = InitializationMode.WhenAvailable)]
  public class VisualizerModule : IModule
  {
    private readonly IRegionManager _regionManager;

    [ImportingConstructor]
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