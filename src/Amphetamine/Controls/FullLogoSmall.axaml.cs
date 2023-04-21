using System.Reflection;
using Amphetamine.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Amphetamine.Controls
{
	public partial class FullLogoSmall : UserControl
	{

		public Brush Accent
		{
			get { return (Brush)GetValue(AccentProperty); }
			set { SetValue(AccentProperty, value); }
		}

		public static readonly AvaloniaProperty AccentProperty = AvaloniaProperty.Register<FullLogoSmall, Brush>(nameof(Accent));

		public string ApplicationName
		{
			get { return Program.Assembly.Product(); }
		}
		public FullLogoSmall()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}

