using System;
using System.IO;
using System.Reflection;

namespace Amphetamine.Core.IO
{
    public static class ProcessExecutable
    {
        public static string ExecutionFolder() => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public static string Name() => Assembly.GetEntryAssembly().GetName().Name;

        public static Version AssemblyVersion() => Assembly.GetEntryAssembly().GetName().Version;
    }
}