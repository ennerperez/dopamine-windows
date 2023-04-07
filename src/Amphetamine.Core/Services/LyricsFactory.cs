using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amphetamine.Core.Interfaces;
using Amphetamine.Core.Models;
using Microsoft.Extensions.Logging;

namespace Amphetamine.Core.Services
{
    public class LyricsFactory
    {
        private readonly IList<ILyricsService> _lyricsApis;
        private readonly IList<ILyricsService> _lyricsApisPipe;

        private readonly ILogger _logger;
        public LyricsFactory(ILoggerFactory loggerFactory)
        {
	        _logger = loggerFactory.CreateLogger(GetType());
            _lyricsApis = new List<ILyricsService>();
            _lyricsApisPipe = new List<ILyricsService>();

            // if (providers.ToLower().Contains("chartlyrics")) _lyricsApis.Add(new ChartLyricsApi(timeoutSeconds));
            // if (providers.ToLower().Contains("lololyrics")) _lyricsApis.Add(new LololyricsApi(timeoutSeconds));
            // if (providers.ToLower().Contains("metrolyrics")) _lyricsApis.Add(new MetroLyricsApi(timeoutSeconds));
            // if (providers.ToLower().Contains("xiamilyrics")) _lyricsApis.Add(new XiamiLyricsApi(timeoutSeconds));
            // if (providers.ToLower().Contains("neteaselyrics")) _lyricsApis.Add(new NeteaseLyricsApi(timeoutSeconds));

        }

        public async Task<Lyrics> GetLyricsAsync(string artist, string title)
        {
            Lyrics lyrics = null;
            foreach (var item in _lyricsApis)
            {
                _lyricsApisPipe.Add(item);
            }
            var api = GetRandomApi();

            while (api != null && (lyrics == null || !lyrics.HasText))
            {
                try
                {
                    lyrics = new Lyrics(await api.GetLyricsAsync(artist, title), api.SourceName);
                }
                catch (Exception ex)
                {
	                _logger.LogError("Error while getting lyrics from '{SourceName}'. Exception: {Message}", api.SourceName, ex.Message);
                }

                api = GetRandomApi();
            }

            return lyrics;
        }

        private ILyricsService GetRandomApi()
        {
	        ILyricsService api = null;

            if (_lyricsApisPipe.Count > 0)
            {
                var rnd = new Random();
                var index = rnd.Next(-1,_lyricsApisPipe.Count+1);
                if (index < 0) index = 0;
                if (index > _lyricsApisPipe.Count) index = _lyricsApisPipe.Count;
                api = _lyricsApisPipe[index];
                _lyricsApisPipe.RemoveAt(index);
            }

            return api;
        }
    }
}
