using System.Collections.Generic;
using AudioVisualizer1.Controls;
using AudioVisualizer1.MVVM.Model;
using RealTimeAudioListener;

namespace AudioVisualizer1.MVVM.ViewModel;

public class VisualizerViewModel : Core.ViewModel
{
    private readonly IRealTimeAudioListener _realTimeAudioListener;
    private readonly GameSense _gameSense;

    public VisualizerViewModel(IRealTimeAudioListener audioListener)
    {
        _realTimeAudioListener = audioListener;
        _realTimeAudioListener.SpectrumDataReceived += OnSpectrumDataReceived;
        _gameSense = new GameSense();
        GameSenseAvailable = _gameSense.InitializeGameSenseConnection();
        SpectrumBarControl = new VisualizerBarControl();
    }

    public bool GameSenseAvailable { get; }

    public bool SendToGameSenseChecked { get; set; }

    public VisualizerBarControl SpectrumBarControl { get; }

    public void OnStartVisualizerReceived(bool start)
    {
        if (start)
            _realTimeAudioListener.Start();
        else
        {
            _realTimeAudioListener.Stop();
            SpectrumBarControl.Set(new List<byte> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
        }
    }

    protected virtual void OnStartVisualizerEvent(bool start)
    {
        // StartVisualizer?.Invoke(this, start);
    }

    private void OnSpectrumDataReceived(object? sender, SpectrumDataEventArgs e)
    {
        // send to spectrum bar control
        SpectrumBarControl.Set(e.SpectrumData);

        // send to Keyboard
        if (SendToGameSenseChecked)
            _gameSense.SendInfoToGameSense(e.SpectrumData);
    }

    ~VisualizerViewModel()
    {
        _realTimeAudioListener.Stop();
        _realTimeAudioListener.SpectrumDataReceived -= OnSpectrumDataReceived;
    }
}