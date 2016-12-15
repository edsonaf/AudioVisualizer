using AudioVisualizer.Utils.RealTimeAudioListener;
using NAudio.CoreAudioApi;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;

namespace AudioVisualizer.Modules.AudioControl
{
  [Export(typeof(AudioControlViewModel))]
  public class AudioControlViewModel : BindableBase
  {
    private readonly IRealTimeAudioListener _audioListener;

    private bool _isListening;
    private DispatcherTimer _timer;

    [ImportingConstructor]
    public AudioControlViewModel(IRealTimeAudioListener realTimeAudioListener)
    {
      _audioListener = realTimeAudioListener;

      SelectedDevice = _audioListener.CaptureDevices.FirstOrDefault();

      _timer = new DispatcherTimer();
      _timer.Interval = TimeSpan.FromMilliseconds(1);
      _timer.Tick += OnGetVolumeLevel;
      _timer.IsEnabled = true;
    }

    #region Properties

    public List<MMDevice> CaptureDevices
    {
      get { return _audioListener.CaptureDevices; }
    }

    public MMDevice SelectedDevice
    {
      get { return _audioListener.SelectedDevice; }
      set
      {
        if (_audioListener.SelectedDevice != value)
          _audioListener.SelectedDevice = value;
      }
    }

    private float _level;

    public float Level
    {
      get { return _level; }
      set { SetProperty(ref _level, value); }
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

    private Brush _themeColor;

    public Brush ThemeColorBrush
    {
      get { return _themeColor; }
      set { SetProperty(ref _themeColor, value); }
    }

    #endregion Properties

    #region Private Function & Events

    private void OnGetVolumeLevel(object sender, EventArgs e)
    {
      if (SelectedDevice != null)
        Level = SelectedDevice.AudioMeterInformation.MasterPeakValue;

    }

    #endregion

  }
}