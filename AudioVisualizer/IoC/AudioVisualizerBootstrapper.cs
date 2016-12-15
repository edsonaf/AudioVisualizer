using AudioVisualizer.Modules.AudioControl;
using Prism.Mef;
using Prism.Modularity;
using System.ComponentModel.Composition.Hosting;
using System.Windows;

namespace AudioVisualizer.IoC
{
  public class AudioVisualizerBootstrapper : MefBootstrapper
  {
    protected override DependencyObject CreateShell()
    {
      return Container.GetExportedValue<Shell>();
    }

    protected override void InitializeShell()
    {
      Application.Current.MainWindow = (Window)Shell;
      Application.Current.MainWindow.Show();
    }

    protected override void ConfigureAggregateCatalog()
    {
      base.ConfigureAggregateCatalog();

      AggregateCatalog.Catalogs.Add(new AssemblyCatalog(GetType().Assembly));

      // TODO: scan directory to get modules
      //DirectoryCatalog catalog = new DirectoryCatalog("DirectoryModules");
      //AggregateCatalog.Catalogs.Add(catalog);
    }

    protected override void ConfigureModuleCatalog()
    {
      ModuleCatalog.AddModule(
        new ModuleInfo()
        {
          ModuleName = typeof(AudioControlModule).Name,
          ModuleType = typeof(AudioControlModule).AssemblyQualifiedName
        });
    }
  }
}