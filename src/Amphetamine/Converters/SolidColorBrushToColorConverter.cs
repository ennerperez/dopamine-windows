﻿using System;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Amphetamine.Converters
{
	public class SolidColorBrushToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}

			if (!(value is SolidColorBrush))
			{
				throw new InvalidOperationException("Unsupported type [" + value.GetType().Name + "], SolidColorBrushToColorConverter.Convert()");
			}

			return ((SolidColorBrush)value).Color;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

	}
}
