using System;
using System.Linq;
using System.Windows;
using Avalonia;
using Avalonia.Controls;

namespace Dopamine.Core.Extensions
{
    public static class WindowExtensions
    {
        public static void SetGeometry(this Window win, double top, double left, double width, double height, double topFallback = 50, double leftFallback = 50)
        {
            var ps = win.Screens.Primary;
            // if (left <= (SystemParameters.VirtualScreenLeft - width) ||
            //     top <= (SystemParameters.VirtualScreenTop - height) ||
            //     (SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth) <= left ||
            //     (SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight) <= top)
            if (left <= (ps.Bounds.X - width) ||
                top <= (ps.Bounds.Y - height) || 
                (ps.Bounds.X + ps.Bounds.Width) <= left ||
                (ps.Bounds.Y + ps.Bounds.Height) <= top)
            {
                top = topFallback;
                left = leftFallback;
            }

            win.Position = new PixelPoint(Convert.ToInt32(top), Convert.ToInt32(left));
            // win.Top = Convert.ToInt32(top);
            // win.Left = Convert.ToInt32(left);
            win.Width = width;
            win.Height = height;
        }
    }
}
