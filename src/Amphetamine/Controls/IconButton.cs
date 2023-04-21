using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Amphetamine.Controls
{
	public class IconButton : Button
	{
		public Geometry Data
		{
			get { return (Geometry)GetValue(DataProperty); }

			set { SetValue(DataProperty, value); }
		}

		public static readonly AvaloniaProperty DataProperty = AvaloniaProperty.Register<IconButton, Geometry>("Data");

		static IconButton()
		{
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
		}
	}
}
