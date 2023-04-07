using System;
using System.Linq;
using System.Reflection;
using Amphetamine.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Amphetamine.Data
{
	public static class Extensions
	{
		public static Func<DbContext> DbContext { get; set; }

		public static IServiceCollection AddData<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsBuilder = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TContext : DbContext
		{
			services.AddDbContext<TContext>(optionsBuilder, serviceLifetime);
			switch (serviceLifetime)
			{
				case ServiceLifetime.Transient:
					services.AddTransient<DbContext, TContext>();
					break;
				case ServiceLifetime.Singleton:
					services.AddSingleton<DbContext, TContext>();
					break;
				default:
					services.AddScoped<DbContext, TContext>();
					break;
			}
			DbContext = () => services.BuildServiceProvider().GetRequiredService<TContext>();

			services.AddFromAssembly(Assembly.GetExecutingAssembly());

			return services;
		}

		#region FromAssembly

		private static void AddFromAssembly(this IServiceCollection services, params Assembly[] assemblies)
		{
			if (!assemblies.Any())
				throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");

			var assembliesToScan = assemblies.Distinct().ToArray();
			services.ConnectImplementationsToTypesClosing(typeof(IGenericRepository<,>), assembliesToScan, false);
			services.ConnectImplementationsToTypesClosing(typeof(IGenericRepository<>), assembliesToScan, false);
		}

		#endregion

	}
}
