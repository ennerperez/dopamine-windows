using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Amphetamine.Converters
{
	public class NullImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return AvaloniaProperty.UnsetValue;
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null; //Binding.DoNothing;
		}
	}
}
