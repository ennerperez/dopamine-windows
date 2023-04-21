using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amphetamine.Core.Audio;
using Amphetamine.Core.Enums;
using Amphetamine.Core.Interfaces;
using Amphetamine.Core.Models;
using Amphetamine.Data.Models;
using Amphetamine.Services.Models;
// ReSharper disable IdentifierTypo

namespace Amphetamine.Services.Interfaces
{

	public delegate void PlaybackFailedEventHandler(object sender, PlaybackFailedEventArgs e);
	public delegate void PlaybackSuccessEventHandler(object sender, PlaybackSuccessEventArgs e);
	public delegate void PlaybackPausedEventHandler(object sender, PlaybackPausedEventArgs e);
	public delegate void PlaybackCountersChangedEventHandler(IList<PlaybackCounter> counters);
	public delegate void PlaybackVolumeChangedEventhandler(object sender, PlaybackVolumeChangedEventArgs e);

	public interface IPlaybackService
    {
        IPlayer Player { get; }

        Track CurrentTrack { get; }

        bool HasQueue { get; }

        bool HasCurrentTrack { get; }

        bool IsSavingQueuedTracks { get; }

        bool IsSavingPlaybackCounters { get; }

        bool HasMediaFoundationSupport { get; }

        IList<Track> Queue { get; }

        bool Shuffle { get; }

        bool Mute { get; }

        bool IsStopped { get; }

        bool IsPlaying { get; }

        TimeSpan GetCurrentTime { get; }

        TimeSpan GetTotalTime { get; }

        double Progress { get; set; }

        float Volume { get; set; }

        LoopMode LoopMode { get; set; }

        bool UseAllAvailableChannels { get; set; }

        int Latency { get; set; }

        bool EventMode { get; set; }

        bool ExclusiveMode { get; set; }

        void Stop();

        void SkipProgress(double progress);

        void SkipSeconds(int jumpSeconds);

        void SetMute(bool mute);

        Task SetShuffleAsync(bool shuffle);

        Task PlayNextAsync();

        Task PlayPreviousAsync();

        Task PlayOrPauseAsync();

        Task PlaySelectedAsync(Track track);

        Task<bool> PlaySelectedAsync(IList<Track> tracks);

        Task EnqueueAsync(IList<Track> tracks, Track track);

        Task EnqueueAsync(IList<Track> tracks);

        Task EnqueueAsync(bool shuffle, bool unshuffle);

        Task EnqueueAsync(IList<Track> tracks, bool shuffle, bool unshuffle);

        Task EnqueueArtistsAsync(IList<string> artists, bool shuffle, bool unshuffle);

        Task EnqueueGenresAsync(IList<string> genres, bool shuffle, bool unshuffle);

        Task EnqueueAlbumsAsync(IList<Album> albumViewModels, bool shuffle, bool unshuffle);

        Task EnqueuePlaylistsAsync(IList<Playlist> playlistViewModels, bool shuffle, bool unshuffle);

        Task StopIfPlayingAsync(Track track);

        Task<EnqueueResult> AddToQueueAsync(IList<Track> tracks);

        Task<EnqueueResult> AddArtistsToQueueAsync(IList<string> artists);

        Task<EnqueueResult> AddGenresToQueueAsync(IList<string> genres);

        Task<EnqueueResult> AddAlbumsToQueueAsync(IList<Album> albumViewModels);

        Task<EnqueueResult> AddToQueueNextAsync(IList<Track> tracks);

        Task<DequeueResult> DequeueAsync(IList<Track> tracks);

        Task SaveQueuedTracksAsync();

        Task SavePlaybackCountersAsync();

        void ApplyPreset(EqualizerPreset preset);

        Task SetIsEqualizerEnabledAsync(bool isEnabled);

        Task UpdateQueueMetadataAsync(IList<FileMetadata> fileMetadatas);

        Task UpdateQueueOrderAsync(IList<Track> tracks);

        Task<IList<AudioDevice>> GetAllAudioDevicesAsync();

        Task SwitchAudioDeviceAsync(AudioDevice audioDevice);

        Task<AudioDevice> GetSavedAudioDeviceAsync();

        event PlaybackSuccessEventHandler PlaybackSuccess;
        event PlaybackFailedEventHandler PlaybackFailed;
        event PlaybackPausedEventHandler PlaybackPaused;
        event EventHandler PlaybackSkipped;
        event EventHandler PlaybackStopped;
        event EventHandler PlaybackResumed;
        event EventHandler PlaybackProgressChanged;
        event PlaybackVolumeChangedEventhandler PlaybackVolumeChanged;
        event EventHandler PlaybackMuteChanged;
        event EventHandler PlaybackLoopChanged;
        event EventHandler PlaybackShuffleChanged;
        event Action<int> AddedTracksToQueue;
        event PlaybackCountersChangedEventHandler PlaybackCountersChanged;
        event Action<bool> LoadingTrack;
        event EventHandler PlayingTrackChanged;
        event EventHandler QueueChanged;
    }
}
