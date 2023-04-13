using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ExCSS;

namespace Amphetamine.Converters
{
	public class BooleanToCollapsedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool vis = bool.Parse(value.ToString());
			return vis ? Visibility.Visible : Visibility.Collapse;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility vis = (Visibility)value;
			return (vis == Visibility.Visible);
		}
	}

	public class InvertingBooleanToCollapsedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool vis = bool.Parse(value.ToString());
			return vis ? Visibility.Collapse : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility vis = (Visibility)value;
			return (vis == Visibility.Collapse);
		}
	}
}
