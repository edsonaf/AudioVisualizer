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
      ColorRetriever = new SystemColorRetriever();
      ColorRetriever.GetSystemColor();
      ColorRetriever.Start();
    }

    public SystemColorRetriever ColorRetriever { get; }

    private bool _topMost = true;
    public bool TopMost
    {
      get => _topMost;
      set => SetProperty(ref _topMost, value);
    }

    ~ShellViewModel()
    {
      ColorRetriever.Stop();
    }
  }
}