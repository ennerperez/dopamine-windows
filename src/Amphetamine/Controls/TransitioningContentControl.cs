using System;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Amphetamine.Controls
{
	public class TransitioningContentControl : ContentControl
    {
        private Timer timer;

        public static readonly AvaloniaProperty FadeInProperty = AvaloniaProperty.Register<TransitioningContentControl,bool>("FadeIn");
        public static readonly AvaloniaProperty FadeInTimeoutProperty = AvaloniaProperty.Register<TransitioningContentControl,bool>("FadeInTimeout");
        public static readonly AvaloniaProperty SlideInProperty = AvaloniaProperty.Register<TransitioningContentControl,bool>("SlideIn");
        public static readonly AvaloniaProperty SlideInTimeoutProperty = AvaloniaProperty.Register<TransitioningContentControl,bool>("SlideInTimeout");
        public static readonly AvaloniaProperty SlideInFromProperty = AvaloniaProperty.Register<TransitioningContentControl,bool>("SlideInFrom");
        public static readonly AvaloniaProperty SlideInToProperty = AvaloniaProperty.Register<TransitioningContentControl,bool>("SlideInTo");
        public static readonly AvaloniaProperty RightToLeftProperty = AvaloniaProperty.Register<TransitioningContentControl,bool>("RightToLeft");

        //public static readonly RoutedEvent ContentChangedEvent = EventManager.RegisterRoutedEvent("ContentChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TransitioningContentControl));

        // public event EventHandler ContentChanged
        // {
        //     add { this.AddHandler(ContentChangedEvent, value); }
        //
        //     remove { this.RemoveHandler(ContentChangedEvent, value); }
        //
        // }
        private void RaiseContentChangedEvent()
        {
            //var newEventArgs = new RoutedEventArgs(TransitioningContentControl.ContentChangedEvent);
            //base.RaiseEvent(newEventArgs);
        }

        public bool FadeIn
        {
            get { return Convert.ToBoolean(GetValue(FadeInProperty)); }

            set { SetValue(FadeInProperty, value); }
        }

        public double FadeInTimeout
        {
            get { return Convert.ToDouble(GetValue(FadeInTimeoutProperty)); }

            set { SetValue(FadeInTimeoutProperty, value); }
        }

        public bool SlideIn
        {
            get { return Convert.ToBoolean(GetValue(SlideInProperty)); }

            set { SetValue(SlideInProperty, value); }
        }

        public double SlideInTimeout
        {
            get { return Convert.ToDouble(GetValue(SlideInTimeoutProperty)); }

            set { SetValue(SlideInTimeoutProperty, value); }
        }

        public int SlideInFrom
        {
            get { return Convert.ToInt32(GetValue(SlideInFromProperty)); }

            set { SetValue(SlideInFromProperty, value); }
        }

        public int SlideInTo
        {
            get { return Convert.ToInt32(GetValue(SlideInToProperty)); }

            set { SetValue(SlideInToProperty, value); }
        }

        public bool RightToLeft
        {
            get { return Convert.ToBoolean(GetValue(RightToLeftProperty)); }

            set { SetValue(RightToLeftProperty, value); }
        }

        protected void OnContentChanged(object oldContent, object newContent)
        {
            this.DoAnimation();
        }

        private void DoAnimation()
        {
            // if (this.FadeInTimeout != null && this.FadeIn)
            // {
            //     var da = new DoubleAnimation();
            //     da.From = 0;
            //     da.To = 1;
            //     da.Duration = new Duration(TimeSpan.FromSeconds(this.FadeInTimeout));
            //     this.BeginAnimation(OpacityProperty, da);
            // }


            // if (this.SlideInTimeout != null && this.SlideInTimeout > 0 && this.SlideIn)
            // {
            //     if (!this.RightToLeft)
            //     {
            //         var ta = new ThicknessAnimation();
            //         ta.From = new Thickness(this.SlideInFrom, this.Margin.Top, 2 * this.SlideInTo - this.SlideInFrom, this.Margin.Bottom);
            //         ta.To = new Thickness(this.SlideInTo, this.Margin.Top, this.SlideInTo, this.Margin.Bottom);
            //         ta.Duration = new Duration(TimeSpan.FromSeconds(this.SlideInTimeout));
            //         this.BeginAnimation(MarginProperty, ta);
            //     }
            //     else
            //     {
            //         var ta = new ThicknessAnimation();
            //         ta.From = new Thickness(2 * this.SlideInTo - this.SlideInFrom, this.Margin.Top, this.SlideInFrom, this.Margin.Bottom);
            //         ta.To = new Thickness(this.SlideInTo, this.Margin.Top, this.SlideInTo, this.Margin.Bottom);
            //         ta.Duration = new Duration(TimeSpan.FromSeconds(this.SlideInTimeout));
            //         this.BeginAnimation(MarginProperty, ta);
            //     }
            // }

            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Elapsed -= new ElapsedEventHandler(this.TimerElapsedHandler);
            }

            this.timer = new Timer();

            double biggestTimeout = this.SlideInTimeout;

            if (this.FadeInTimeout > this.SlideInTimeout)
            {
                biggestTimeout = this.FadeInTimeout;
            }

            this.timer.Interval = TimeSpan.FromSeconds(biggestTimeout).TotalMilliseconds;

            this.timer.Elapsed += new ElapsedEventHandler(this.TimerElapsedHandler);

            this.timer.Start();
        }

        private void TimerElapsedHandler(object sender, ElapsedEventArgs e)
        {
            this.timer.Stop();

            try
            {
                //Application.Current.Dispatcher.BeginInvoke(new Action(() => this.RaiseContentChangedEvent()));
            }
            catch (Exception)
            {
            }
        }
    }
}
