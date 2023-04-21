using System;

namespace Amphetamine.Services
{
	public class PlaybackPausedEventArgs : EventArgs
	{
		public bool IsSilent { get; set; }
	}
}
