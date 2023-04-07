using System;
using System.Linq;
using Amphetamine.Core;
using Amphetamine.Data;
using Amphetamine.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Amphetamine.Console
{
	internal class Program
	{

		public static ILogger Logger { get; private set; }
		public static IConfiguration Configuration { get; private set; }
		public static IServiceCollection Services { get; private set; }
		public static IServiceProvider Container { get; private set; }

		[STAThread]
		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledException;

			Initialize(args);

			var dbContext = Container.GetService<DbContext>();
			dbContext.Initialize();

		}
		private static void Initialize(string[] args)
		{
			var services = new ServiceCollection();

			var config = Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false, true)
#if DEBUG
				.AddJsonFile("appsettings.Development.json", true, true)
#endif
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			services.AddSingleton(Configuration);

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
				.AddData<DefaultContext>(m=> m.UseDbEngine(Configuration, providerName: DatabaseProviders.Sqlite ), ServiceLifetime.Singleton);

			Services = services;
			Container = Services.BuildServiceProvider();

			var factory = Container.GetService<ILoggerFactory>();
			if (factory != null)
				Logger = factory.CreateLogger(typeof(Program));
		}

		private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = (Exception)e.ExceptionObject;
			if (Logger != null)
			{
				Logger.LogError(ex, "{Message}", ex.Message);
			}
			else
			{
				System.Console.ForegroundColor = ConsoleColor.DarkRed;
				System.Console.WriteLine(ex.Message);
				System.Console.ResetColor();
			}
		}
	}
}
