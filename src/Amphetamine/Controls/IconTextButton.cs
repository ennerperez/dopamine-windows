using System;
using Avalonia;
using Avalonia.Controls;

namespace Amphetamine.Controls
{
	public class IconTextButton : Button
	{
		public string Glyph
		{
			get { return Convert.ToString(GetValue(GlyphProperty)); }

			set { SetValue(GlyphProperty, value); }
		}

		public double GlyphSize
		{
			get { return Convert.ToDouble(GetValue(GlyphSizeProperty)); }

			set { SetValue(GlyphSizeProperty, value); }
		}

		public static readonly AvaloniaProperty GlyphProperty = AvaloniaProperty.Register<IconTextButton, string>("Glyph");
		public static readonly AvaloniaProperty GlyphSizeProperty = AvaloniaProperty.Register<IconTextButton, double>("GlyphSize");

		static IconTextButton()
		{
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(IconTextButton), new FrameworkPropertyMetadata(typeof(IconTextButton)));
		}
	}
}
