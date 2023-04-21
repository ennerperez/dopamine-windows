using Microsoft.Extensions.DependencyInjection;

namespace Amphetamine.Shell
{
	public static class Extensions
	{
		public static IServiceCollection AddShell(this IServiceCollection services)
		{
			//services.AddSingleton<IShellService, ShellService>();
			return services;
		}

	}
}
