using System;

namespace Amphetamine.Services
{
	public class PlaybackSuccessEventArgs : EventArgs
	{
		public bool IsPlayingPreviousTrack { get; set; }

		public bool IsSilent { get; set; }
	}

}
