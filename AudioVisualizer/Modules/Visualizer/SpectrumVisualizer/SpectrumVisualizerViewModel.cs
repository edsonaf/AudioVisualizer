using System.Collections.Generic;
using AudioVisualizer.Controls;
using AudioVisualizer.Modules.AudioControl;
using AudioVisualizer.Modules.GameSenseControl;
using AudioVisualizer.ViewModels;
using Prism.Events;
using RealTimeAudioListener;

namespace AudioVisualizer.Modules.Visualizer.SpectrumVisualizer
{
  public class SpectrumVisualizerViewModel : ShellViewModel
  {
    private readonly IEventAggregator _eventAggregator;
    private readonly IRealTimeAudioListener _realTimeAudioListener;
    private readonly IGameSenseModule _gameSenseModule;

    public SpectrumVisualizerViewModel(IEventAggregator aggregator, IRealTimeAudioListener audioListener, IGameSenseModule gameSenseModule)
    {
      SpectrumBarControl = new VisualizerBarControl();

      _eventAggregator = aggregator;
      _eventAggregator.GetEvent<StartVisualizerEvent>().Subscribe(OnStartVisualizerReceived);

      _realTimeAudioListener = audioListener;
      _realTimeAudioListener.SpectrumDataReceived += OnSpectrumDataReceived;

      _gameSenseModule = gameSenseModule;
      GameSenseAvailable = _gameSenseModule.InitializeGameSenseConnection();
    }

    public bool GameSenseAvailable { get; }

    public bool SendToGameSenseChecked { get; set; }

    public VisualizerBarControl SpectrumBarControl { get; }

    private void OnStartVisualizerReceived(bool start)
    {
      if (start)
        _realTimeAudioListener.Start();
      else
      {
        _realTimeAudioListener.Stop();
        SpectrumBarControl.Set(new List<byte>(){0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0});
      }
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