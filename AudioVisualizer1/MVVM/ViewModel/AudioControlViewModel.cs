using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using AudioVisualizer1.Core;
using AudioVisualizer1.Services;
using NAudio.CoreAudioApi;
using RealTimeAudioListener;

namespace AudioVisualizer1.MVVM.ViewModel;

public class AudioControlViewModel : Core.ViewModel
{
    private readonly IRealTimeAudioListener _audioListener;
    private readonly Timer _timer;
    private bool _isListening;

    public AudioControlViewModel(INavigationService navService, IRealTimeAudioListener audioListener)
    {
        _audioListener = audioListener;
        SelectedDevice = _audioListener.DeviceCollection.FirstOrDefault();

        AudioBars = (navService.NavigateTo<VisualizerViewModel>() as VisualizerViewModel)!;

        _timer = new Timer(5);
        _timer.Elapsed += OnGetVolumeLevel;
        _timer.Start();
    }

    private void OnGetVolumeLevel(object? sender, ElapsedEventArgs e)
    {
        if (SelectedDevice != null) OnPropertyChanged(nameof(Level));
    }

    public VisualizerViewModel AudioBars { get; }

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
    private bool _isPause = false; // TODO

    public bool ComboboxEnabled
    {
        get => _comboBoxEnabled;
        set
        {
            _comboBoxEnabled = value;
            OnPropertyChanged();
        }
    }

    public List<MMDevice> CaptureDevices => _audioListener.DeviceCollection.ToList();

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
            OnOffButtonText = _isPause ? "Pause" : "Stop";
            ComboboxEnabled = false;
            OnStartVisualizerEvent(true);
        }
        else
        {
            OnOffButtonText = "Start";
            ComboboxEnabled = true;
            OnStartVisualizerEvent(false);
        }

        _isListening = !_isListening;
    }

    private void OnStartVisualizerEvent(bool start)
    {
        AudioBars.OnStartVisualizerReceived(start);
    }

    ~AudioControlViewModel()
    {
        _timer.Stop();
        _timer.Elapsed -= OnGetVolumeLevel;
    }
}