using System.Collections.Generic;

namespace Amphetamine.Services.Models
{
	public class DequeueResult
	{
		public bool IsSuccess { get; set; }

		public IList<Track> DequeuedTracks { get; set; }

		public bool IsPlayingTrackDequeued { get; set; }

		public Track NextAvailableTrack { get; set; }
	}
}
