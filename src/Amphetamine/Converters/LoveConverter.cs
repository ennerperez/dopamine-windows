using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ExCSS;

namespace Amphetamine.Converters
{
	public class LoveConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool isLoved = System.Convert.ToBoolean(value);
			return isLoved ? Visibility.Visible : Visibility.Collapse;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class UnloveConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool isLoved = System.Convert.ToBoolean(value);
			return isLoved ? Visibility.Collapse : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
