using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Amphetamine.Core.Utils
{
    public static class ResourceUtils
    {
        public static string GetString(string resourceName)
        {
            object resource;
            Application.Current.TryFindResource((object)resourceName, out resource);
            return resource != null ? resource.ToString() : resourceName;
        }

        public static Geometry GetGeometry(string resourceName)
        {
            object result;
            Application.Current.TryFindResource((object)resourceName, out result);
            return (Geometry)result;
        }
    }
}