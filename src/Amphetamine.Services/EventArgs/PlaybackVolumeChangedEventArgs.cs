namespace Amphetamine.Services
{
	public class PlaybackVolumeChangedEventArgs
	{
		public bool IsChangedWhileLoadingSettings { get; private set; }

		public PlaybackVolumeChangedEventArgs(bool isChangedWhileLoadingSettings)
		{
			IsChangedWhileLoadingSettings = isChangedWhileLoadingSettings;
		}
	}
}
