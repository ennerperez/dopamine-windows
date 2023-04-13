using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ExCSS;

namespace Amphetamine.Converters
{
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool vis = bool.Parse(value.ToString());
			return vis ? Visibility.Visible : Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility vis = (Visibility)value;
			return (vis == Visibility.Visible);
		}
	}

	public class InvertingBooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool vis = bool.Parse(value.ToString());
			return vis ? Visibility.Hidden : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility vis = (Visibility)value;
			return (vis == Visibility.Hidden);
		}
	}
}
