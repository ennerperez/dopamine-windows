﻿using System;
using System.Globalization;
using System.Text;
using Avalonia.Data.Converters;

namespace Amphetamine.Converters
{
	public class BooleanToOpacityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null || !parameter.ToString().IsNumeric())
			{
				return 1.0;
			}

			if (bool.Parse(value.ToString()))
			{
				return 1.0;
			}
			else
			{
				return double.Parse(parameter.ToString());
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
