using System.ComponentModel.Composition;
using Prism.Mvvm;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Models;

namespace AudioVisualizer.Modules.SpotifyIntegration
{
  [PartCreationPolicy(CreationPolicy.Shared)] // Singleton
  [Export(typeof(ISpotifyLocal))]
  public class SpotifyLocal : BindableBase, ISpotifyLocal
  {
    private readonly SpotifyLocalAPI _spotify;
    private Track _currentTrack;

    public SpotifyLocal()
    {
      _spotify = new SpotifyLocalAPI();
      if (IsRunningLocally)
      {
        if (_spotify.Connect())
        {
          _spotify.ListenForEvents = true;
          UpdateInfos();
        }

      }
      _spotify.OnTrackChange += OnSpotifyTrackChange;
    }

    public bool IsRunningLocally => SpotifyLocalAPI.IsSpotifyRunning();


    public Track CurrentTrack
    {
      get => _currentTrack;
      set => SetProperty(ref _currentTrack, value);
    }

    public void UpdateInfos()
    {
      StatusResponse status = _spotify.GetStatus();

      if (status?.Track != null) //Update track infos
        UpdateTrack(status.Track);
    }

    private void UpdateTrack(Track track)
    {
      CurrentTrack = track;
      OnPropertyChanged(() => CurrentTrack);
      SpotifyUri uri = track.TrackResource.ParseUri();
    }

    private void OnSpotifyTrackChange(object sender, TrackChangeEventArgs e)
    {
      UpdateTrack(e.NewTrack);
    }

  }
}