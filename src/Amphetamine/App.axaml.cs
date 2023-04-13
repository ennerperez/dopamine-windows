using System;
using System.IO;
using System.Linq;
using Amphetamine.Core;
using Amphetamine.Shell;
using Amphetamine.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Amphetamine
{
	public class App : PrismApplication
	{

		public static bool IsSingleViewLifetime =>
			Environment.GetCommandLineArgs()
				.Any(a => a == "--fbdev" || a == "--drm");

		public static AppBuilder BuildAvaloniaApp() =>
			AppBuilder
				.Configure<App>()
				.UsePlatformDetect();
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
			base.Initialize();// <-- Required
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterInstance(Program.Configuration);
			containerRegistry.RegisterInstance(Program.Logger);

			// Register Services
			//containerRegistry.Register<IRestService, RestService>();

			// services.AddSingleton(logger);
			//
			// services.AddLogging(builder =>
			// {
			// 	builder.ClearProviders();
			// 	builder.AddSerilog(logger);
			// }).AddOptions();
			//
			// services
			// 	.AddCore()
			// 	.AddData<DefaultContext>(m => m.UseDbEngine(Configuration, providerName: DatabaseProviders.Sqlite), ServiceLifetime.Transient)
			// 	.AddServices();
			//
			// Services = services;
			// Container = Services.BuildServiceProvider();
			//
			// var factory = Container.GetService<ILoggerFactory>();
			// if (factory != null)
			// 	Logger = factory.CreateLogger(typeof(Program));
			//
			// var dbContext = Container.GetService<DbContext>();
			// if (dbContext != null)
			// 	dbContext.Initialize();

			// Views - Generic
			containerRegistry.Register<MainWindow>();

			containerRegistry
				.AddCore()
				.AddServices();

			// Views - Region Navigation
			// containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();
			// containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
			// containerRegistry.RegisterForNavigation<SidebarView, SidebarViewModel>();
		}

		protected override AvaloniaObject CreateShell()
		{
			// if (IsSingleViewLifetime)
			// 	return Container.Resolve<MainControl>();// For Linux Framebuffer or DRM
			// else
			return Container.Resolve<MainWindow>();
		}

		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			// Register modules
			// moduleCatalog.AddModule<Module1.Module>();
			// moduleCatalog.AddModule<Module2.Module>();
			// moduleCatalog.AddModule<Module3.Module>();
		}

		/// <summary>Called after <seealso cref="Initialize"/>.</summary>
		protected override void OnInitialized()
		{
			// Register initial Views to Region.
			var regionManager = Container.Resolve<IRegionManager>();
			// regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DashboardView));
			// regionManager.RegisterViewWithRegion(RegionNames.SidebarRegion, typeof(SidebarView));
		}

	}
}
