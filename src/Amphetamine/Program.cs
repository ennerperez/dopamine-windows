using Avalonia;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Amphetamine.Core;
using Amphetamine.Data;
using Amphetamine.Data.Contexts;
using Amphetamine.Dialogs;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.ReactiveUI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Prism.DryIoc;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

namespace Amphetamine
{
	internal class Program
	{
		public static bool IsRunning { get; internal set; } = false;

		public static ILogger Logger { get; private set; }
		public static IConfiguration Configuration { get; private set; }
		public static IServiceCollection Services { get; private set; }
		public static IServiceProvider Container { get; private set; }

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

			Initialize(args);

			double GetScaling()
			{
				var idx = Array.IndexOf(args, "--scaling");
				if (idx != 0 && args.Length > idx + 1 &&
				    double.TryParse(args[idx + 1], NumberStyles.Any, CultureInfo.InvariantCulture, out var scaling))
					return scaling;
				return 1;
			}

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

				Logger.LogInformation("Application Starting");
				var builder = CreateAppBuilder();
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
				Logger.LogError(ex, "The Application failed to start");
				throw;
			}

		}

		private static void Initialize(string[] args)
		{
			var services = new ServiceCollection();

			var config = Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#if DEBUG
				.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
#endif
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			// Initialize Logger
			var logger = Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config)
				.CreateLogger();

			services.AddSingleton(logger);

			services.AddLogging(builder =>
			{
				builder.ClearProviders();
				builder.AddSerilog(logger);
			}).AddOptions();

			services
				.AddCore()
				.AddData<DefaultContext>(m=> m.UseDbEngine(Configuration, providerName: DatabaseProviders.Sqlite), ServiceLifetime.Transient);

			Services = services;
			Container = Services.BuildServiceProvider();

			var factory = Container.GetService<ILoggerFactory>();
			if (factory != null)
				Logger = factory.CreateLogger(typeof(Program));

			var dbContext = Container.GetService<DbContext>();
			if (dbContext != null)
				dbContext.Initialize();
		}
		private static async void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = (Exception)e.ExceptionObject;
			if (Logger != null)
			{
				Logger.LogError(ex, "{Message}", ex.Message);
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine(ex.Message);
				Console.ResetColor();
			}
			//await MessageBox.Show((Window)(Application.Current as PrismApplication)?.MainWindow!, ex.Message, "Error", MessageBox.MessageBoxButtons.Ok);
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
		public static AppBuilder CreateAppBuilder() =>
			AppBuilder
				.Configure<App>()
				.UsePlatformDetect()
				.With(new X11PlatformOptions {EnableMultiTouch = true, UseDBusMenu = true})
				.With(new Win32PlatformOptions {EnableMultitouch = true, AllowEglInitialization = true})
				.UseSkia()
				.UseReactiveUI()
				.UseManagedSystemDialogs()
				.LogToTrace();
	}
}
