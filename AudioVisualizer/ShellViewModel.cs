using AudioVisualizer.Utils.SystemColorRetriever;
using Prism.Mvvm;

namespace AudioVisualizer
{
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