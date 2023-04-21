using System;
using Amphetamine.Core.Enums;

namespace Amphetamine.Services
{
	public class PlaybackFailedEventArgs : EventArgs
	{
		public PlaybackFailureReason FailureReason { get; set; }

		public string Message { get; set; }

		public string StackTrace { get; set; }
	}
}
