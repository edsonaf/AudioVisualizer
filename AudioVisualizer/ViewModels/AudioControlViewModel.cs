using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using AudioVisualizer.Modules.AudioControl;
using AudioVisualizer.Modules.SpotifyIntegration;
using NAudio.CoreAudioApi;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using RealTimeAudioListener;

namespace AudioVisualizer.ViewModels
{
  public class AudioControlViewModel : BindableBase, INavigationAware
  {
    private readonly IEventAggregator _eventAggregator;
    private readonly IRealTimeAudioListener _audioListener;
    private readonly ISpotifyLocal _localSpotify;

    private bool _isListening;
    private readonly Timer _timer;

    public AudioControlViewModel(IEventAggregator aggregator, IRealTimeAudioListener realTimeAudioListener, ISpotifyLocal localSpotify)
    {
      _eventAggregator = aggregator;
      _audioListener = realTimeAudioListener;
      _localSpotify = localSpotify;

      SelectedDevice = _audioListener.CaptureDevices.FirstOrDefault();

      _timer = new Timer(5);
      _timer.Elapsed += OnGetVolumeLevel;
      _timer.Start();
    }

    #region Properties

    public bool KeepAlive { get; }
    
    public List<MMDevice> CaptureDevices => _audioListener.CaptureDevices;

    public MMDevice SelectedDevice
    {
      get => _audioListener.SelectedDevice;
      set
      {
        if (_audioListener.SelectedDevice != value)
          _audioListener.SelectedDevice = value;
      }
    }

    public float Level => SelectedDevice?.AudioMeterInformation.MasterPeakValue ?? 0;

    private string _onOffButtonText = "Start";
    public string OnOffButtonText
    {
      get => _onOffButtonText;
      set => SetProperty(ref _onOffButtonText, value);
    }

    private bool _comboBoxEnabled = true;
    public bool ComboboxEnabled
    {
      get => _comboBoxEnabled;
      set => SetProperty(ref _comboBoxEnabled, value);
    }

    public bool SpotifyPlaying => _localSpotify.IsRunningLocally;

    public ISpotifyLocal Spotify => _localSpotify;

    #endregion Properties

    #region Commands

    private DelegateCommand _startCommand;

    public ICommand StartCommand => _startCommand ?? (_startCommand = new DelegateCommand(Start, () => SelectedDevice != null));

    #endregion Commands

    #region Private Function & Events

    private void OnGetVolumeLevel(object sender, EventArgs e)
    {
      // if (SelectedDevice != null) OnPropertyChanged(() => Level);
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

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
      
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
      return false;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
      // throw new NotImplementedException();
    }
  }
}
