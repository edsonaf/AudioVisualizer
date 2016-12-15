using System.ComponentModel.Composition;
using System.Windows;

namespace AudioVisualizer
{
  /// <summary>
  /// Interaction logic for Shell.xaml
  /// </summary>
  [Export]
  public partial class Shell : Window
  {
    public Shell()
    {
      InitializeComponent();
    }

    private ShellViewmodel ViewModel
    {
      set
      {
        DataContext = value;
      }
    }
  }
}