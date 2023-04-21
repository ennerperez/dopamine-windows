using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Amphetamine.Controls
{
	public partial class Logo : UserControl
	{

		public Brush Accent
		{
			get { return (Brush)GetValue(AccentProperty); }

			set { SetValue(AccentProperty, value); }
		}

		public static readonly AvaloniaProperty AccentProperty = AvaloniaProperty.Register<Logo, Brush>("Accent");

		public Logo()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}

