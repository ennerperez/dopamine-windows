using System;

namespace Amphetamine.Core.Events
{
	public class PlaybackInterruptedEventArgs : EventArgs
	{
		public string Message { get; set; }
	}
}
