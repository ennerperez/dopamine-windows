using System;
using System.Diagnostics;
using Avalonia.Media;

namespace Amphetamine.Core.Utils
{
    /// <summary>
    /// Same as MS.Internal.PresentationCore
    /// </summary>
    [DebuggerStepThrough]
    public static class ColorUtils
    {
        delegate byte ComponentSelector(Color color);

        static ComponentSelector _redSelector = color => color.R;
        static ComponentSelector _greenSelector = color => color.G;
        static ComponentSelector _blueSelector = color => color.B;

        static byte InterpolateComponent(
            Color endPoint1,
            Color endPoint2,
            double lambda,
            ComponentSelector selector)
        {
            return (byte)(selector(endPoint1)
                          + (selector(endPoint2) - selector(endPoint1)) * lambda);
        }

        public static Color InterpolateColor(Color from, Color to, double progress)
        {
            //return from + ((to - from) * (float)progress);
            if (progress < 0 || progress > 1)
                throw new ArgumentOutOfRangeException("progress");

            Color color = Color.FromRgb(
                InterpolateComponent(from, to, progress, _redSelector),
                InterpolateComponent(from, to, progress, _greenSelector),
                InterpolateComponent(from, to, progress, _blueSelector)
            );

            return color;
        }
    }
}