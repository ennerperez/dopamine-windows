using System;
using System.Globalization;
using System.Text;
using Avalonia.Data.Converters;

namespace Amphetamine.Converters
{
	public class PercentSizeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// Value = Binding to total available size
			// Parameter = percent of total available size to return
			if (value == null || parameter == null || !parameter.ToString().IsNumeric())
			{
				return null; //Binding.DoNothing;
			}

			return double.Parse(value.ToString()) * double.Parse(parameter.ToString()) / 100;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null; //Binding.DoNothing;
		}
	}
}
