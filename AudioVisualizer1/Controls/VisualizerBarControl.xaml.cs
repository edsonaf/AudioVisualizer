using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AudioVisualizer1.Controls;

public partial class VisualizerBarControl : UserControl
{
    public static readonly DependencyProperty ThemeColorProperty = DependencyProperty.Register("ThemeColor",
        typeof(Brush), typeof(VisualizerBarControl), new PropertyMetadata());

    public Brush ThemeColor
    {
        get => (Brush)GetValue(ThemeColorProperty);
        set => SetValue(ThemeColorProperty, value);
    }

    public VisualizerBarControl()
    {
        InitializeComponent();
    }

    public void Set(List<byte> spectrumData)
    {
        Dispatcher.Invoke(() =>
        {
            if (spectrumData.Count < 16) return;
            Bar01.Value = spectrumData[0];
            Bar02.Value = spectrumData[1];
            Bar03.Value = spectrumData[2];
            Bar04.Value = spectrumData[3];
            Bar05.Value = spectrumData[4];
            Bar06.Value = spectrumData[5];
            Bar07.Value = spectrumData[6];
            Bar08.Value = spectrumData[7];
            Bar09.Value = spectrumData[8];
            Bar10.Value = spectrumData[9];
            Bar11.Value = spectrumData[10];
            Bar12.Value = spectrumData[11];
            Bar13.Value = spectrumData[12];
            Bar14.Value = spectrumData[13];
            Bar15.Value = spectrumData[14];
            Bar16.Value = spectrumData[15];
            //Bar01.Value = data[0];
        });
    }

    public void Clear()
    {
        Bar01.Value = 0;
        Bar02.Value = 0;
        Bar03.Value = 0;
        Bar04.Value = 0;
        Bar05.Value = 0;
        Bar06.Value = 0;
        Bar07.Value = 0;
        Bar08.Value = 0;
        Bar09.Value = 0;
        Bar10.Value = 0;
        Bar11.Value = 0;
        Bar12.Value = 0;
        Bar13.Value = 0;
        Bar14.Value = 0;
        Bar15.Value = 0;
        Bar16.Value = 0;
    }
}