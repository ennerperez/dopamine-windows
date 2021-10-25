using System;
using System.Diagnostics;
using System.Drawing;

namespace Dopamine.Core.Utils
{
    /// <summary>
    /// Same as MS.Internal.PresentationCore
    /// Ref: https://gist.github.com/peterk87/5453080
    /// </summary>
    [DebuggerStepThrough]
    public static class ColorUtils
    {
        
        delegate byte ComponentSelector(Color color);
        static ComponentSelector _redSelector = color => color.R;
        static ComponentSelector _greenSelector = color => color.G;
        static ComponentSelector _blueSelector = color => color.B;
        public static Color InterpolateColor(Color from, Color to, double progress)
        {
            //return from + ((to - from) * (float)progress);
            if (progress < 0 || progress > 1) {
                throw new ArgumentOutOfRangeException("progress");
            }
            Color color = Color.FromArgb(
                InterpolateComponent(from, to, progress, _redSelector),
                InterpolateComponent(from, to, progress, _greenSelector),
                InterpolateComponent(from, to, progress, _blueSelector)
            );

            return color;
        }
        
        static byte InterpolateComponent(
            Color endPoint1,
            Color endPoint2,
            double lambda,
            ComponentSelector selector) {
            return (byte)(selector(endPoint1)
                          + (selector(endPoint2) - selector(endPoint1)) * lambda);
        }
    }
}