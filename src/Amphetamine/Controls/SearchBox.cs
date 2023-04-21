using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Amphetamine.Controls
{
	public class SearchBox : TextBox
	{
		private Border searchBorder;

		public bool HasText
		{
			get { return Convert.ToBoolean(GetValue(HasTextProperty)); }

			set { SetValue(HasTextProperty, value); }
		}

		public bool HasFocus
		{
			get { return Convert.ToBoolean(GetValue(HasFocusProperty)); }

			set { SetValue(HasFocusProperty, value); }
		}

		public static readonly AvaloniaProperty HasTextProperty =
			AvaloniaProperty.Register<SearchBox, bool>(nameof(HasText), false);

		public static readonly AvaloniaProperty HasFocusProperty =
			AvaloniaProperty.Register<SearchBox, bool>(nameof(HasFocus), false);

		static SearchBox()
		{
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));
		}

		// public override void OnApplyTemplate(TemplateAppliedEventArgs e)
		// {
		// 	base.OnApplyTemplate(e);
		//
		// 	this.searchBorder = (Border)GetTemplateChild("PART_SearchBorder");
		//
		// 	if (this.searchBorder != null)
		// 	{
		// 		this.searchBorder.MouseLeftButtonUp += SearchButton_MouseLeftButtonUphandler;
		// 	}
		// }

		protected void OnTextChanged()//TextChangedEventArgs e)
		{
			//base.OnTextChanged(e);
			this.HasText = this.Text.Length > 0;
		}

		private void SearchButton_MouseLeftButtonUphandler(object sender, PointerEventArgs e)
		{
			if (this.HasText)
			{
				this.Text = string.Empty;
			}
		}

		public void SetKeyboardFocus()
		{
			this.Focus();
		}
	}
}
