using System;
using System.Resources;
using Amphetamine.Core;
using Amphetamine.Data.Models;
using Amphetamine.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Amphetamine.Services.Models
{
	public class Track : ObservableObject
	{
		private int scaledTrackCoverSize = Convert.ToInt32(Constants.TrackCoverSize * Constants.CoverUpscaleFactor);
		private IMetadataService metadataService;
		private IScrobblingService scrobblingService;
		private bool isPlaying;
		private bool isPaused;
		private bool showTrackNumber;
		private ResourceManager _resourceManager;

		public Track(IMetadataService metadataService, IScrobblingService scrobblingService, Amphetamine.Data.Entities.Track entity)
		{
			this.metadataService = metadataService;
			this.scrobblingService = scrobblingService;
			this.Entity = entity;
			_resourceManager = Amphetamine.Core.Resources.Strings.ResourceManager;
		}

		public string PlaylistEntry { get; set; }

		public bool IsPlaylistEntry => !string.IsNullOrEmpty(this.PlaylistEntry);

		public Amphetamine.Data.Entities.Track Entity { get; private set; }

		// SortDuration is used to correctly sort by Length, otherwise sorting goes like this: 1:00, 10:00, 2:00, 20:00.
		public long SortDuration => this.Entity.Duration.HasValue ? this.Entity.Duration.Value : 0;

		// SortAlbumTitle is used to sort by AlbumTitle, then by TrackNumber.
		public string SortAlbumTitle => this.AlbumTitle + this.Entity.Number.Value.ToString("0000");

		// SortAlbumArtist is used to sort by AlbumArtists, then by AlbumTitle, then by TrackNumber.
		public string SortAlbumArtist => this.AlbumArtist + this.AlbumTitle + this.Entity.Number.Value.ToString("0000");

		// SortArtistName is used to sort by ArtistName, then by AlbumTitle, then by TrackNumber.
		public string SortArtistName => this.ArtistName + this.AlbumTitle + this.Entity.Number.Value.ToString("0000");

		public long SortBitrate => this.Entity.BitRate ?? 0;

		public string SortPlayCount => this.Entity.PlayCount?.ToString("0000") ?? string.Empty;

		public string SortSkipCount => this.Entity.SkipCount?.ToString("0000") ?? string.Empty;

		public long SortTrackNumber => this.Entity.Number.HasValue ? this.Entity.Number.Value : 0;

		public string SortDiscNumber => this.Entity.DiscNumber?.ToString("0000") ?? string.Empty;

		public DateTime SortDateAdded => this.Entity.DateAdded;

		public DateTime SortDateFileCreated => this.Entity.DateFileCreated;

		public string DateAdded => this.Entity.DateAdded.ToString("d") ?? string.Empty;

		public string DateFileCreated => this.Entity.DateFileCreated.ToString("d") ?? string.Empty;

		public bool HasLyrics => this.Entity.HasLyrics == 1 ? true : false;

		public string Bitrate => this.Entity.BitRate != null ? this.Entity.BitRate + " kbps" : "";

		public string AlbumTitle => !string.IsNullOrEmpty(this.Entity.Album) ? this.Entity.Album : _resourceManager.GetString("Language_Unknown_Album");

		public string PlayCount => this.Entity.PlayCount?.ToString() ?? string.Empty;

		public string SkipCount => this.Entity.SkipCount?.ToString() ?? string.Empty;

		public string DateLastPlayed => this.Entity.DateLastPlayed?.ToString("g") ?? string.Empty;

		public long SortDateLastPlayed => this.Entity.DateLastPlayed.HasValue ? this.Entity.DateLastPlayed.Value : 0;

		public string Title => string.IsNullOrEmpty(this.Entity.Title) ? this.Entity.FileName : this.Entity.Title;

		public string FileName => this.Entity.FileName;

		public string Path => this.Entity.Path;

		public string SafePath => this.Entity.SafePath;

		public string ArtistName => !string.IsNullOrEmpty(this.Entity.Artist) ? this.Entity.Artist : _resourceManager.GetString("Language_Unknown_Artist");

		public string AlbumArtist => this.GetAlbumArtist();

		public string Genre => !string.IsNullOrEmpty(this.Entity.Genre) ? this.Entity.Genre : _resourceManager.GetString("Language_Unknown_Genre");

		public string FormattedTrackNumber => this.Entity.Number?.ToString("00") ?? "--";

		public string TrackNumber => this.Entity.Number?.ToString() ?? string.Empty;

		public string DiscNumber => this.Entity.DiscNumber?.ToString() ?? string.Empty;

		public string Year => this.Entity.Year?.ToString() ?? string.Empty;

		public string GroupHeader => this.Entity.DiscCount > 1 && this.Entity.DiscNumber > 0  ? $"{this.Entity.Album} ({this.Entity.DiscNumber})" : this.Entity.Album;

		public string GroupSubHeader => this.AlbumArtist;

		public string GetAlbumArtist()
		{
			if (!string.IsNullOrEmpty(this.Entity.Artist))
			{
				return this.Entity.AlbumArtist;
			}
			else if (!string.IsNullOrEmpty(this.Entity.Artist))
			{
				return this.Entity.Artist;
			}

			return _resourceManager.GetString("Language_Unknown_Artist");
		}

		public string Duration
		{
			get
			{
				if (this.Entity.Duration.HasValue)
				{
					TimeSpan ts = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(this.Entity.Duration));

					if (ts.Hours > 0)
					{
						return ts.ToString("hh\\:mm\\:ss");
					}
					else
					{
						return ts.ToString("m\\:ss");
					}
				}
				else
				{
					return "0:00";
				}
			}
		}

		public int Rating
		{
			get { return this.Entity.Rating; }
			set
			{
				// Update the UI
				this.Entity.Rating = value;
				this.OnPropertyChanged(nameof(this.Rating));

				// Update Rating in the database
				this.metadataService.UpdateTrackRatingAsync(this.Entity.Path, value);
			}
		}

		public bool Love
		{
			get { return this.Entity.Love.HasValue && this.Entity.Love.Value != 0 ? true : false; }
			set
			{
				// Update the UI
				this.Entity.Love = value ? 1 : 0;
				this.OnPropertyChanged(nameof(this.Love));

				// Update Love in the database
				this.metadataService.UpdateTrackLoveAsync(this.Entity.Path, value);

				// Send Love/Unlove to the scrobbling service
				this.scrobblingService.SendTrackLoveAsync(this, value);
			}
		}

		public bool IsPlaying
		{
			get { return this.isPlaying; }
			set { SetProperty<bool>(ref this.isPlaying, value); }
		}

		public bool IsPaused
		{
			get { return this.isPaused; }
			set { SetProperty<bool>(ref this.isPaused, value); }
		}

		public bool ShowTrackNumber
		{
			get { return this.showTrackNumber; }
			set { SetProperty<bool>(ref this.showTrackNumber, value); }
		}

		public void UpdateVisibleRating(int rating)
		{
			this.Entity.Rating = rating;
			this.OnPropertyChanged(nameof(this.Rating));
		}

		public void UpdateVisibleLove(bool love)
		{
			this.Entity.Love = love ? 1 : 0;
			this.OnPropertyChanged(nameof(this.Love));
		}

		public void UpdateVisibleCounters(PlaybackCounter counters)
		{
			this.Entity.PlayCount = counters.PlayCount;
			this.Entity.SkipCount = counters.SkipCount;
			this.Entity.DateLastPlayed = counters.DateLastPlayed;
			this.OnPropertyChanged(nameof(this.PlayCount));
			this.OnPropertyChanged(nameof(this.SkipCount));
			this.OnPropertyChanged(nameof(this.DateLastPlayed));
			this.OnPropertyChanged(nameof(this.SortDateLastPlayed));
		}

		public override string ToString()
		{
			return this.Title;
		}

		public Track DeepCopy()
		{
			return new Track(this.metadataService, this.scrobblingService, this.Entity);
		}

		public void UpdateTrack(Amphetamine.Data.Entities.Track track)
		{
			if (track == null)
			{
				return;
			}

			this.Entity = track;

			this.OnPropertyChanged();
		}

		public void Refresh()
		{
			this.OnPropertyChanged();
		}
	}
}
