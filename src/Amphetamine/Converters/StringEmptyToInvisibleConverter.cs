using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using ExCSS;

namespace Amphetamine.Converters
{
	public class StringEmptyToInvisibleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return Visibility.Collapse;
			}

			return (string.IsNullOrEmpty(value.ToString()) ? Visibility.Collapse : Visibility.Visible);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null; //Binding.Collapse;
		}
	}
}
