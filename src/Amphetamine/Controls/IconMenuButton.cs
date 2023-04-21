using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Amphetamine.Controls
{
	public class IconMenuButton : RadioButton
	{
		public Brush AccentForeground
		{
			get { return (Brush)GetValue(AccentForegroundProperty); }

			set { SetValue(AccentForegroundProperty, value); }
		}

		public static readonly AvaloniaProperty AccentForegroundProperty = AvaloniaProperty.Register<IconMenuButton ,Brush>("AccentForeground");

		static IconMenuButton()
		{
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(IconMenuButton), new FrameworkPropertyMetadata(typeof(IconMenuButton)));
		}
	}
}
