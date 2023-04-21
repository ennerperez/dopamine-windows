using System;
using Amphetamine.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Amphetamine.Controls
{
	public class ScalingTextBlock : TextBlock
	{
		public double MinFontSize
		{
			get { return Convert.ToDouble(GetValue(MinFontSizeProperty)); }
			set { SetValue(MinFontSizeProperty, value); }
		}

		public double MaxFontSize
		{
			get { return Convert.ToDouble(GetValue(MaxFontSizeProperty)); }
			set { SetValue(MaxFontSizeProperty, value); }
		}

		public static readonly AvaloniaProperty MinFontSizeProperty = AvaloniaProperty.Register<ScalingTextBlock, double>("MinFontSize", Constants.GlobalFontSize);
		public static readonly AvaloniaProperty MaxFontSizeProperty = AvaloniaProperty.Register<ScalingTextBlock, double>("MaxFontSize", Constants.GlobalFontSize * 2);

		static ScalingTextBlock()
		{
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(ScalingTextBlock), new FrameworkPropertyMetadata(typeof(ScalingTextBlock)));
		}

		protected void OnInitialized(EventArgs e)
		{
			base.OnInitialized();
			this.SetFontSize();

			// TextBlock doesn't have a TexChanged event. This adds possibility to detect that the Text Property has changed.
			// Because there is a limited amount of these TextBlocks in the application, this AvaloniaPropertyDescriptor should
			// not cause any memory leaks.
			//DependencyPropertyDescriptor dp = DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));

			// dp.AddValueChanged(this, (object a, EventArgs b) =>
			// {
			// 	this.SetFontSize();
			// });

			//this.Loaded += ScalingTextBlock_Loaded;
		}

		private void ScalingTextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			this.SetFontSize();
		}

		// protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		// {
		// 	base.OnRenderSizeChanged(sizeInfo);
		// 	this.SetFontSize();
		// }

		private void SetFontSize()
		{
			this.IsVisible = false; // Visibility.Hidden;

			try
			{
				double increment = (this.MaxFontSize - this.MinFontSize) / 3;

				this.FontSize = this.MaxFontSize;

				while (this.FontSize > this.MinFontSize & this.TextIsTooLarge())
				{
					this.FontSize -= increment;
				}

				if (this.FontSize < this.MinFontSize) this.FontSize = this.MinFontSize;
			}
			catch (Exception ex)
			{
				//LogClient.Error("Could not set the font size. Setting minimum font size. Exception: {0}", ex.Message);
				this.FontSize = this.MinFontSize;
			}

			this.IsVisible = true;// Visibility.Visible;
		}

		private bool TextIsTooLarge()
		{
			try
			{
				this.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
			}
			catch (Exception ex)
			{
				//LogClient.Error("Could not find out if the text is too large. Exception: {0}", ex.Message);
				return true;
			}

			return this.Width < this.DesiredSize.Width;
		}
	}
}
