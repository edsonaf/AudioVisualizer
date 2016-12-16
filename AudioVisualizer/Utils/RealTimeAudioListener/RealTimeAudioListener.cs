using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace AudioVisualizer.Utils.RealTimeAudioListener
{
  [PartCreationPolicy(CreationPolicy.Shared)] // Singleton
  [Export(typeof(IRealTimeAudioListener))]
  public class RealTimeAudioListener : IRealTimeAudioListener
  {
    public event EventHandler<SpectrumDataEventArgs> SpectrumDataReceived;

    private List<byte> _lastSpectrumData = new List<byte>(15);
    private readonly List<byte> _spectrumData = new List<byte>(15);   //spectrum data buffer

    private int _sameDataCounter;

    private WasapiLoopbackCapture _capture;

    public RealTimeAudioListener()
    {
      var enumerator = new MMDeviceEnumerator();
      CaptureDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
    }

    public List<MMDevice> CaptureDevices { get; set; }

    public MMDevice SelectedDevice { get; set; }

    public List<byte> SpectrumData => _spectrumData;

    public void Start()
    {
      _capture = new WasapiLoopbackCapture(SelectedDevice);
      _capture.DataAvailable += DataAvailable;
      _capture.StartRecording();
    }

    public void Stop()
    {
      _capture.StopRecording();
    }

    #region Data Processing

    private float[] ConvertByteToFloat(byte[] array, int length)
    {
      int samplesNeeded = length / 4;
      float[] floatArr = new float[samplesNeeded];

      for (int i = 0; i < samplesNeeded; i++)
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
        float[] data = ConvertByteToFloat(e.Buffer, e.BytesRecorded);
        if (data.Length < 1)
          return;

        Complex[] channelClone = new Complex[(int)FftDataSize.FFT4096];

        for (int i = 0; i < channelClone.Length; i++)
        {
          channelClone[i].X = data[i];
          channelClone[i].Y = 0;
        }

        int binaryExponentitaion = (int)Math.Log((int)FftDataSize.FFT4096, 2);
        FastFourierTransform.FFT(true, binaryExponentitaion, channelClone);
        for (int i = 0; i < channelClone.Length / 2; i++)
        {
          data[i] = (float)Math.Sqrt(channelClone[i].X * channelClone[i].X + channelClone[i].Y * channelClone[i].Y);
        }

        int x, y;
        int b0 = 0;

        //computes the spectrum data, the code is taken from a bass_wasapi sample.
        for (x = 0; x < 16; x++)
        {
          float peak = 0;
          int b1 = (int)Math.Pow(2, x * 10.0 / (16 - 1));
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

        //if (true)
        //{
        //  _spectrum.SpectrumData = _spectrumData;
        //  _spectrum.Set();
        //}

        SpectrumDataReceived?.Invoke(this, new SpectrumDataEventArgs(_spectrumData));

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
      if (_capture != null) _capture.DataAvailable -= DataAvailable;
    }
  }
}