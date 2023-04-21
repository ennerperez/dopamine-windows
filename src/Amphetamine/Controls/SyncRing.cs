using System;
using Avalonia;
using Avalonia.Controls;

namespace Amphetamine.Controls
{
	public class SyncRing : Label
	{
		public double Middle
		{
			get { return Convert.ToDouble(GetValue(MiddleProperty)); }

			set { SetValue(MiddleProperty, value); }
		}

		public static readonly AvaloniaProperty MiddleProperty = AvaloniaProperty.Register<SyncRing,double> ("Middle" );

		static SyncRing()
		{
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(SyncRing), new FrameworkPropertyMetadata(typeof(SyncRing)));
		}

		public void OnApplyTemplate()
		{
			// base.OnApplyTemplate();
			//
			// this.SizeChanged += SizeChangedHandler;
		}

		private void SizeChangedHandler(object sender, EventArgs e)
		{
			this.Middle = this.Width / 2;
		}
	}
}
