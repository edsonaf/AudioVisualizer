using System.ComponentModel.Composition;

namespace AudioVisualizer.Modules.AudioControl
{
  /// <summary>
  /// Interaction logic for AudioControlView.xaml
  /// </summary>
  [Export]
  public partial class AudioControlView
  {
    public AudioControlView()
    {
      InitializeComponent();
    }

    [Import]
    private AudioControlViewModel ViewModel
    {
      set { DataContext = value; }
    }
  }
}