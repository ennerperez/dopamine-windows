using System.Reflection;
using Avalonia.Controls.Primitives;

namespace Amphetamine.Controls
{
	public class PopupEx : Popup
	{
		//private static FieldInfo fi = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);

		public void Open()
		{
			// if (SystemParameters.MenuDropAlignment)
			// {
			// 	fi.SetValue(null, false);
			// 	this.IsOpen = true;
			// 	fi.SetValue(null, true);
			// }
			// else
			// {
			// 	this.IsOpen = true;
			// }
			this.IsOpen = true;
		}

		public void Close()
		{
			this.IsOpen = false;
		}
	}
}
