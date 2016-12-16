using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AudioVisualizer.Modules.Visualizer.SpectrumVisualizer
{
  /// <summary>
  /// Interaction logic for SpectrumVisualizer.xaml
  /// </summary>
  [Export]
  public partial class SpectrumVisualizerView
  {
    public SpectrumVisualizerView()
    {
      InitializeComponent();
    }

    [Import]
    private SpectrumVisualizerViewModel ViewModel
    {
      set { DataContext = value; }
    }
  }
}