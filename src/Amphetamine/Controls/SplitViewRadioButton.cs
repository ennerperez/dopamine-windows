using System;
using Avalonia;
using Avalonia.Controls;

namespace Amphetamine.Controls
{
	public class SplitViewRadioButton : RadioButton
	{
		public String Icon
		{
			get { return (String)GetValue(IconProperty); }

			set { SetValue(IconProperty, value); }
		}

		public static readonly AvaloniaProperty IconProperty =
			AvaloniaProperty.Register<SplitViewRadioButton,string>(nameof(Icon));

		static SplitViewRadioButton()
		{
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitViewRadioButton), new FrameworkPropertyMetadata(typeof(SplitViewRadioButton)));
		}
	}
}
