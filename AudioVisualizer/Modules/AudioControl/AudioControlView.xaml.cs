namespace AudioVisualizer.Modules.AudioControl
{
  /// <summary>
  /// Interaction logic for AudioControlView.xaml
  /// </summary>
  public partial class AudioControlView
  {
    public AudioControlView()
    {
      InitializeComponent();
    }

    private AudioControlViewModel ViewModel
    {
      set { DataContext = value; }
    }
  }
}