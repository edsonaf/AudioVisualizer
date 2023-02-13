using System.Diagnostics;
using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;

namespace RealTimeAudioListener;

public class RealTimeAudioListener : IRealTimeAudioListener
{
    public event EventHandler<SpectrumDataEventArgs> SpectrumDataReceived;

    private readonly List<byte> _spectrumData = new(15); //spectrum data buffer
    private List<byte> _lastSpectrumData = new(15);
    private WasapiLoopbackCapture _capture;
    private readonly MMDeviceEnumerator _enumerator = new();
    private readonly NotificationClientImplementation _notificationClient;
    private readonly NAudio.CoreAudioApi.Interfaces.IMMNotificationClient _notifyClient;

    public RealTimeAudioListener()
    {
        Debug.WriteLine($"Started {nameof(RealTimeAudioListener)}...");

        _notificationClient = new NotificationClientImplementation();
        _notifyClient = _notificationClient;
        _enumerator.RegisterEndpointNotificationCallback(_notifyClient);
        _selectedDevice = _enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
    }

    public MMDeviceCollection DeviceCollection =>
        _enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

    private MMDevice _selectedDevice;
    private int _sameDataCounter;

    public MMDevice SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            _selectedDevice = value;
            
        }
    }

    public List<byte> SpectrumData => _spectrumData;

    public void Start()
    {
        _capture = new WasapiLoopbackCapture(SelectedDevice);
        _capture.DataAvailable += DataAvailable!;
        _capture.StartRecording();
    }

    public void Stop()
    {
        _capture.StopRecording();
    }

    #region Data Processing

    private static float[] ConvertByteToFloat(byte[] array, int length)
    {
        var samplesNeeded = length / 4;
        var floatArr = new float[samplesNeeded];

        for (var i = 0; i < samplesNeeded; i++)
        {
            floatArr[i] = BitConverter.ToSingle(array, i * 4);
        }

        return floatArr;
    }

    private void DataAvailable(object sender, WaveInEventArgs e)
    {
        try
        {
            // Convert byte[] to float[].
            var data = ConvertByteToFloat(e.Buffer, e.BytesRecorded);
            if (data.Length < 1)
                return;

            var channelClone = new Complex[(int)FftDataSize.FFT4096];

            for (var i = 0; i < channelClone.Length; i++)
            {
                channelClone[i].X = data[i];
                channelClone[i].Y = 0;
            }

            var binaryExponentiation = (int)Math.Log((int)FftDataSize.FFT4096, 2);
            FastFourierTransform.FFT(true, binaryExponentiation, channelClone);
            for (var i = 0; i < channelClone.Length / 2; i++)
            {
                data[i] = (float)Math.Sqrt(
                    channelClone[i].X * channelClone[i].X + channelClone[i].Y * channelClone[i].Y);
            }

            int x, y;
            var b0 = 0;

            // computes the spectrum data, the code is taken from a bass_wasapi sample.
            for (x = 0; x < 16; x++)
            {
                float peak = 0;
                var b1 = (int)Math.Pow(2, x * 10.0 / (16 - 1));
                if (b1 > 1023) b1 = 2047;
                if (b1 <= b0) b1 = b0 + 1;
                for (; b0 < b1; b0++)
                {
                    if (peak < data[1 + b0]) peak = data[1 + b0];
                }

                y = (int)(Math.Sqrt(peak) * 3 * 255 - 4);
                if (y > 255) y = 255;
                if (y < 0) y = 0;
                _spectrumData.Add((byte)y);
            }

            SpectrumDataReceived(this, new SpectrumDataEventArgs(_spectrumData));

            if (_spectrumData.Equals(_lastSpectrumData))
            {
                _sameDataCounter++;
            }
            else
            {
                _sameDataCounter = 0;
            }

            _spectrumData.Clear();
            _lastSpectrumData = _spectrumData;
        }
        catch (Exception)
        {
            // do nothing
        }
    }

    #endregion Data Processing

    ~RealTimeAudioListener()
    {
        _capture.DataAvailable -= DataAvailable!;
    }
}