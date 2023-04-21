using Avalonia;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Amphetamine.Core;
using Amphetamine.Data;
using Amphetamine.Data.Contexts;
using Amphetamine.Dialogs;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger=Serilog.ILogger;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

namespace Amphetamine
{
	internal class Program
	{
		public static bool IsRunning { get; internal set; } = false;

		public static ILogger Logger { get; private set; }

		public static IConfiguration Configuration { get; private set; }

		public static Assembly Assembly { get; private set; } = Assembly.GetAssembly(typeof(Program));

		public static IServiceProvider ServiceProvider  { get; internal set; }

		public static bool IsSingleViewLifetime =>
			Environment.GetCommandLineArgs()
				.Any(a => a == "--fbdev" || a == "--drm");

		// Initialization code. Don't use any Avalonia, third-party APIs or any
		// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		// yet and stuff might break.
		[STAThread]
		public static int Main(string[] args)
		{

			AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;

			var config = Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(new EmbeddedFileProvider(typeof(Program).Assembly, typeof(Program).Namespace), "appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile("usersettings.json", optional: false, reloadOnChange: true)
#if DEBUG
				.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
				.AddJsonFile("usersettings.Development.json", optional: true, reloadOnChange: true)
#endif
				.AddEnvironmentVariables()
				.Build();

			// Initialize Logger
			Logger = Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config)
				.CreateLogger();

			// double GetScaling()
			// {
			// 	var idx = Array.IndexOf(args, "--scaling");
			// 	if (idx != 0 && args.Length > idx + 1 &&
			// 	    double.TryParse(args[idx + 1], NumberStyles.Any, CultureInfo.InvariantCulture, out var scaling))
			// 		return scaling;
			// 	return 1;
			// }

			try
			{
				IsRunning = true;

				if (args.Contains("--wait-for-attach"))
				{
					Console.WriteLine("Attach debugger and use 'Set next statement'");
					while (true)
					{
						Thread.Sleep(100);
						if (Debugger.IsAttached)
							break;
					}
				}

				Logger.Information("Application Starting");
				var builder = BuildAvaloniaApp();
				// if (args.Contains("--fbdev"))
				// {
				// 	SilenceConsole();
				// 	return builder.StartLinuxFbDev(args, scaling: GetScaling());
				// }
				// else if (args.Contains("--drm"))
				// {
				// 	SilenceConsole();
				// 	return builder.StartLinuxDrm(args, scaling: GetScaling());
				// }
				// else
				return builder.StartWithClassicDesktopLifetime(args);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "The Application failed to start");
				throw;
			}

		}

		private static async void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = (Exception)e.ExceptionObject;
			if (Logger != null)
			{
				Logger.Error(ex, "{Message}", ex.Message);
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine(ex.Message);
				Console.ResetColor();
			}
			//await MessageBox.Show((Window)(Application.Current as Application)?.MainWindow!, ex.Message, "Error", MessageBox.MessageBoxButtons.Ok);
		}

		private static void SilenceConsole()
		{
			new Thread(() =>
			{
				Console.CursorVisible = false;
				while (true)
					Console.ReadKey(true);
				// ReSharper disable once FunctionNeverReturns
			}) {IsBackground = true}.Start();
		}

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp() =>
			AppBuilder
				.Configure<App>()
				.UsePlatformDetect()
				.LogToTrace()
				.With(new X11PlatformOptions {EnableMultiTouch = true, UseDBusMenu = true})
				.With(new Win32PlatformOptions {EnableMultitouch = true, AllowEglInitialization = true})
				.UseSkia()
				.UseManagedSystemDialogs();
	}
}
