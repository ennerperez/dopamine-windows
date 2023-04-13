using System;
using Avalonia;
using Avalonia.Controls;

namespace Amphetamine.Controls
{
	public class AccentTextBox : TextBox
	{
		public bool ShowIcon
		{
			get { return Convert.ToBoolean(GetValue(ShowIconProperty)); }

			set { SetValue(ShowIconProperty, value); }
		}

		public string IconGlyph
		{
			get { return Convert.ToString(GetValue(IconGlyphProperty)); }

			set { SetValue(IconGlyphProperty, value); }
		}

		public string IconToolTip
		{
			get { return Convert.ToString(GetValue(IconToolTipProperty)); }

			set { SetValue(IconToolTipProperty, value); }
		}

		public bool ShowAccent
		{
			get { return Convert.ToBoolean(GetValue(ShowAccentProperty)); }

			set { SetValue(ShowAccentProperty, value); }
		}

		public static readonly AvaloniaProperty ShowIconProperty = AvaloniaProperty.Register<AccentTextBox, bool>("ShowIcon");
		public static readonly AvaloniaProperty IconGlyphProperty = AvaloniaProperty.Register<AccentTextBox, string>("IconGlyph");
		public static readonly AvaloniaProperty IconToolTipProperty = AvaloniaProperty.Register<AccentTextBox, string>("IconToolTip");
		public static readonly AvaloniaProperty ShowAccentProperty = AvaloniaProperty.Register<AccentTextBox, bool>("ShowAccent");

		static AccentTextBox()
		{
			//This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
			//This style is defined in themes\generic.xaml
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(AccentTextBox), new FrameworkPropertyMetadata(typeof(AccentTextBox)));
		}
	}
}
