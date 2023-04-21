using System.Collections.Generic;

namespace Amphetamine.Services.Models
{
	public class EnqueueResult
	{
		public bool IsSuccess { get; set; }

		public IList<Track> EnqueuedTracks { get; set; }
	}
}
