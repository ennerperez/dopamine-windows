using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Amphetamine.Controls
{
	public class Tile : Label
	{
		private Border mTile;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public double IconSize
		{
			get { return Convert.ToDouble(GetValue(IconSizeProperty)); }

			set { SetValue(IconSizeProperty, value); }
		}

		public double IconSizePercent
		{
			get { return Convert.ToDouble(GetValue(IconSizePercentProperty)); }

			set { SetValue(IconSizePercentProperty, value); }
		}

		public static readonly AvaloniaProperty IconSizeProperty = AvaloniaProperty.Register<Tile,double> ("IconSize"); //, typeof(double), typeof(Tile), new PropertyMetadata(null));
		public static readonly AvaloniaProperty IconSizePercentProperty = AvaloniaProperty.Register<Tile,double> ("IconSizePercent"); //, typeof(double), typeof(Tile), new PropertyMetadata(null));

		static Tile()
		{
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(Tile), new FrameworkPropertyMetadata(typeof(Tile)));
		}

		protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
		{
			base.OnApplyTemplate(e);

			//this.mTile = (Border)GetTemplateChild("PART_Tile");

			if (this.mTile != null)
			{
				this.SetIconSize(this.mTile);

				//this.mTile.SizeChanged += SizeChangedHandler;
			}
		}

		private void SizeChangedHandler(object sender, EventArgs e)
		{
			this.SetIconSize((Border)sender);
		}

		private void SetIconSize(Border iTile)
		{
			try
			{
				// For some reason, "ActualHeight" is always 0 when arriving here, so we use "Height"
				this.IconSize = iTile.Height * (this.IconSizePercent / 100);
			}
			catch (Exception)
			{
				this.IconSize = 0;
			}
		}
	}
}
