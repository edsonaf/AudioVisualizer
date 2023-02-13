using AudioVisualizer1.Core;
using AudioVisualizer1.Services;
using System.Windows.Input;

namespace AudioVisualizer1.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        private readonly RelayCommand _navigateToHomeCommand;
        public ICommand NavigateToHomeCommand => _navigateToHomeCommand;

        private readonly RelayCommand _navigateToAudioCommand;

        public ICommand NavigateToAudioControlCommand => _navigateToAudioCommand;
        
        public MainViewModel(INavigationService navService)
        {
            _navigation = navService;
            _navigateToHomeCommand = new RelayCommand(_ => { Navigation.NavigateAndSetCurrentViewTo<HomeViewModel>(); },
                _ => true);
            _navigateToAudioCommand = new RelayCommand(_ => { Navigation.NavigateAndSetCurrentViewTo<AudioControlViewModel>(); },
                    _ => true);

            // Default
            Navigation.NavigateAndSetCurrentViewTo<HomeViewModel>();
        }
    }
}