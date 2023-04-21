using System;

namespace Amphetamine.Interfaces
{
	public interface IWindowsIntegrationService
	{
		bool IsTabletModeEnabled { get; }
		bool IsSystemUsingLightTheme { get; }
		bool IsStartedFromExplorer { get; }
		event EventHandler TabletModeChanged;
		event EventHandler SystemUsesLightThemeChanged;
		void StartMonitoringTabletMode();
		void StopMonitoringTabletMode();
		void StartMonitoringSystemUsesLightTheme();
		void StopMonitoringSystemUsesLightTheme();
	}
}
