using System;
using System.Windows;
using AudioVisualizer.Modules.AudioControl;
using AudioVisualizer.Modules.GameSenseControl;
using AudioVisualizer.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

namespace AudioVisualizer
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : PrismApplication
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      // AudioVisualizerBootstrapper bootstrapper = new AudioVisualizerBootstrapper();
      // bootstrapper.Run();

#if (DEBUG)
      RunInDebugMode();
#else
            RunInReleaseMode();
#endif
      this.ShutdownMode = ShutdownMode.OnMainWindowClose;
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterSingleton<RealTimeAudioListener.RealTimeAudioListener>();
    }

    protected override Window CreateShell()
    {
      var w = Container.Resolve<Shell>();
      return w;
    }
    
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
      moduleCatalog.AddModule(
        new ModuleInfo
        {
          ModuleName = nameof(AudioControlModule),
          ModuleType = typeof(AudioControlModule).AssemblyQualifiedName
        });
    }

    private static void RunInDebugMode()
    {
      // AudioVisualizerBootstrapper bootstrapper = new AudioVisualizerBootstrapper();
      // bootstrapper.Run();
    }

    private static void RunInReleaseMode()
    {
      // AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
      // try
      // {
      //   AudioVisualizerBootstrapper bootstrapper = new AudioVisualizerBootstrapper();
      //   bootstrapper.Run();
      // }
      // catch (Exception ex)
      // {
      //   HandleException(ex);
      // }
    }

    private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      HandleException(e.ExceptionObject as Exception);
    }

    private static void HandleException(Exception? ex)
    {
      if (ex == null)
        return;

      MessageBox.Show(ex.Message);
      Environment.Exit(1);
    }
  }
}