using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Amphetamine.Converters
{
	public class BooleanToCenterTextAlignmentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value == true)
			{
				return TextAlignment.Center;
			}
			else
			{
				return TextAlignment.Left;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
