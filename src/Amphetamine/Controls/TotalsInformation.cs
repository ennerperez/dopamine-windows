using System;
using Avalonia;
using Avalonia.Controls;

namespace Amphetamine.Controls
{
	public class TotalsInformation : Control
	{
		public string TotalDurationInformation
		{
			get { return Convert.ToString(GetValue(TotalDurationInformationProperty)); }

			set { SetValue(TotalDurationInformationProperty, value); }
		}

		public string TotalSizeInformation
		{
			get { return Convert.ToString(GetValue(TotalSizeInformationProperty)); }

			set { SetValue(TotalSizeInformationProperty, value); }
		}

		public static readonly AvaloniaProperty TotalDurationInformationProperty = AvaloniaProperty.Register<TotalsInformation, string>("TotalDurationInformation");
		public static readonly AvaloniaProperty TotalSizeInformationProperty = AvaloniaProperty.Register<TotalsInformation,string> ("TotalSizeInformation");

		static TotalsInformation()
		{
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(TotalsInformation), new FrameworkPropertyMetadata(typeof(TotalsInformation)));
		}
	}
}
