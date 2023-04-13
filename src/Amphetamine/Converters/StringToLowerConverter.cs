using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Amphetamine.Converters
{
	public class StringToLowerConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var str = value?.ToString();

			return !string.IsNullOrEmpty(str) ? str.ToLower() : value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
