using System;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Win32WindowImpl=Avalonia.Win32.WindowImpl;

namespace Amphetamine.Dialogs
{
	public class MessageBox : Window
	{
#pragma warning disable 8601
		private static readonly MethodInfo s_getExtendedStyle = typeof(Win32WindowImpl).GetMethod("GetExtendedStyle", BindingFlags.NonPublic | BindingFlags.Instance);
		private static readonly MethodInfo s_setExtendedStyle = typeof(Win32WindowImpl).GetMethod("SetExtendedStyle", BindingFlags.NonPublic | BindingFlags.Instance);
#pragma warning restore 8601

		[Flags]
		private enum WindowStyles : uint
		{
			WS_EX_DLGMODALFRAME = 0x00000001
		}

		public enum MessageBoxButtons
		{
			Ok,
			OkCancel,
			YesNo,
			YesNoCancel
		}

		public enum MessageBoxResult
		{
			Ok,
			Cancel,
			Yes,
			No
		}

		public MessageBox()
		{
#pragma warning disable 8605
			if (PlatformImpl is Win32WindowImpl win)
			{
				var exStyle = (WindowStyles)s_getExtendedStyle.Invoke(win, null);
				exStyle |= WindowStyles.WS_EX_DLGMODALFRAME;
				s_setExtendedStyle.Invoke(win, new object[] {exStyle, true});
			}
#pragma warning restore 8605

			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public static Task<MessageBoxResult> Show(Window parent, string text, string title, MessageBoxButtons buttons)
		{
			var msgbox = new MessageBox() {Title = title, Icon = parent.Icon};

			msgbox.FindControl<TextBlock>("Text").Text = text;
			var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");

			var res = MessageBoxResult.Ok;

			void AddButton(string caption, MessageBoxResult r, bool def = false)
			{
				var btn = new Button {Content = caption};
				btn.Click += (_, _) =>
				{
					res = r;
					msgbox.Close();
				};
				buttonPanel.Children.Add(btn);
				if (def)
					res = r;
			}

			if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
				AddButton("Ok", MessageBoxResult.Ok, true);
			if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
			{
				AddButton("Yes", MessageBoxResult.Yes);
				AddButton("No", MessageBoxResult.No, true);
			}

			if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
				AddButton("Cancel", MessageBoxResult.Cancel, true);

			var x = (int)(parent.Position.X + (parent.Width / 2.0) - msgbox.Width / 2);
			var y = (int)(parent.Position.Y + (parent.Height / 2.0) - msgbox.Height / 2);
			msgbox.Position = new PixelPoint(x, y);

			var tcs = new TaskCompletionSource<MessageBoxResult>();
			msgbox.Closed += delegate { tcs.TrySetResult(res); };
			msgbox.ShowDialog(parent);
			return tcs.Task;
		}
	}
}
