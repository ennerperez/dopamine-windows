using System;
using System.Collections.Generic;
using System.ComponentModel;
using Amphetamine.Core.Interfaces;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using Avalonia.Threading;

namespace Amphetamine.Controls
{

	public enum SpectrumAnimationStyle
	{
		Nervous = 1,
		Gentle
	}
	public class SpectrumAnalyzer : Control
    {
        private const double minDBValue = -90;
        private const double maxDBValue = 0;
        private const double dbScale = (maxDBValue - minDBValue);
        private const int defaultRefreshInterval = 25;

        private readonly DispatcherTimer animationTimer;
        private Canvas spectrumCanvas;
        private ISpectrumPlayer soundPlayer;
        private readonly List<Shape> barShapes = new List<Shape>();
        private double[] barHeights;
        private float[] channelData = new float[1024];
        private float[] channelPeakData;
        private double bandWidth = 1.0;
        private int maximumFrequency = 20000;
        private int maximumFrequencyIndex = 2047;
        private int minimumFrequency = 20;
        private int minimumFrequencyIndex;
        private int[] barIndexMax;
        private int[] barLogScaleIndexMax;
        private int peakFallDelay = 10;

        public static readonly AvaloniaProperty AnimationStyleProperty = AvaloniaProperty.Register<SpectrumAnalyzer,SpectrumAnimationStyle>("AnimationStyle", SpectrumAnimationStyle.Nervous);

        public SpectrumAnimationStyle AnimationStyle
        {
            get
            {
                return (SpectrumAnimationStyle)GetValue(AnimationStyleProperty);
            }
            set
            {
                SetValue(AnimationStyleProperty, value);
            }
        }

        public static readonly AvaloniaProperty BarBackgroundProperty = AvaloniaProperty.Register<SpectrumAnalyzer,IBrush>("BarBackground",  Brushes.White);//, OnBarBackgroundChanged());

        private static void OnBarBackgroundChanged(AvaloniaObject o, AvaloniaPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;

            if (spectrumAnalyzer.barShapes != null && spectrumAnalyzer.barShapes.Count > 0)
            {
                foreach (Shape bar in spectrumAnalyzer.barShapes)
                {
                    bar.Fill = spectrumAnalyzer.BarBackground;
                }
            }
        }

        public Brush BarBackground
        {
            get
            {
                return (Brush)GetValue(BarBackgroundProperty);
            }
            set
            {
                SetValue(BarBackgroundProperty, value);
            }
        }

        public static readonly AvaloniaProperty BarWidthProperty = AvaloniaProperty.Register<SpectrumAnalyzer,double>("BarWidth", 1.0); //, OnBarWidthChanged, OnCoerceBarWidth));

        private static object OnCoerceBarWidth(AvaloniaObject o, object value)
        {
            return value;
        }

        private static void OnBarWidthChanged(AvaloniaObject o, AvaloniaPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;

            if (spectrumAnalyzer.barShapes != null && spectrumAnalyzer.barShapes.Count > 0)
            {
                foreach (Shape bar in spectrumAnalyzer.barShapes)
                {
                    bar.Width = spectrumAnalyzer.BarWidth;
                }
            }
        }

        public double BarWidth
        {
            get
            {
                return (double)GetValue(BarWidthProperty);
            }
            set
            {
                SetValue(BarWidthProperty, value);
            }
        }

        public static readonly AvaloniaProperty BarCountProperty = AvaloniaProperty.Register<SpectrumAnalyzer, int>("BarCount", 32); //, OnBarCountChanged, OnCoerceBarCount));

