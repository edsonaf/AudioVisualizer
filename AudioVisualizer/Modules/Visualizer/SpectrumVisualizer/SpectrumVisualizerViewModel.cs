using AudioVisualizer.Controls;
using AudioVisualizer.Modules.AudioControl;
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

    [ImportingConstructor]
    public SpectrumVisualizerViewModel(IEventAggregator aggregator, IRealTimeAudioListener audioListener)
    {
      if (aggregator == null) throw new ArgumentNullException();
      if (audioListener == null) throw new ArgumentException();

      SpectrumBarControl = new VisualizerBarControl();

      _eventAggregator = aggregator;
      _eventAggregator.GetEvent<StartVisualizerEvent>().Subscribe(OnStartVisualizerReceived);

      _realTimeAudioListener = audioListener;
      _realTimeAudioListener.SpectrumDataReceived += OnSpectrumDataReceived;
    }

    public Brush ThemeColor
    {
      get
      {
        Color color = SystemColorRetriever.GetSystemColor();
        return new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B));
      }
    }

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
      SpectrumBarControl.Set(e.SpectrumData);
    }

    ~SpectrumVisualizerViewModel()
    {
      _eventAggregator.GetEvent<StartVisualizerEvent>().Unsubscribe(OnStartVisualizerReceived);

      _realTimeAudioListener.Stop();
      _realTimeAudioListener.SpectrumDataReceived -= OnSpectrumDataReceived;
    }
  }
}