using System.Windows.Controls;

namespace AudioVisualizer.Modules.Visualizer.SpectrumVisualizer
{
  /// <summary>
  /// Interaction logic for SpectrumVisualizer.xaml
  /// </summary>
  public partial class SpectrumVisualizerView
  {
    public SpectrumVisualizerView()
    {
      InitializeComponent();
    }

    private SpectrumVisualizerViewModel ViewModel
    {
      set { DataContext = value; }
    }
  }
}