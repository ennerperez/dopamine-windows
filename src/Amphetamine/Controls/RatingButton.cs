using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using ExCSS;

namespace Amphetamine.Controls
{
	public class RatingButton : Control
    {
        private Button ratingButton;
        private StackPanel ratingStars;
        private StackPanel adjustmentStars;
        private TextBlock adjustmentStar1;
        private TextBlock adjustmentStar2;
        private TextBlock adjustmentStar3;
        private TextBlock adjustmentStar4;
        private TextBlock adjustmentStar5;

        public int Rating
        {
            get { return Convert.ToInt32(GetValue(RatingProperty)); }
            set { SetValue(RatingProperty, value); }
        }

        public static readonly AvaloniaProperty RatingProperty =
            AvaloniaProperty.Register<RatingButton, int>(nameof(Rating));

        public new double FontSize
        {
            get { return Convert.ToDouble(GetValue(FontSizeProperty)); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static new readonly AvaloniaProperty FontSizeProperty =
            AvaloniaProperty.Register<RatingButton,double>(nameof(FontSize),11.0);

        static RatingButton()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(RatingButton), new FrameworkPropertyMetadata(typeof(RatingButton)));
        }

        protected void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            //base.OnApplyTemplate(e);

            // this.ratingButton = (Button)GetTemplateChild("PART_RatingButton");
            //
            // this.ratingStars = (StackPanel)GetTemplateChild("PART_RatingStars");
            // this.adjustmentStars = (StackPanel)GetTemplateChild("PART_AdjustmentStars");
            //
            // this.adjustmentStar1 = (TextBlock)GetTemplateChild("PART_AdjustmentStar1");
            // this.adjustmentStar2 = (TextBlock)GetTemplateChild("PART_AdjustmentStar2");
            // this.adjustmentStar3 = (TextBlock)GetTemplateChild("PART_AdjustmentStar3");
            // this.adjustmentStar4 = (TextBlock)GetTemplateChild("PART_AdjustmentStar4");
            // this.adjustmentStar5 = (TextBlock)GetTemplateChild("PART_AdjustmentStar5");

            if (this.ratingButton != null)
            {
                this.ratingButton.Click += RatingButton_Click;
                this.ratingButton.PointerEnter += RatingButton_PointerEnter;
                this.ratingButton.PointerLeave += RatingButton_PointerLeave;
                this.ratingButton.DoubleTapped += RatingButton_PreviewPointerDoubleClick;
            }

            if (this.adjustmentStar1 != null)
            {
                this.adjustmentStar1.PointerEnter += AdjustmentStar1_PointerEnter;
            }

            if (this.adjustmentStar2 != null)
            {
                this.adjustmentStar2.PointerEnter += AdjustmentStar2_PointerEnter;
            }

            if (this.adjustmentStar3 != null)
            {
                this.adjustmentStar3.PointerEnter += AdjustmentStar3_PointerEnter;
            }

            if (this.adjustmentStar4 != null)
            {
                this.adjustmentStar4.PointerEnter += AdjustmentStar4_PointerEnter;
            }

            if (this.adjustmentStar5 != null)
            {
                this.adjustmentStar5.PointerEnter += AdjustmentStar5_PointerEnter;
            }

            this.ratingStars.IsVisible = true; //Visibility.Visible;
            this.adjustmentStars.IsVisible = false; //Visibility.Collapsed;
        }

        private void RatingButton_PreviewPointerDoubleClick(object sender, RoutedEventArgs e)
        {
            // This prevents other double click actions while rating, like playing the selected song.
            e.Handled = true;
        }

        private void RatingButton_PointerEnter(object sender, PointerEventArgs e)
        {
            this.ratingStars.IsVisible = false; //Visibility.Hidden;
            this.adjustmentStars.IsVisible = true; //Visibility.Visible;
        }

        private void RatingButton_PointerLeave(object sender, PointerEventArgs e)
        {
            this.ratingStars.IsVisible = true; //Visibility.Visible;
            this.adjustmentStars.IsVisible = false; //Visibility.Hidden;
        }

        private void ApplyRating(int newRating)
        {
            if(this.Rating.Equals(newRating))
            {
                // Clear rating
                this.Rating = 0;
                return;
            }

            this.Rating = newRating;
        }

        private void RatingButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.adjustmentStar1.IsPointerOver)
            {
                this.ApplyRating(1);
            }
            else if (this.adjustmentStar2.IsPointerOver)
            {
                this.ApplyRating(2);
            }
            else if (this.adjustmentStar3.IsPointerOver)
            {
                this.ApplyRating(3);
            }
            else if (this.adjustmentStar4.IsPointerOver)
            {
                this.ApplyRating(4);
            }
            else if (this.adjustmentStar5.IsPointerOver)
            {
                this.ApplyRating(5);
            }
        }

        private void AdjustmentStar1_PointerEnter(object sender, PointerEventArgs e)
        {
            this.adjustmentStar1.Opacity = 1.0;
            this.adjustmentStar2.Opacity = 0.2;
            this.adjustmentStar3.Opacity = 0.2;
            this.adjustmentStar4.Opacity = 0.2;
            this.adjustmentStar5.Opacity = 0.2;
        }

        private void AdjustmentStar2_PointerEnter(object sender, PointerEventArgs e)
        {
            this.adjustmentStar1.Opacity = 1.0;
            this.adjustmentStar2.Opacity = 1.0;
            this.adjustmentStar3.Opacity = 0.2;
            this.adjustmentStar4.Opacity = 0.2;
            this.adjustmentStar5.Opacity = 0.2;
        }

        private void AdjustmentStar3_PointerEnter(object sender, PointerEventArgs e)
        {
            this.adjustmentStar1.Opacity = 1.0;
            this.adjustmentStar2.Opacity = 1.0;
            this.adjustmentStar3.Opacity = 1.0;
            this.adjustmentStar4.Opacity = 0.2;
            this.adjustmentStar5.Opacity = 0.2;
        }

        private void AdjustmentStar4_PointerEnter(object sender, PointerEventArgs e)
        {
            this.adjustmentStar1.Opacity = 1.0;
            this.adjustmentStar2.Opacity = 1.0;
            this.adjustmentStar3.Opacity = 1.0;
            this.adjustmentStar4.Opacity = 1.0;
            this.adjustmentStar5.Opacity = 0.2;
        }

        private void AdjustmentStar5_PointerEnter(object sender, PointerEventArgs e)
        {
            this.adjustmentStar1.Opacity = 1.0;
            this.adjustmentStar2.Opacity = 1.0;
            this.adjustmentStar3.Opacity = 1.0;
            this.adjustmentStar4.Opacity = 1.0;
            this.adjustmentStar5.Opacity = 1.0;
        }
    }
}
