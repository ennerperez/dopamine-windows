using System;
using Avalonia.Controls;

namespace Amphetamine.Controls
{
	public class DataGridEx : DataGrid
	{
		protected override void OnInitialized()
		{
			base.OnInitialized();

			if (DataContext != null && Columns.Count > 0)
			{
				foreach (var col in Columns)
				{
					//col.SetValue(FrameworkElement.DataContextProperty, this.DataContext);
				}
			}
		}
	}
}
