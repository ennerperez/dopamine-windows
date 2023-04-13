using System;
using System.Globalization;
using System.Text;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Amphetamine.Converters
{
	public class AddValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

			if (value == null || parameter == null || !parameter.ToString().IsNumeric())
			{
				return null; //Binding.DoNothing;
			}

			return double.Parse(value.ToString()) + double.Parse(parameter.ToString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null; //Binding.DoNothing;
		}
	}
}
