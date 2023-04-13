using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Amphetamine.Controls
{
	public class CrossFadeContentControl : ContentControl
    {
        private ContentPresenter PART_MainContent;
        private Shape PART_PaintArea;

        public double Duration
        {
            get { return Convert.ToDouble(GetValue(DurationProperty)); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly AvaloniaProperty DurationProperty = AvaloniaProperty.Register<CrossFadeContentControl, double>("Duration", 0.5);

        static CrossFadeContentControl()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(CrossFadeContentControl), new FrameworkPropertyMetadata(typeof(CrossFadeContentControl)));
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            // this.PART_MainContent = (ContentPresenter)GetTemplateChild("PART_MainContent");
            // this.PART_PaintArea = (Shape)GetTemplateChild("PART_PaintArea");

            base.OnApplyTemplate(e);
        }

        // protected override void OnContentChanged(object oldContent, object newContent)
        // {
        //     try
        //     {
        //         if (PART_PaintArea != null && PART_MainContent != null)
        //         {
        //             PART_PaintArea.Fill = this.CreateBrushFromVisual(PART_MainContent);
        //             this.BeginAnimateContentReplacement();
        //         }
        //         base.OnContentChanged(oldContent, newContent);
        //     }
        //     catch (Exception)
        //     {
        //     }
        // }

        private Brush CreateBrushFromVisual(Visual iVisual)
        {

            if (iVisual == null)
            {
                throw new ArgumentNullException("iVisual");
            }

            dynamic target = new RenderTargetBitmap(new PixelSize(Convert.ToInt32(this.Width), Convert.ToInt32(this.Height)), new Vector(96,96)); //, PixelFormats.Pbgra32);
            target.Render(iVisual);
            dynamic brush = new ImageBrush(target);
            brush.Freeze();
            return brush;
        }

        private void BeginAnimateContentReplacement()
        {
            PART_PaintArea.Opacity = 1.0;

            // var fadeOutAnimation = new DoubleAnimation
            // {
            //     From = 1.0,
            //     To = 0.0,
            //     Duration = new Duration(TimeSpan.FromSeconds(this.Duration)),
            //     AutoReverse = false
            // };
            //
            // var fadeInAnimation = new DoubleAnimation
            // {
            //     From = 0.0,
            //     To = 1.0,
            //     Duration = new Duration(TimeSpan.FromSeconds(this.Duration)),
            //     AutoReverse = false
            // };
            //
            // PART_PaintArea.BeginAnimation(OpacityProperty, fadeOutAnimation);
            // PART_MainContent.BeginAnimation(OpacityProperty, fadeInAnimation);
        }
    }
}
