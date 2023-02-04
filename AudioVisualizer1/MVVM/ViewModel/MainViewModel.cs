using AudioVisualizer1.Core;
using AudioVisualizer1.Services;
using AudioVisualizer1.Utils;
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

        //public SystemColorRetriever ColorRetriever { get; }

        private readonly RelayCommand _navigateToHomeCommand;
        public ICommand NavigateToHomeCommand => _navigateToHomeCommand;

        private readonly RelayCommand _navigateToAudioCommand;

        public ICommand NavigateToAudioControlCommand => _navigateToAudioCommand;


        public MainViewModel(INavigationService navService, SystemColorRetriever colorRetriever)
        {
            _navigation = navService;
            _navigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateAndSetCurrentViewTo<HomeViewModel>(); }, o => true);
            _navigateToAudioCommand =
                new RelayCommand(o => { Navigation.NavigateAndSetCurrentViewTo<AudioControlViewModel>(); }, o => true);
            //ColorRetriever = colorRetriever;
            //ColorRetriever.GetSystemColor();
            //ColorRetriever.Start();
        }
    }
}