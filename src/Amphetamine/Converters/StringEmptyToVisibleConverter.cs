using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using ExCSS;

namespace Amphetamine.Converters
{
	public class StringEmptyToVisibleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

			if (value == null)
			{
				return Visibility.Visible;
			}

			return (string.IsNullOrEmpty(value.ToString()) ? Visibility.Visible : Visibility.Collapse);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null; //Binding.DoNothing;
		}
	}
}
