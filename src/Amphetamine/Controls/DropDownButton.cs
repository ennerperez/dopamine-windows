using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Amphetamine.Controls
{
	public class DropDownButton : ToggleButton
	{
		public DropDownButton()
		{
			// Bind the ToggleButton.IsChecked property to the drop-down's IsOpen property
			var binding = new Binding("Menu.IsOpen");
			binding.Source = this;
			//this.SetBinding(IsCheckedProperty, binding);

			DataContextChanged += (_, _) =>
			{
				if (Menu != null)
				{
					Menu.DataContext = DataContext;
				}
			};
		}

		public ContextMenu Menu
		{
			get { return (ContextMenu)GetValue(MenuProperty); }
			set { SetValue(MenuProperty, value); }
		}

		public static readonly AvaloniaProperty MenuProperty = AvaloniaProperty.Register<DropDownButton,ContextMenu>(nameof(Menu));

		// private static void OnMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		// {
		// 	var dropDownButton = (DropDownButton)d;
		// 	var contextMenu = (ContextMenu)e.NewValue;
		// 	contextMenu.DataContext = dropDownButton.DataContext;
		// }

		protected override void OnClick()
		{
			if (Menu != null)
			{
				Menu.PlacementTarget = this;
				Menu.PlacementMode = PlacementMode.Bottom;
				//Menu.IsOpen = true;
			}
		}
	}
}
