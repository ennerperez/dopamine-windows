using System;
using System.Reflection;
using System.Threading;
using Amphetamine.Core;
using Amphetamine.Data;
using Amphetamine.Data.Contexts;
using Amphetamine.Shell;
using Amphetamine.ViewModels;
using Amphetamine.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Amphetamine
{
	public partial class App : Application
	{

#pragma warning disable CS0414
		private static Mutex s_instanceMutex = null;
 #pragma warning restore CS0414
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);

			var services = new ServiceCollection();

			services
				.AddCore()
				.AddData<DefaultContext>(options => options.UseDbEngine(Program.Configuration), ServiceLifetime.Singleton)
				//.AddServices()
				.AddShell();

			Program.ServiceProvider = services.BuildServiceProvider();
		}

		protected void OnStartup(EventArgs e)
		{
			// Create a jump-list and assign it to the current application
			//JumpList.SetJumpList(Current, new JumpList());

			// Check that there is only one instance of the application running
			s_instanceMutex = new Mutex(true, $"{Program.Assembly.Guid()}-{Program.Assembly.Version()}", out var isNewInstance);

			// Process the command-line arguments
			ProcessCommandLineArguments(isNewInstance);

			if (isNewInstance)
			{
				s_instanceMutex.ReleaseMutex();
				CreateOrMigrateDatabase();
				//base.OnStartup(e);
			}
			else
			{
				Program.Logger.Warning("{Name} is already running. Shutting down", Program.Assembly.Product());
				//this.Shutdown();
			}

		}
		private void ProcessCommandLineArguments(bool isNewInstance)
		{
			// Get the command-line arguments
			var args = Environment.GetCommandLineArgs();
			if (args.Length > 1)
			{
				Program.Logger.Information("Found command-line arguments");
			}
		}

		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				OnStartup(EventArgs.Empty);
				desktop.MainWindow = new MainWindow {DataContext = new MainWindowViewModel(),};
			}

			base.OnFrameworkInitializationCompleted();
		}

		private void CreateOrMigrateDatabase()
		{
			var context = Program.ServiceProvider.GetRequiredService<DbContext>();
			context.Database.EnsureCreated();
			context.Database.Migrate();
		}

	}
}
