using AudioVisualizer1.Utils;

namespace AudioVisualizer1.Core;

public abstract class ViewModel : ObservableObject
{
    public SystemColorRetriever ColorRetriever { get; }

    protected ViewModel()
    {
        ColorRetriever = new SystemColorRetriever();
        ColorRetriever.GetSystemColor();
    }
}