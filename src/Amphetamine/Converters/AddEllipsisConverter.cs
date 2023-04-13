using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Amphetamine.Converters
{
	public class AddEllipsisConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || (!object.ReferenceEquals(value.GetType(), typeof(string))))
			{
				return null; //Binding.DoNothing;
			}

			return string.Concat(value.ToString(), "...");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null; //Binding.DoNothing;
		}
	}
}
