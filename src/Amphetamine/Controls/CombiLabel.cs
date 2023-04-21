using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
// ReSharper disable IdentifierTypo

namespace Amphetamine.Controls
{
	public class CombiLabel : Label
    {
        public static readonly AvaloniaProperty FontSize2Property =
	        AvaloniaProperty.Register<CombiLabel, int>("FontSize2");
        public static readonly AvaloniaProperty FontWeight2Property =
	        AvaloniaProperty.Register<CombiLabel, FontWeight>("FontWeight2");
        public static readonly AvaloniaProperty FontStyle2Property =
	        AvaloniaProperty.Register<CombiLabel, FontStyle>("FontStyle2");
        public static readonly AvaloniaProperty Content2Property =
	        AvaloniaProperty.Register<CombiLabel, object>("Content2");
        public static readonly AvaloniaProperty Foreground2Property =
	        AvaloniaProperty.Register<CombiLabel, Brush>("Foreground2");

        public int FontSize2
        {
            get { return Convert.ToInt32(GetValue(FontSize2Property)); }

            set { SetValue(FontSize2Property, value); }
        }

        public FontWeight FontWeight2
        {
            get { return (FontWeight)GetValue(FontWeight2Property); }

            set { SetValue(FontWeight2Property, value); }
        }

        public object Content2
        {
            get { return (object)GetValue(Content2Property); }

            set { SetValue(Content2Property, value); }
        }

        public Brush Foreground2
        {
            get { return (Brush)GetValue(Foreground2Property); }

            set { SetValue(Foreground2Property, value); }
        }

        static CombiLabel()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(CombiLabel), new FrameworkPropertyMetadata(typeof(CombiLabel)));
        }
    }
}
