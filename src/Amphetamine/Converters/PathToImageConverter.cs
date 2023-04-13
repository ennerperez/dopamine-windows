using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace Amphetamine.Converters
{
	public class PathToImageConverter : IMultiValueConverter
	{
		public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				string path = values[0] as string;
				int size = Int32.Parse(values[1].ToString());

				if (!string.IsNullOrEmpty(path) & System.IO.File.Exists(path))
				{
					var info = new FileInfo(path);

					if (info.Exists && info.Length > 0)
					{
						var result = new Bitmap(info.FullName);
						return result;
					}
				}
			}
			catch (Exception)
			{
			}

			return null;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
}
