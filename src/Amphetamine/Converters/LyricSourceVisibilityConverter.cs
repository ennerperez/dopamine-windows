using System;
using System.Globalization;
using Amphetamine.Core.Enums;
using Avalonia.Data.Converters;
using ExCSS;

namespace Amphetamine.Converters
{
	public class LyricSourceVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return Visibility.Hidden;
			}

			return (Source) value == (Source) parameter ? Visibility.Visible : Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
