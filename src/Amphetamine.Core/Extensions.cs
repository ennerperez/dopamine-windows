using Amphetamine.Core.Interfaces;
using Amphetamine.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;

namespace Amphetamine.Core
{
	public static class Extensions
	{

		public static IServiceCollection AddCore(this IServiceCollection services)
		{
			services.AddHttpClient();

			services.AddSingleton<IFileService>(new FileSystemService() {ContainerName = "Data", CreateIfNotExists = true});

			// External Services
			services.AddSingleton(new FanartService());
			services.AddSingleton(new LastFMService());

			// LyricsServices
			services.AddSingleton<ILyricsService, ChartLyricsService>();
			services.AddSingleton<ILyricsService, LololyricsService>();
			services.AddSingleton<ILyricsService, MetroLyricsService>();
			services.AddSingleton<ILyricsService, NeteaseLyricsService>();
			services.AddSingleton<ILyricsService, XiamiLyricsService>();

			return services;
		}

		public static IContainerRegistry AddCore(this IContainerRegistry services)
		{
			services.RegisterInstance<IFileService>(new FileSystemService() {ContainerName = "Data", CreateIfNotExists = true});

			// External Services
			services.RegisterInstance(new FanartService());
			services.RegisterInstance(new LastFMService());

			// LyricsServices
			services.RegisterSingleton<ILyricsService, ChartLyricsService>();
			services.RegisterSingleton<ILyricsService, LololyricsService>();
			services.RegisterSingleton<ILyricsService, MetroLyricsService>();
			services.RegisterSingleton<ILyricsService, NeteaseLyricsService>();
			services.RegisterSingleton<ILyricsService, XiamiLyricsService>();

			return services;
		}

	}
}
