using System;
using System.Diagnostics;

namespace Amphetamine.Core.Utils
{
    public static class EnvironmentUtils
    {
        public static string GetFriendlyWindowsVersion()
        {
            try
            {
                //TODO
                // object obj = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>().Select<ManagementObject, object>((Func<ManagementObject, object>) (x => x.GetPropertyValue("Caption"))).FirstOrDefault<object>();
                // return obj != null ? obj.ToString() : "Unknown";
                return "Unknown";
            }
            catch (Exception ex)
            {
                return "Unknown";
            }
        }

        public static string GetInternalWindowsVersion() => Environment.OSVersion.VersionString;

        public static bool IsWindows10()
        {
            bool flag = false;
            OperatingSystem osVersion = Environment.OSVersion;
            Version version = osVersion.Version;
            if (osVersion.Platform == PlatformID.Win32NT && version.Major == 10)
                flag = true;
            return flag;
        }

        public static bool IsSingleInstance(string processName)
        {
            Process[] processesByName = Process.GetProcessesByName(processName);
            return !(processesByName.Length > 1 | processesByName.Length == 0);
        }
    }
}