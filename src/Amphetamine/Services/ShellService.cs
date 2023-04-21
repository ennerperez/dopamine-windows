// using System;
// using System.Reactive;
// using System.Threading.Tasks;
// using Amphetamine.Core;
// using Amphetamine.Data.Enums;
// using Amphetamine.Services.Shell;
// using Amphetamine.Services.WindowsIntegration;
// using Amphetamine.Shell;
// using Amphetamine.Shell.Events;
// using Avalonia;
// using Avalonia.Controls;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Configurations;
// using ReactiveUI;
//
// // ReSharper disable IdentifierTypo
//
// namespace Amphetamine.Services
// {
// 	public class ShellService : IShellService
// 	{
//
// 		internal class Empty : UserControl
// 		{
// 		}
//
// 		private readonly IRegionManager _regionManager;
// 		private readonly IConfiguration _configuration;
// 		private readonly IWindowsIntegrationService _windowsIntegrationService;
// 		private ActiveMiniPlayerPlayList _activeMiniPlayerPlaylist = ActiveMiniPlayerPlayList.None;
// 		private bool _canSaveWindowGeometry;
// 		private bool _isMiniPlayerActive;
// 		string _nowPlayingPage;
// 		private string _fullPlayerPage;
// 		private string _coverPlayerPage;
// 		private string _microplayerPage;
// 		private string _nanoPlayerPage;
//
// 		public event WindowStateChangedEventHandler WindowStateChanged = delegate {};
// 		public event WindowStateChangeRequestedEventHandler WindowStateChangeRequested = delegate {};
// 		public event PlaylistVisibilityChangeRequestedEventHandler PlaylistVisibilityChangeRequested = delegate {};
// 		public event IsMovableChangeRequestedEventHandler IsMovableChangeRequested = delegate {};
// 		public event ResizeModeChangeRequestedEventHandler ResizeModeChangeRequested = delegate {};
// 		public event TopmostChangeRequestedEventHandler TopmostChangeRequested = delegate {};
// 		public event MinimumSizeChangeRequestedEventHandler MinimumSizeChangeRequested = delegate {};
// 		public event GeometryChangeRequestedEventHandler GeometryChangeRequested = delegate {};
//
//
// 		public DelegateCommand ShowNowPlayingCommand { get; set; }
//
// 		public DelegateCommand ShowFullPlayerCommmand { get; set; }
//
// 		public DelegateCommand TogglePlayerCommand { get; set; }
//
// 		public DelegateCommand<string> ChangePlayerTypeCommand { get; set; }
//
// 		public DelegateCommand<bool?> CoverPlayerPlaylistButtonCommand { get; set; }
//
// 		public DelegateCommand<bool?> MicroPlayerPlaylistButtonCommand { get; set; }
//
// 		public DelegateCommand<bool?> NanoPlayerPlaylistButtonCommand { get; set; }
//
// 		public DelegateCommand ToggleMiniPlayerPositionLockedCommand { get; set; }
//
// 		public DelegateCommand ToggleMiniPlayerAlwaysOnTopCommand { get; set; }
// 		public WindowState WindowState { get; set; }
//
// 		public ShellService(IRegionManager regionManager, IConfiguration configuration, IWindowsIntegrationService windowsIntegrationService, IEventAggregator eventAggregator,
// 			string nowPlayingPage, string fullPlayerPage, string coverPlayerPage, string microplayerPage, string nanoPlayerPage)
// 		{
// 			_regionManager = regionManager;
// 			_configuration = configuration;
// 			_windowsIntegrationService = windowsIntegrationService;
// 			_nowPlayingPage = nowPlayingPage;
// 			_fullPlayerPage = fullPlayerPage;
// 			_coverPlayerPage = coverPlayerPage;
// 			_microplayerPage = microplayerPage;
// 			_nanoPlayerPage = nanoPlayerPage;
//
// 			ShowNowPlayingCommand = new DelegateCommand(() =>
// 			{
// 				_regionManager.RequestNavigate(RegionNames.PlayerTypeRegion, _nowPlayingPage);
// 				configuration.Set("FullPlayer", "IsNowPlayingSelected", true);
// 				eventAggregator.GetEvent<IsNowPlayingPageActiveChanged>().Publish(true);
// 			});
//
// 			Commands.ShowNowPlayingCommand.RegisterCommand(ShowNowPlayingCommand);
//
// 			ShowFullPlayerCommmand = new DelegateCommand(() =>
// 			{
// 				_regionManager.RequestNavigate(RegionNames.PlayerTypeRegion, _fullPlayerPage);
// 				configuration.Set("FullPlayer", "IsNowPlayingSelected", false);
// 				eventAggregator.GetEvent<IsNowPlayingPageActiveChanged>().Publish(false);
// 			});
//
// 			Commands.ShowFullPlayerCommand.RegisterCommand(ShowFullPlayerCommmand);
//
// 			// Window state
// 			WindowState = configuration.Get<bool>("FullPlayer", "IsMaximized") ? WindowState.Maximized : WindowState.Normal;
//
// 			// Player type
// 			ChangePlayerTypeCommand = new DelegateCommand<string>((miniPlayerType) =>
// 				SetPlayer(true, (MiniPlayer)Convert.ToInt32(miniPlayerType)));
//
// 			Commands.ChangePlayerTypeCommand.RegisterCommand(ChangePlayerTypeCommand);
//
// 			TogglePlayerCommand = new DelegateCommand(() =>
// 			{
// 				// If tablet mode is enabled, we should not be able to toggle the player.
// 				if (!_windowsIntegrationService.IsTabletModeEnabled)
// 				{
// 					TogglePlayer();
// 				}
// 			});
//
// 			Commands.TogglePlayerCommand.RegisterCommand(TogglePlayerCommand);
//
// 			// Mini Player Playlist
// 			CoverPlayerPlaylistButtonCommand = new DelegateCommand<bool?>(isPlaylistButtonChecked =>
// 			{
// 				ToggleMiniPlayerPlaylist(MiniPlayer.CoverPlayer, isPlaylistButtonChecked);
// 			});
//
// 			Commands.CoverPlayerPlaylistButtonCommand.RegisterCommand(CoverPlayerPlaylistButtonCommand);
//
// 			MicroPlayerPlaylistButtonCommand = new DelegateCommand<bool?>(isPlaylistButtonChecked =>
// 			{
// 				ToggleMiniPlayerPlaylist(MiniPlayer.MicroPlayer, isPlaylistButtonChecked);
// 			});
//
// 			Commands.MicroPlayerPlaylistButtonCommand.RegisterCommand(MicroPlayerPlaylistButtonCommand);
//
// 			NanoPlayerPlaylistButtonCommand = new DelegateCommand<bool?>(isPlaylistButtonChecked =>
// 			{
// 				ToggleMiniPlayerPlaylist(MiniPlayer.NanoPlayer, isPlaylistButtonChecked);
// 			});
//
// 			Commands.NanoPlayerPlaylistButtonCommand.RegisterCommand(NanoPlayerPlaylistButtonCommand);
//
// 			// Mini Player
// 			ToggleMiniPlayerPositionLockedCommand = new DelegateCommand(() =>
// 			{
// 				var isMiniPlayerPositionLocked = configuration.Get<bool>("Behaviour", "MiniPlayerPositionLocked");
// 				configuration.Set("Behaviour", "MiniPlayerPositionLocked", !isMiniPlayerPositionLocked);
// 				SetWindowPositionLockedFromSettings();
// 			});
//
// 			Commands.ToggleMiniPlayerPositionLockedCommand.RegisterCommand(ToggleMiniPlayerPositionLockedCommand);
//
// 			ToggleMiniPlayerAlwaysOnTopCommand = new DelegateCommand(() =>
// 			{
// 				var topmost = configuration.Get<bool>("Behaviour", "MiniPlayerOnTop");
// 				configuration.Set("Behaviour", "MiniPlayerOnTop", !topmost);
// 				SetWindowTopmostFromSettings();
// 			});
//
// 			Commands.ToggleMiniPlayerAlwaysOnTopCommand.RegisterCommand(ToggleMiniPlayerAlwaysOnTopCommand);
// 		}
//
// 		private void TogglePlayer()
// 		{
// 			if (_isMiniPlayerActive)
// 			{
// 				// Show the Full Player
// 				SetPlayer(false, MiniPlayer.CoverPlayer);
// 			}
// 			else
// 			{
// 				// Show the Mini Player, with the player type which is saved in the settings
// 				SetPlayer(true, (MiniPlayer)_configuration.Get<int>("General", "MiniPlayer"));
// 			}
// 		}
//
// 		public async void SetPlayer(bool isMiniPlayer, MiniPlayer miniPlayerType, bool isInitializing = false)
// 		{
// 			var screenName = typeof(Empty).FullName;
//
// 			// Clear player content
// 			_regionManager.RequestNavigate(RegionNames.PlayerTypeRegion, typeof(Empty).FullName);
//
// 			// Save the player type in the settings
// 			_configuration.Set("General", "IsMiniPlayer", isMiniPlayer);
//
// 			// Only save the Mini Player Type in the settings if the current player is set to the Mini Player
// 			if (isMiniPlayer) _configuration.Set("General", "MiniPlayer", (int)miniPlayerType);
//
// 			// Prevents saving window state and size to the Settings XML while switching players
// 			_canSaveWindowGeometry = false;
//
// 			// Sets the geometry of the player
// 			if (isMiniPlayer | (!_windowsIntegrationService.IsTabletModeEnabled & _windowsIntegrationService.IsStartedFromExplorer))
// 			{
// 				switch (miniPlayerType)
// 				{
// 					case MiniPlayer.CoverPlayer:
// 						SetMiniPlayer(MiniPlayer.CoverPlayer, _activeMiniPlayerPlaylist == ActiveMiniPlayerPlayList.CoverPlayer);
// 						screenName = _coverPlayerPage;
// 						break;
// 					case MiniPlayer.MicroPlayer:
// 						SetMiniPlayer(MiniPlayer.MicroPlayer, _activeMiniPlayerPlaylist == ActiveMiniPlayerPlayList.MicroPlayer);
// 						screenName = _microplayerPage;
// 						break;
// 					case MiniPlayer.NanoPlayer:
// 						SetMiniPlayer(MiniPlayer.NanoPlayer, _activeMiniPlayerPlaylist == ActiveMiniPlayerPlayList.NanoPlayer);
// 						screenName = _nanoPlayerPage;
// 						break;
// 					// Doesn't happen
// 				}
// 			}
// 			else
// 			{
// 				SetFullPlayer();
//
// 				// Default case
// 				screenName = _fullPlayerPage;
//
// 				// Special cases
// 				if (_configuration.Get<bool>("FullPlayer", "IsNowPlayingSelected"))
// 				{
// 					if (isInitializing)
// 					{
// 						if (_configuration.Get<bool>("Startup", "ShowLastSelectedPage"))
// 						{
// 							screenName = _nowPlayingPage;
// 						}
// 					}
// 					else
// 					{
// 						screenName = _nowPlayingPage;
// 					}
// 				}
// 			}
//
// 			// Determine if the player position is locked
// 			SetWindowPositionLockedFromSettings();
//
// 			// Determine if the player is
// 			SetWindowTopmostFromSettings();
//
// 			// Delay, otherwise content is never shown (probably because regions don't exist yet at startup)
// 			await Task.Delay(150);
//
// 			// Navigate to content
// 			_regionManager.RequestNavigate(RegionNames.PlayerTypeRegion, screenName);
//
// 			_canSaveWindowGeometry = true;
// 		}
//
//
// 		private void SetFullPlayer()
// 		{
// 			_isMiniPlayerActive = false;
//
// 			PlaylistVisibilityChangeRequested(this, new PlaylistVisibilityChangeRequestedEventArgs(false, MiniPlayer.CoverPlayer));
//
// 			ResizeModeChangeRequested(this, new ResizeModeChangeRequestedEventArgs(ResizeMode.CanResize));
//
// 			if (_configuration.Get<bool>("FullPlayer", "IsMaximized"))
// 			{
// 				WindowStateChangeRequested(this, new WindowStateChangeRequestedEventArgs(WindowState.Maximized));
// 			}
// 			else
// 			{
// 				WindowStateChangeRequested(this, new WindowStateChangeRequestedEventArgs(WindowState.Normal));
//
// 				GeometryChangeRequested(this, new GeometryChangeRequestedEventArgs(
// 					_configuration.Get<int>("FullPlayer", "Top"),
// 					_configuration.Get<int>("FullPlayer", "Left"),
// 					new Size(_configuration.Get<int>("FullPlayer", "Width"), _configuration.Get<int>("FullPlayer", "Height"))));
// 			}
//
// 			// Set MinWidth and MinHeight AFTER SetGeometry(). This prevents flicker.
// 			MinimumSizeChangeRequested(this, new MinimumSizeChangeRequestedEventArgs(new Size(Constants.MinShellWidth, Constants.MinShellHeight)));
// 		}
//
//
// 		private void SetMiniPlayer(MiniPlayer miniPlayerType, bool openPlaylist)
// 		{
// 			_isMiniPlayerActive = true;
//
// 			// Hide the playlist BEFORE changing window dimensions to avoid strange behaviour
// 			PlaylistVisibilityChangeRequested(this, new PlaylistVisibilityChangeRequestedEventArgs(false, MiniPlayer.CoverPlayer));
//
// 			WindowStateChangeRequested(this, new WindowStateChangeRequestedEventArgs(WindowState.Normal));
// 			ResizeModeChangeRequested(this, new ResizeModeChangeRequestedEventArgs(ResizeMode.CanMinimize));
//
// 			double width = 0;
// 			double height = 0;
//
// 			switch (miniPlayerType)
// 			{
// 				case MiniPlayer.CoverPlayer:
//
// 					width = Constants.CoverPlayerWidth;
// 					height = Constants.CoverPlayerHeight;
// 					break;
// 				case MiniPlayer.MicroPlayer:
// 					width = Constants.MicroPlayerWidth;
// 					height = Constants.MicroPlayerHeight;
// 					break;
// 				case MiniPlayer.NanoPlayer:
// 					width = Constants.NanoPlayerWidth;
// 					height = Constants.NanoPlayerHeight;
// 					break;
// 			}
//
// 			// Set MinWidth and MinHeight BEFORE SetMiniPlayerDimensions(). This prevents flicker.
// 			var minimumSize = new Size(width, height);
//
// 			if (_configuration.Get<bool>("Appearance", "ShowWindowBorder"))
// 			{
// 				// Correction to take into account the window border, otherwise the content
// 				// misses 2px horizontally and vertically when displaying the window border
// 				minimumSize = new Size(width + 2, height + 2);
// 			}
//
// 			MinimumSizeChangeRequested(this, new MinimumSizeChangeRequestedEventArgs(minimumSize));
//
// 			GeometryChangeRequested(this, new GeometryChangeRequestedEventArgs(
// 				_configuration.Get<int>("MiniPlayer", "Top"),
// 				_configuration.Get<int>("MiniPlayer", "Left"),
// 				minimumSize));
//
// 			// Show the playlist AFTER changing window dimensions to avoid strange behavior
// 			if (openPlaylist)
// 			{
// 				PlaylistVisibilityChangeRequested(this, new PlaylistVisibilityChangeRequestedEventArgs(true, miniPlayerType));
// 			}
// 		}
//
// 		public void CheckIfTabletMode(bool isInitializing)
// 		{
// 			if (_windowsIntegrationService.IsTabletModeEnabled)
// 			{
// 				// Always revert to full player when tablet mode is enabled. Maximizing will be done by Windows.
// 				SetPlayer(false, (MiniPlayer)_configuration.Get<int>("General", "MiniPlayer"), isInitializing);
// 			}
// 			else
// 			{
// 				var isMiniPlayer = _configuration.Get<bool>("General", "IsMiniPlayer");
// 				var isMaximized = _configuration.Get<bool>("FullPlayer", "IsMaximized");
// 				WindowStateChangeRequested(this, new WindowStateChangeRequestedEventArgs(isMaximized & !isMiniPlayer ? WindowState.Maximized : WindowState.Normal));
//
// 				SetPlayer(isMiniPlayer, (MiniPlayer)_configuration.Get<int>("General", "MiniPlayer"), isInitializing);
// 			}
// 		}
//
// 		public void SaveWindowLocation(double top, double left, WindowState state)
// 		{
// 			if (_canSaveWindowGeometry)
// 			{
// 				if (_isMiniPlayerActive)
// 				{
// 					_configuration.Set("MiniPlayer", "Top", Convert.ToInt32(top));
// 					_configuration.Set("MiniPlayer", "Left", Convert.ToInt32(left));
// 				}
// 				else if (state != WindowState.Maximized)
// 				{
// 					_configuration.Set("FullPlayer", "Top", Convert.ToInt32(top));
// 					_configuration.Set("FullPlayer", "Left", Convert.ToInt32(left));
// 				}
// 			}
// 		}
//
// 		private void SetWindowTopmostFromSettings()
// 		{
// 			if (_isMiniPlayerActive)
// 			{
// 				TopmostChangeRequested(this, new TopmostChangeRequestedEventArgs(_configuration.Get<bool>("Behaviour", "MiniPlayerOnTop")));
// 			}
// 			else
// 			{
// 				// Full player is never topmost
// 				TopmostChangeRequested(this, new TopmostChangeRequestedEventArgs(false));
// 			}
// 		}
//
// 		private void SetWindowPositionLockedFromSettings()
// 		{
// 			// Only lock position when the mini player is active
// 			if (_isMiniPlayerActive)
// 			{
// 				IsMovableChangeRequested(this, new IsMovableChangeRequestedEventArgs(!_configuration.Get<bool>("Behaviour", "MiniPlayerPositionLocked")));
// 			}
// 			else
// 			{
// 				IsMovableChangeRequested(this, new IsMovableChangeRequestedEventArgs(true));
// 			}
// 		}
//
// 		private void ToggleMiniPlayerPlaylist(MiniPlayer miniPlayerType, bool? isPlaylistVisible)
// 		{
// 			if (isPlaylistVisible ?? false)
// 			{
// 				switch (miniPlayerType)
// 				{
// 					case MiniPlayer.CoverPlayer:
// 						_activeMiniPlayerPlaylist = ActiveMiniPlayerPlayList.CoverPlayer;
// 						break;
// 					case MiniPlayer.MicroPlayer:
// 						_activeMiniPlayerPlaylist = ActiveMiniPlayerPlayList.MicroPlayer;
// 						break;
// 					case MiniPlayer.NanoPlayer:
// 						_activeMiniPlayerPlaylist = ActiveMiniPlayerPlayList.NanoPlayer;
// 						break;
// 					// Shouldn't happen
// 				}
// 			}
// 			else
// 			{
// 				_activeMiniPlayerPlaylist = ActiveMiniPlayerPlayList.None;
// 			}
//
// 			PlaylistVisibilityChangeRequested(this, new PlaylistVisibilityChangeRequestedEventArgs(isPlaylistVisible ?? false, miniPlayerType));
// 		}
//
// 		public void SaveWindowState(WindowState state)
// 		{
// 			WindowState = state;
//
// 			// Only save window state when not in tablet mode. Tablet mode maximizes the screen.
// 			// We don't want to save that, as we want to be able to restore to the original state when leaving tablet mode.
// 			if (_canSaveWindowGeometry & !_windowsIntegrationService.IsTabletModeEnabled)
// 			{
// 				_configuration.Set("FullPlayer", "IsMaximized", state == WindowState.Maximized);
// 			}
//
// 			WindowStateChanged(this, new WindowStateChangedEventArgs(state));
// 		}
//
// 		public void SaveWindowSize(WindowState state, Size size)
// 		{
// 			if (_canSaveWindowGeometry)
// 			{
// 				if (!_isMiniPlayerActive & state != WindowState.Maximized)
// 				{
// 					_configuration.Set("FullPlayer", "Width", Convert.ToInt32(size.Width));
// 					_configuration.Set("FullPlayer", "Height", Convert.ToInt32(size.Height));
// 				}
// 			}
// 		}
//
// 	}
// }
