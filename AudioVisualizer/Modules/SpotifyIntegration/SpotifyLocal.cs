// using System;
// using Prism.Mvvm;
// using SpotifyAPI.Local;
// using SpotifyAPI.Local.Models;
using SpotifyAPI.Web;

namespace AudioVisualizer.Modules.SpotifyIntegration
{
  public class SpotifyLocal //: BindableBase, ISpotifyLocal
  {
    private readonly SpotifyClient _spotify;
    private FullTrack _currentTrack;

    public SpotifyLocal(string token)
    {
      _spotify = new SpotifyClient(token);
      // if (IsRunningLocally)
      // {
        // if (_spotify.Connect())
        // {
        //   _spotify.ListenForEvents = true;
        //   UpdateInfos();
        // }
        var current = new PlayerCurrentlyPlayingRequest(PlayerCurrentlyPlayingRequest.AdditionalTypes.Track);
        var currentlyPlaying = _spotify.Player.GetCurrentlyPlaying(current);
      // }
      // _spotify.OnTrackChange += OnSpotifyTrackChange;
    }

    // public bool IsRunningLocally => SpotifyLocalAPI.IsSpotifyRunning();


    public FullTrack CurrentTrack
    {
      get => _currentTrack;
      // set => SetProperty(ref _currentTrack, value);
    }

    public void UpdateInfos()
    {
      // StatusResponse status = _spotify.GetStatus();

      // if (status?.Track != null) //Update track infos
        // UpdateTrack(status.Track);
    }

    private void UpdateTrack(FullTrack track)
    {
      // CurrentTrack = track;
      // OnPropertyChanged(() => CurrentTrack);
      // SpotifyUri uri = track.TrackResource.ParseUri();
    }

    // private void OnSpotifyTrackChange(object sender, TrackChangeEventArgs e)
    // {
    //   UpdateTrack(e.NewTrack);
    // }

  }
}