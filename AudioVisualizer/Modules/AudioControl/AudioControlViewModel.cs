using AudioVisualizer.Utils.RealTimeAudioListener;
using NAudio.CoreAudioApi;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace AudioVisualizer.Modules.AudioControl
{
  [Export(typeof(AudioControlViewModel))]
  public class AudioControlViewModel : BindableBase
  {
    private readonly IRealTimeAudioListener _audioListener;

    private bool _isListening;
    
    public AudioControlViewModel(IRealTimeAudioListener realTimeAudioListener)
    {
      _audioListener = realTimeAudioListener;
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
  }
}