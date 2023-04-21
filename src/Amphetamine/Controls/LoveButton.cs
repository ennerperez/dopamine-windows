using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Amphetamine.Controls
{
	public class LoveButton : Control
    {
        private Button loveButton;
        private TextBlock heartFill;
        private TextBlock heart;

        public bool Love
        {
            get { return Convert.ToBoolean(GetValue(LoveProperty)); }
            set { SetValue(LoveProperty, value); }
        }

        public static readonly AvaloniaProperty LoveProperty =
	        AvaloniaProperty.Register<LoveButton, bool>(nameof(Love));

        public new double FontSize
        {
            get { return Convert.ToDouble(GetValue(FontSizeProperty)); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static new readonly AvaloniaProperty FontSizeProperty =
	        AvaloniaProperty.Register<LoveButton, double>(nameof(FontSize), 14.0);

        public Brush SelectedForeground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }

        public static readonly AvaloniaProperty SelectedForegroundProperty =
	        AvaloniaProperty.Register<LoveButton, Brush>(nameof(SelectedForeground));

        static LoveButton()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(LoveButton), new FrameworkPropertyMetadata(typeof(LoveButton)));
        }

        protected void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            //base.OnApplyTemplate(e);

            // this.loveButton = (Button)GetTemplateChild("PART_LoveButton");
            // this.heartFill = (TextBlock)GetTemplateChild("PART_HeartFill");
            // this.heart = (TextBlock)GetTemplateChild("PART_Heart");

            if (this.loveButton != null)
            {
                this.loveButton.Click += LoveButton_Click;
                this.loveButton.DoubleTapped += LoveButton_PreviewMouseDoubleClick;
            }

            if (this.heartFill != null)
            {
                this.heartFill.PointerEnter += HeartFill_MouseEnter;
                this.heartFill.PointerLeave += HeartFill_MouseLeave;
            }

            if (this.heart != null)
            {
                this.heart.PointerEnter += Heart_MouseEnter;
                this.heart.PointerLeave += Heart_MouseLeave;
            }
        }

        private void Heart_MouseEnter(object sender, PointerEventArgs e)
        {
            this.heart.Opacity = 1.0;
        }

        private void Heart_MouseLeave(object sender, PointerEventArgs e)
        {
            this.heart.Opacity = 0.2;
        }

        private void HeartFill_MouseEnter(object sender, PointerEventArgs e)
        {
            this.heartFill.Text = char.ConvertFromUtf32(0xE00C);
        }

        private void HeartFill_MouseLeave(object sender, PointerEventArgs e)
        {
            this.heartFill.Text = char.ConvertFromUtf32(0xE0A5);
        }

        private void LoveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Love = !this.Love;
        }

        private void LoveButton_PreviewMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            // This prevents other double click actions while rating, like playing the selected song.
            e.Handled = true;
        }
    }
}
