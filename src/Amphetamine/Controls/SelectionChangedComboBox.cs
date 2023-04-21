using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace Amphetamine.Controls
{
	public class SelectionChangedComboBox : ComboBox
	{
		bool mouseWasDown = false;

		public Brush SelectionChangedForeground
		{
			get { return (Brush)GetValue(SelectionChangedForegroundProperty); }

			set { SetValue(SelectionChangedForegroundProperty, value); }
		}

		public static readonly AvaloniaProperty SelectionChangedForegroundProperty = AvaloniaProperty.Register<SelectionChangedComboBox,Brush>("SelectionChangedForeground");

		private void Me_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (mouseWasDown)
			{
				this.Foreground = this.SelectionChangedForeground;
			}
		}

		private void Me_PreviewMouseLeftButtonUp(object sender, PointerEventArgs e)
		{
			mouseWasDown = true;
		}
	}
}
