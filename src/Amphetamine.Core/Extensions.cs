using Amphetamine.Core.Interfaces;
using Amphetamine.Core.Services;
using Microsoft.Extensions.DependencyInjection;

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

	}
}
