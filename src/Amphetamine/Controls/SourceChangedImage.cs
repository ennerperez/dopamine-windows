using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Amphetamine.Controls
{
	public class SourceChangedImage : Image
	{
		// public static readonly RoutedEvent SourceChangedEvent = EventManager.RegisterRoutedEvent("SourceChanged",
		// 	RoutingStrategy.Direct, typeof(EventHandler<RoutedEventArgs>), typeof(SourceChangedImage));

		static SourceChangedImage()
		{
			//SourceProperty.OverrideMetadata(typeof(SourceChangedImage), new FrameworkPropertyMetadata(SourcePropertyChanged));
		}

		public event EventHandler<RoutedEventArgs> SourceChanged = delegate { };

		private static void SourcePropertyChanged(AvaloniaObject obj, EventArgs e)
		{
			//(obj as SourceChangedImage)?.RaiseEvent(new RoutedEventArgs(SourceChangedEvent));
		}
	}
}
