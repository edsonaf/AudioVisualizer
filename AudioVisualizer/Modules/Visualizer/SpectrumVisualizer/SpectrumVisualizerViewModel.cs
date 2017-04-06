using AudioVisualizer.Controls;
using AudioVisualizer.Modules.AudioControl;
using AudioVisualizer.Modules.GameSenseControl;
using AudioVisualizer.Utils.RealTimeAudioListener;
using AudioVisualizer.Utils.SystemColorRetriever;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace AudioVisualizer.Modules.Visualizer.SpectrumVisualizer
{
  [Export(typeof(SpectrumVisualizerViewModel))]
  public class SpectrumVisualizerViewModel : BindableBase
  {
    private readonly IEventAggregator _eventAggregator;
    private readonly IRealTimeAudioListener _realTimeAudioListener;
    private readonly IGameSenseModule _gameSenseModule;

    [ImportingConstructor]
    public SpectrumVisualizerViewModel(IEventAggregator aggregator, IRealTimeAudioListener audioListener, IGameSenseModule gameSenseModule)
    {
      if (aggregator == null) throw new ArgumentNullException();
      if (audioListener == null) throw new ArgumentNullException();
      if (gameSenseModule == null) throw new ArgumentNullException();

      SpectrumBarControl = new VisualizerBarControl();

      _eventAggregator = aggregator;
      _eventAggregator.GetEvent<StartVisualizerEvent>().Subscribe(OnStartVisualizerReceived);

      _realTimeAudioListener = audioListener;
      _realTimeAudioListener.SpectrumDataReceived += OnSpectrumDataReceived;

      _gameSenseModule = gameSenseModule;
      GameSenseAvailable = _gameSenseModule.InitializeGameSenseConnection();
    }

    public Brush ThemeColor
    {
      get
      {
        Color color = SystemColorRetriever.GetSystemColor();
        return new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B));
      }
    }

    public bool GameSenseAvailable { get; private set; }

    public bool SendToGameSenseChecked { get; set; }

    public VisualizerBarControl SpectrumBarControl { get; }

    private void OnStartVisualizerReceived(bool start)
    {
      if (start)
        _realTimeAudioListener.Start();
      else
        _realTimeAudioListener.Stop();
    }

    private void OnSpectrumDataReceived(object sender, SpectrumDataEventArgs e)
    {
      // send to spectrum bar control
      SpectrumBarControl.Set(e.SpectrumData);

      // send to Keyboard
      if (SendToGameSenseChecked)
        _gameSenseModule.SendInfoToGameSense(e.SpectrumData);
    }

    ~SpectrumVisualizerViewModel()
    {
      _eventAggregator.GetEvent<StartVisualizerEvent>().Unsubscribe(OnStartVisualizerReceived);

      _realTimeAudioListener.Stop();
      _realTimeAudioListener.SpectrumDataReceived -= OnSpectrumDataReceived;
    }
  }
}