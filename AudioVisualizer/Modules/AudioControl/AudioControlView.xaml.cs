using AudioVisualizer.Infrastructure;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AudioVisualizer.Modules.AudioControl
{
  /// <summary>
  /// Interaction logic for AudioControlView.xaml
  /// </summary>
  [Export]
  public partial class AudioControlView : UserControl
  {
    public AudioControlView()
    {
      InitializeComponent();
    }

    [Import]
    private AudioControlViewModel ViewModel
    {
      set
      {
        DataContext = value;
      }
    }

  }
}