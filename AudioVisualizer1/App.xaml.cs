using System;
using System.Windows;
using AudioVisualizer1.Core;
using AudioVisualizer1.MVVM.View;
using AudioVisualizer1.MVVM.ViewModel;
using AudioVisualizer1.Services;
using AudioVisualizer1.Utils;
using Microsoft.Extensions.DependencyInjection;
using RealTimeAudioListener;

namespace AudioVisualizer1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        
        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainWindow>(p => new MainWindow()
            {
                DataContext = p.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<IRealTimeAudioListener, RealTimeAudioListener.RealTimeAudioListener>();
            services.AddSingleton<SystemColorRetriever>();
            
            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<AudioControlViewModel>();
            
            // Navigation services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, ViewModel>>(p =>
                viewModelType => (ViewModel)p.GetRequiredService(viewModelType));
            
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}