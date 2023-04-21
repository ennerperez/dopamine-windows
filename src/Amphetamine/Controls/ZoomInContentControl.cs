using System;
using Avalonia;
using Avalonia.Controls;

namespace Amphetamine.Controls
{
	public class ZoomInContentControl : ContentControl
	{
		public double Duration
		{
			get { return Convert.ToDouble(GetValue(DurationProperty)); }

			set { SetValue(DurationProperty, value); }
		}

		public static readonly AvaloniaProperty DurationProperty = AvaloniaProperty.Register<ZoomInContentControl,double>("Duration", 0.5);

		protected void OnContentChanged(object oldContent, object newContent)
		{
			if (newContent != null)
			{
				this.DoAnimation();
			}
		}

		private void DoAnimation()
		{
			// var ta = new ThicknessAnimation();
			// ta.From = new Thickness(this.ActualWidth / 2, this.ActualHeight / 2, this.ActualWidth / 2, this.ActualHeight / 2);
			// ta.To = new Thickness(0, 0, 0, 0);
			// ta.Duration = new Duration(TimeSpan.FromSeconds(this.Duration));
			// this.BeginAnimation(MarginProperty, ta);
		}
	}
}
