using Amphetamine.Services;
using Amphetamine.Services.Shell;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;

namespace Amphetamine
{
	public static class Extensions
	{
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddSingleton<IShellService, ShellService>();
			return services;
		}

		public static IContainerRegistry AddServices(this IContainerRegistry services)
		{
			services.RegisterSingleton<IShellService, ShellService>();
			return services;
		}
	}
}
