
using Amphetamine.Services.Interfaces;

namespace Amphetamine.ViewModels
{
	public partial class MainWindowViewModel : ViewModelBase
	{

		public ITaskbarService TaskbarService { get; }

		public MainWindowViewModel()
		{
			// Since this is a basic ShellWindow, there's nothing
			// to do here.
			// For enterprise apps, you could register up subscriptions
			// or other startup background tasks so that they get triggered
			// on startup, rather than putting them in the DashboardViewModel.
			//
			// For example, initiate the pulling of News Feeds, etc.

			Title = "Amphetamine";
		}
	}
}
