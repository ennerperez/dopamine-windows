using Avalonia.Controls;

namespace Amphetamine.Services.Interfaces
{
	public interface ITaskbarService
	{
		double ProgressValue { get; }
		int ProgressState { get; }
		string Description { get; }
		string PlayPauseText { get; }
		Image PlayPauseIcon { get; }
		void SetShowProgressInTaskbar(bool showProgressInTaskbar);
	}
}