        private static object OnCoerceBarCount(AvaloniaObject o, object value)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null) return spectrumAnalyzer.OnCoerceBarCount((int)value);
            return value;
        }

        private static void OnBarCountChanged(AvaloniaObject o, AvaloniaPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null) spectrumAnalyzer.OnBarCountChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected virtual int OnCoerceBarCount(int value)
        {
            value = Math.Max(value, 1);
            return value;
        }

        protected virtual void OnBarCountChanged(int oldValue, int newValue)
        {
            this.UpdateBarLayout();
        }

        public int BarCount
        {
            get
            {
                return (int)GetValue(BarCountProperty);
            }
            set
            {
                SetValue(BarCountProperty, value);
            }
        }

        public static readonly AvaloniaProperty BarSpacingProperty = AvaloniaProperty.Register<SpectrumAnalyzer,double>("BarSpacing", 5.0d); //, OnBarSpacingChanged, OnCoerceBarSpacing));

        private static object OnCoerceBarSpacing(AvaloniaObject o, object value)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;

            if (spectrumAnalyzer != null) return spectrumAnalyzer.OnCoerceBarSpacing((double)value);
            return value;
        }

        private static void OnBarSpacingChanged(AvaloniaObject o, AvaloniaPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null) spectrumAnalyzer.OnBarSpacingChanged((double)e.OldValue, (double)e.NewValue);
        }

        protected virtual double OnCoerceBarSpacing(double value)
        {
            value = Math.Max(value, 0);
            return value;
        }

        protected virtual void OnBarSpacingChanged(double oldValue, double newValue)
        {
            this.UpdateBarLayout();
        }

        public double BarSpacing
        {
            get
            {
                return (double)GetValue(BarSpacingProperty);
            }
            set
            {
                SetValue(BarSpacingProperty, value);
            }
        }

        public static readonly AvaloniaProperty RefreshIntervalProperty = AvaloniaProperty.Register<SpectrumAnalyzer,int>("RefreshInterval", defaultRefreshInterval); //, OnRefreshIntervalChanged, OnCoerceRefreshInterval));

        private static object OnCoerceRefreshInterval(AvaloniaObject o, object value)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null) return spectrumAnalyzer.OnCoerceRefreshInterval((int)value);
            return value;
        }

        private static void OnRefreshIntervalChanged(AvaloniaObject o, AvaloniaPropertyChangedEventArgs e)
        {
            SpectrumAnalyzer spectrumAnalyzer = o as SpectrumAnalyzer;
            if (spectrumAnalyzer != null) spectrumAnalyzer.OnRefreshIntervalChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected virtual int OnCoerceRefreshInterval(int value)
        {
            value = Math.Min(1000, Math.Max(10, value));
            return value;
        }

        protected virtual void OnRefreshIntervalChanged(int oldValue, int newValue)
        {
            animationTimer.Interval = TimeSpan.FromMilliseconds(newValue);
        }

        public int RefreshInterval
        {
            get
            {
                return (int)GetValue(RefreshIntervalProperty);
            }
            set
            {
                SetValue(RefreshIntervalProperty, value);
            }
        }

        static SpectrumAnalyzer()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(SpectrumAnalyzer), new FrameworkPropertyMetadata(typeof(SpectrumAnalyzer)));
        }

        public SpectrumAnalyzer()
        {
            this.animationTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle)
            {
                Interval = TimeSpan.FromMilliseconds(defaultRefreshInterval)
            };

            this.animationTimer.Tick += animationTimer_Tick;
        }

        public void OnApplyTemplate()
        {
            // this.spectrumCanvas = (Canvas)GetTemplateChild("PART_SpectrumCanvas");
            // this.spectrumCanvas.SizeChanged += spectrumCanvas_SizeChanged;
            this.UpdateBarLayout();
        }

        protected void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            // base.OnTemplateChanged(oldTemplate, newTemplate);
            // if (this.spectrumCanvas != null) this.spectrumCanvas.SizeChanged -= spectrumCanvas_SizeChanged;
        }

        protected void OnRender(DrawingContext dc)
        {
            //base.OnRender(dc);
            this.UpdateBarLayout();
            this.UpdateSpectrum();
        }

        protected void OnRenderSizeChanged() //SizeChangedInfo sizeInfo)
        {
            //base.OnRenderSizeChanged(sizeInfo);
            this.UpdateBarLayout();
            this.UpdateSpectrum();
        }

        public void RegisterSoundPlayer(ISpectrumPlayer soundPlayer)
        {
            this.UnregisterSoundPlayer();

            this.soundPlayer = soundPlayer;
            this.soundPlayer.PropertyChanged += soundPlayer_PropertyChanged;
            this.UpdateBarLayout();
            this.animationTimer.Start();
        }

        public void UnregisterSoundPlayer()
        {
            this.animationTimer.Stop();

            if (soundPlayer != null)
            {
                this.soundPlayer.PropertyChanged -= soundPlayer_PropertyChanged;
                this.soundPlayer = null;
            }
        }

        private void UpdateSpectrum()
        {
            if (this.soundPlayer == null || this.spectrumCanvas == null || this.spectrumCanvas.DesiredSize.Width < 1 || this.spectrumCanvas.DesiredSize.Height < 1)
            {
                return;
            }

            if (this.soundPlayer.IsPlaying && !this.soundPlayer.GetFFTData(ref this.channelData))
            {
                return;
            }

            this.UpdateSpectrumShapes();
        }

        private void UpdateSpectrumShapes()
        {
            try
            {
                bool allZero = true;
                double fftBucketHeight = 0f;
                double barHeight = 0f;
                double lastPeakHeight = 0f;
                double peakYPos = 0f;
                double height = spectrumCanvas.DesiredSize.Height;
                int barIndex = 0;

                for (int i = this.minimumFrequencyIndex; i <= this.maximumFrequencyIndex; i++)
                {
                    // If we're paused, keep drawing, but set the current height to 0 so the peaks fall.
                    if (!this.soundPlayer.IsPlaying)
                    {
                        barHeight = 0f;
                    }
                    else // Draw the maximum value for the bar's band
                    {
                        switch (this.AnimationStyle)
                        {
                            case SpectrumAnimationStyle.Nervous:
                                // Do nothing
                                break;
                            case SpectrumAnimationStyle.Gentle:
                                this.channelData[i] -= 0.003f;
                                break;
                            default:
                                break;
                        }

                        double dbValue = 20 * Math.Log10((double)this.channelData[i]);

                        fftBucketHeight = ((dbValue - minDBValue) / dbScale) * height;

                        if (barHeight < fftBucketHeight)
                        {
                            barHeight = fftBucketHeight;
                        }

                        if (barHeight < 0f)
                        {
                            barHeight = 0f;
                        }
                    }

                    // If this is the last FFT bucket in the bar's group, draw the bar.
                    if (i == this.barIndexMax[barIndex])
                    {
                        // Peaks can't surpass the height of the control.
                        if (barHeight > height)
                        {
                            barHeight = height;
                        }

                        peakYPos = barHeight;

                        if (this.channelPeakData[barIndex] < peakYPos)
                        {
                            this.channelPeakData[barIndex] = (float)peakYPos;
                        }
                        else
                        {
                            this.channelPeakData[barIndex] = (float)(peakYPos + (this.peakFallDelay * this.channelPeakData[barIndex])) / ((float)(this.peakFallDelay + 1));
                        }

                        double xCoord = this.BarSpacing + (this.BarWidth * barIndex) + (this.BarSpacing * barIndex) + 1;

                        switch (this.AnimationStyle)
                        {
                            case SpectrumAnimationStyle.Nervous:
                                this.barShapes[barIndex].Margin = new Thickness(xCoord, (height - 1) - barHeight, 0, 0);
                                this.barShapes[barIndex].Height = barHeight;
                                break;
                            case SpectrumAnimationStyle.Gentle:
                                this.barShapes[barIndex].Margin = new Thickness(xCoord, (height - 1) - this.channelPeakData[barIndex], 0, 0);
                                this.barShapes[barIndex].Height = this.channelPeakData[barIndex];
                                break;
                            default:
                                break;
                        }

                        if (this.channelPeakData[barIndex] > 0.05)
                        {
                            allZero = false;
                        }

                        lastPeakHeight = barHeight;
                        barHeight = 0f;
                        barIndex++;
                    }
                }

                if (!allZero || this.soundPlayer.IsPlaying)
                {
                    return;
                }

                this.animationTimer.Stop();
            }
            catch (IndexOutOfRangeException)
            {
                // Intended suppression.
            }
        }

        private void UpdateBarLayout()
        {
            if (this.soundPlayer == null || this.spectrumCanvas == null) return;

            this.maximumFrequencyIndex = Math.Min(this.soundPlayer.GetFFTFrequencyIndex(this.maximumFrequency) + 1, 2047);
            this.minimumFrequencyIndex = Math.Min(this.soundPlayer.GetFFTFrequencyIndex(this.minimumFrequency), 2047);
            this.bandWidth = Math.Max(((double)(this.maximumFrequencyIndex - this.minimumFrequencyIndex)) / this.spectrumCanvas.DesiredSize.Width, 1.0);

            int actualBarCount;

            if (this.BarWidth >= 1.0d)
            {
                actualBarCount = this.BarCount;
            }
            else
            {
                actualBarCount = Math.Max((int)((this.spectrumCanvas.DesiredSize.Width - this.BarSpacing) / (this.BarWidth + this.BarSpacing)), 1);
            }

            this.channelPeakData = new float[actualBarCount];

            int indexCount = this.maximumFrequencyIndex - this.minimumFrequencyIndex;
            int linearIndexBucketSize = (int)Math.Round((double)indexCount / (double)actualBarCount, 0);
            var maxIndexList = new List<int>();
            var maxLogScaleIndexList = new List<int>();
            double maxLog = Math.Log(actualBarCount, actualBarCount);

            for (int i = 1; i < actualBarCount; i++)
            {
                maxIndexList.Add(this.minimumFrequencyIndex + (i * linearIndexBucketSize));
                int logIndex = (int)((maxLog - Math.Log((actualBarCount + 1) - i, (actualBarCount + 1))) * indexCount) + this.minimumFrequencyIndex;
                maxLogScaleIndexList.Add(logIndex);
            }

            maxIndexList.Add(this.maximumFrequencyIndex);
            maxLogScaleIndexList.Add(this.maximumFrequencyIndex);
            this.barIndexMax = maxIndexList.ToArray();
            this.barLogScaleIndexMax = maxLogScaleIndexList.ToArray();

            this.barHeights = new double[actualBarCount];

            this.spectrumCanvas.Children.Clear();
            this.barShapes.Clear();

            double height = this.spectrumCanvas.DesiredSize.Height;

            for (int i = 0; i < actualBarCount; i++)
            {
                double xCoord = this.BarSpacing + (this.BarWidth * i) + (this.BarSpacing * i) + 1;
                Rectangle barRectangle = new Rectangle()
                {
                    Margin = new Thickness(xCoord, height, 0, 0),
                    Width = this.BarWidth,
                    Height = 0,
                    Fill = this.BarBackground
                };

                this.barShapes.Add(barRectangle);
            }

            foreach (Shape shape in barShapes) this.spectrumCanvas.Children.Add(shape);
        }

        private void soundPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsPlaying":
                    if (this.soundPlayer.IsPlaying && !this.animationTimer.IsEnabled)
                    {
                        this.animationTimer.Start();
                    }
                    break;
            }
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            this.UpdateSpectrum();
        }

        private void spectrumCanvas_SizeChanged(object sender, EventArgs e)
        {
            this.UpdateBarLayout();
        }
    }
}
