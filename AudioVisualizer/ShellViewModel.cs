using AudioVisualizer.Utils.SystemColorRetriever;
using Prism.Mvvm;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace AudioVisualizer
{
  [Export]
  public class ShellViewModel : BindableBase
  {
    public ShellViewModel()
    {
      ThemeColor = SystemColorRetriever.GetSystemColor();
    }

    private Color _themeColor;

    public Color ThemeColor
    {
      get { return _themeColor; }
      set { SetProperty(ref _themeColor, value); }
    }
  }
}