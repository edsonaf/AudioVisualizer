using AudioVisualizer.Utils.RealTimeAudioListener;
using AudioVisualizer.Utils.SystemColorRetriever;
using NAudio.CoreAudioApi;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Timers;

namespace AudioVisualizer.Modules.AudioControl
{
  [Export(typeof(AudioControlViewModel))]
  public class AudioControlViewModel : BindableBase
  {
    private readonly IEventAggregator _eventAggregator;
    private readonly IRealTimeAudioListener _audioListener;

    private bool _isListening;
    private readonly Timer _timer;

    [ImportingConstructor]
    public AudioControlViewModel(IEventAggregator aggregator, IRealTimeAudioListener realTimeAudioListener)
    {
      _eventAggregator = aggregator;
      _audioListener = realTimeAudioListener;

      SelectedDevice = _audioListener.CaptureDevices.FirstOrDefault();

      _timer = new Timer(5);
      _timer.Elapsed += OnGetVolumeLevel;
      _timer.Start();
    }

    #region Properties

    public List<MMDevice> CaptureDevices => _audioListener.CaptureDevices;

    public MMDevice SelectedDevice
    {
      get { return _audioListener.SelectedDevice; }
      set
      {
        if (_audioListener.SelectedDevice != value)
          _audioListener.SelectedDevice = value;
      }
    }

    
    public float Level
    {
      get { return SelectedDevice?.AudioMeterInformation.MasterPeakValue ?? 0; }
    }

    private string _onOffButtonText = "Start";

    public string OnOffButtonText
    {
      get { return _onOffButtonText; }
      set { SetProperty(ref _onOffButtonText, value); }
    }

    private bool _comboBoxEnabled = true;

    public bool ComboboxEnabled
    {
      get { return _comboBoxEnabled; }
      set { SetProperty(ref _comboBoxEnabled, value); }
    }

    public Brush ThemeColorBrush
    {
      get
      {
        Color color = SystemColorRetriever.GetSystemColor();
        return new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B));
      }
    }

    #endregion Properties

    #region Commands

    private DelegateCommand _startCommand;

    public ICommand StartCommand => _startCommand ?? (_startCommand = new DelegateCommand(Start, () => SelectedDevice != null));

    #endregion Commands

    #region Private Function & Events

    private void OnGetVolumeLevel(object sender, EventArgs e)
    {
      if (SelectedDevice != null) OnPropertyChanged(() => Level);
    }

    private void Start()
    {
      if (!_isListening)
      {
        //TODO: Start the Visualization Control in the VisualizationRegion
        OnOffButtonText = "Stop";
        ComboboxEnabled = false;

        _eventAggregator.GetEvent<StartVisualizerEvent>().Publish(true);
      }
      else
      {
        OnOffButtonText = "Start";
        ComboboxEnabled = true;
        _eventAggregator.GetEvent<StartVisualizerEvent>().Publish(false);
      }

      _isListening = !_isListening;
    }

    #endregion Private Function & Events

    ~AudioControlViewModel()
    {
      _timer.Stop();
    }
  }
}