using System.Windows.Media;
using AudioVisualizer1.Utils;

namespace AudioVisualizer1.Core;

public abstract class ViewModel : ObservableObject
{
    private readonly SystemColorRetriever _colorRetriever = new();

    public SystemColorRetriever ColorRetriever => _colorRetriever;
    
    protected ViewModel()
    {
        _colorRetriever.Start();
        _colorRetriever.GetSystemColor();
    }
}