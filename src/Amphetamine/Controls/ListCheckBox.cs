using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Amphetamine.Controls
{
	public class ListCheckBox : CheckBox
	{
		public Brush CheckBackground
		{
			get { return (Brush)GetValue(CheckBackgroundProperty); }

			set { SetValue(CheckBackgroundProperty, value); }
		}

		public Brush CheckBorderBrush
		{
			get { return (Brush)GetValue(CheckBorderBrushProperty); }

			set { SetValue(CheckBorderBrushProperty, value); }
		}

		public Brush CheckMarkBrush
		{
			get { return (Brush)GetValue(CheckMarkBrushProperty); }

			set { SetValue(CheckMarkBrushProperty, value); }
		}

		public static readonly AvaloniaProperty CheckBackgroundProperty =
			AvaloniaProperty.Register<Brush, ListCheckBox>("CheckBackground");

		public static readonly AvaloniaProperty CheckBorderBrushProperty =
			AvaloniaProperty.Register<Brush, ListCheckBox>("CheckBorderBrush");

		public static readonly AvaloniaProperty CheckMarkBrushProperty =
			AvaloniaProperty.Register<Brush, ListCheckBox>("CheckMarkBrush");

		static ListCheckBox()
		{
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(ListCheckBox), new FrameworkPropertyMetadata(typeof(ListCheckBox)));
		}
	}
}
