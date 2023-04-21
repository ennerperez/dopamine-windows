using System;
using System.Reflection;
using Amphetamine.Services.Interfaces;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace Amphetamine.Services
{
	public class TaskbarService : ObservableObject, ITaskbarService
	{
		private readonly IConfiguration _configuration;
		private readonly IPlaybackService _playbackService;

		public TaskbarService(IConfiguration configuration, IPlaybackService playbackService)
		{
			_configuration = configuration;
			_playbackService = playbackService;

			this.ShowTaskBarItemInfoPause(false);  // When starting, we're not playing yet.

            _playbackService.PlaybackFailed += (_, __) =>
            {
                this.Description = Program.Assembly.Product();
                this.SetTaskbarProgressState(configuration.Get<bool>("Playback", "ShowProgressInTaskbar"), _playbackService.IsPlaying);
                this.ShowTaskBarItemInfoPause(false);
            };

            _playbackService.PlaybackPaused += (_, __) =>
            {
                this.SetTaskbarProgressState(configuration.Get<bool>("Playback", "ShowProgressInTaskbar"), _playbackService.IsPlaying);
                this.ShowTaskBarItemInfoPause(false);
            };

            _playbackService.PlaybackResumed += (_, __) =>
            {
                this.SetTaskbarProgressState(configuration.Get<bool>("Playback", "ShowProgressInTaskbar"), _playbackService.IsPlaying);
                this.ShowTaskBarItemInfoPause(true);
            };

            _playbackService.PlaybackStopped += (_, __) =>
            {
                this.Description = Program.Assembly.Product();
                this.SetTaskbarProgressState(false, false);
                this.ShowTaskBarItemInfoPause(false);
            };

            _playbackService.PlaybackSuccess += (_, __) =>
            {
                if (!string.IsNullOrWhiteSpace(_playbackService.CurrentTrack.ArtistName) && !string.IsNullOrWhiteSpace(_playbackService.CurrentTrack.Title))
                {
                    this.Description = _playbackService.CurrentTrack.ArtistName + " - " + _playbackService.CurrentTrack.Title;
                }
                else
                {
                    this.Description = _playbackService.CurrentTrack.FileName;
                }

                this.SetTaskbarProgressState(_configuration.Get<bool>("Playback", "ShowProgressInTaskbar"), _playbackService.IsPlaying);
                this.ShowTaskBarItemInfoPause(true);
            };

            _playbackService.PlaybackProgressChanged += (_, __) => { this.ProgressValue = _playbackService.Progress; };
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			private set { SetProperty(ref _description, value); }
		}

		private int _progressState;
		public int ProgressState
		{
			get { return _progressState; }
			private set { SetProperty(ref _progressState, value); }
		}

		private double _progressValue;
		public double ProgressValue
		{
			get { return _progressValue; }
			private set { SetProperty(ref _progressValue, value); }
		}

		private string _playPauseText;
		public string PlayPauseText
		{
			get { return _playPauseText; }
			private set { SetProperty(ref _playPauseText, value); }
		}

		private Image _playPauseIcon;
		public Image PlayPauseIcon
		{
			get { return _playPauseIcon; }
			private set { SetProperty(ref _playPauseIcon, value); }
		}

		private void ShowTaskBarItemInfoPause(bool showPause)
		{
			string value = "Play";

			try
			{
				if (showPause)
				{
					value = "Pause";
				}

				this.PlayPauseText = Application.Current.TryFindResource("Language_" + value).ToString();

				Application.Current.Dispatcher.Invoke(() => { this.PlayPauseIcon = (Image)new ImageConverter().ConvertFromString("pack://application:,,,/Icons/TaskbarItemInfo_" + value + ".ico"); });
			}
			catch (Exception ex)
			{
				Program.Logger.Error("Could not change the TaskBarItemInfo Play/Pause icon to '{0}'. Exception: {1}", ex.Message, value);
			}
		}

		private void SetTaskbarProgressState(bool showProgressInTaskbar, bool isPlaying)
		{
			if (showProgressInTaskbar)
			{
				if (isPlaying)
				{
					this.ProgressState = TaskbarItemProgressState.Normal;
				}
				else
				{
					this.ProgressState = TaskbarItemProgressState.Paused;
				}
			}
			else
			{
				this.ProgressValue = 0;
				this.ProgressState = TaskbarItemProgressState.None;
			}
		}

		public void SetShowProgressInTaskbar(bool showProgressInTaskbar)
		{
			this.SetTaskbarProgressState(showProgressInTaskbar, _playbackService.IsPlaying);
		}
	}
	}
}
