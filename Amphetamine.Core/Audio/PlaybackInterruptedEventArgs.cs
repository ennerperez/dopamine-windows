using System;

namespace Amphetamine.Core.Audio
{
    public class PlaybackInterruptedEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
