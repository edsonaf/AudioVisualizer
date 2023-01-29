using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using AudioVisualizer1.Core;
using AudioVisualizer1.Utils;
using NAudio.CoreAudioApi;
using RealTimeAudioListener;

namespace AudioVisualizer1.MVVM.ViewModel;

public class AudioControlViewModel : Core.ViewModel
{
    private readonly IRealTimeAudioListener _audioListener;
    private readonly SystemColorRetriever _colorRetriever;
    
    private readonly Timer _timer;
    private bool _isListening;

    public AudioControlViewModel(IRealTimeAudioListener audioListener, SystemColorRetriever colorRetriever)
    {
        _audioListener = audioListener;
        SelectedDevice = _audioListener.CaptureDevices.FirstOrDefault();

        _colorRetriever = colorRetriever;
        _colorRetriever.GetSystemColor();
        
        _timer = new Timer(5);
        _timer.Elapsed += OnGetVolumeLevel;
        _timer.Start();
    }

    private void OnGetVolumeLevel(object? sender, ElapsedEventArgs e)
    {
        if (SelectedDevice != null) OnPropertyChanged(nameof(Level));
    }

    public SystemColorRetriever ColorRetriever => _colorRetriever;

    public float Level => SelectedDevice?.AudioMeterInformation.MasterPeakValue ?? 0;

    private string _onOffButtonText = "Start";

    public string OnOffButtonText
    {
        get => _onOffButtonText;
        private set
        {
            _onOffButtonText = value;
            OnPropertyChanged();
        }
    }

    private bool _comboBoxEnabled = true;

    public bool ComboboxEnabled
    {
        get => _comboBoxEnabled;
        set
        {
            _comboBoxEnabled = value;
            OnPropertyChanged();
        }
    }

    public List<MMDevice> CaptureDevices => _audioListener.CaptureDevices;

    public MMDevice? SelectedDevice
    {
        get => _audioListener.SelectedDevice;
        set
        {
            if (_audioListener.SelectedDevice != value)
                _audioListener.SelectedDevice = value;
        }
    }

    public ICommand StartCommand =>
        new RelayCommand(_ => Start(),
            _ => SelectedDevice != null);

    private void Start()
    {
        if (!_isListening)
        {
            OnOffButtonText = "Stop";
            ComboboxEnabled = false;
            // _eventAggregator.GetEvent<StartVisualizerEvent>().Publish(true);
        }
        else
        {
            OnOffButtonText = "Start";
            ComboboxEnabled = true;
            // _eventAggregator.GetEvent<StartVisualizerEvent>().Publish(false);
        }

        _isListening = !_isListening;
    }

    ~AudioControlViewModel()
    {
        _timer.Stop();
        _timer.Elapsed -= OnGetVolumeLevel;
    }
}