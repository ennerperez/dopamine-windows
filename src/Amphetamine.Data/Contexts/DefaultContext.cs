using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Amphetamine.Data.Entities;
using Amphetamine.Data.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Amphetamine.Data.Contexts
{
	public class DefaultContext : DbContext
	{
		public DefaultContext()
		{
		}

		public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
		{
		}

		private static string ProviderName { get; set; }
		internal static bool HasSchema => DbContextExtensions.HasSchema(ProviderName);
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

			ProviderName = Database.ProviderName?.Split('.').Last();

			// Configurations
			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly, m => m.GetCustomAttributes(typeof(DbContextAttribute), true).OfType<DbContextAttribute>().Any(a => a.ContextType == GetType()));

			// Conventions
			// modelBuilder.RemovePluralizingTableNameConvention();
			modelBuilder.AddProviderTypeConventions(m =>
			{
				m.Provider = ProviderName;
				m.DecimalConfig.Add(6, new[] {"Lat", "Long"});
				m.Exclude = null;
				m.UseDateTime = false;
			});
			//modelBuilder.AddAuditableEntitiesConventions<IAuditable>(ProviderName);
			//modelBuilder.AddSynchronizableEntitiesConventions<ISyncronizable>(ProviderName);
		}

#if DEBUG
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder?.EnableDetailedErrors();
			optionsBuilder?.EnableSensitiveDataLogging();

			var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var configuration = new ConfigurationBuilder()
				.SetBasePath(directory)
				.AddJsonFile("appsettings.json", false, true)
#if DEBUG
				.AddJsonFile("appsettings.Development.json", true, true)
#endif
				.AddEnvironmentVariables()
				.Build();

			optionsBuilder?.UseDbEngine(configuration, nameof(DefaultContext),  DatabaseProviders.Sqlite);
		}
#endif

		#region DbSet

		public DbSet<Setting> Settings { get; set; }
		public DbSet<Track> Tracks { get; set; }

		#endregion DbSet

	}
}
