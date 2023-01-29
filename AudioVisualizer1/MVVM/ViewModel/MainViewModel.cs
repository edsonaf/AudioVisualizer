using AudioVisualizer1.Core;
using AudioVisualizer1.Services;
using AudioVisualizer1.Utils;

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
        
        public SystemColorRetriever ColorRetriever { get; }

        public RelayCommand NavigateToHomeCommand { get; set; }
        public RelayCommand NavigateToAudioControlCommand { get; set; }

        public MainViewModel(INavigationService navService, SystemColorRetriever colorRetriever)
        {
            _navigation = navService;
            NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
            NavigateToAudioControlCommand = new RelayCommand(o => { Navigation.NavigateTo<AudioControlViewModel>(); }, o => true);

            ColorRetriever = colorRetriever;
            ColorRetriever.GetSystemColor();
            ColorRetriever.Start();
        }
    }
}