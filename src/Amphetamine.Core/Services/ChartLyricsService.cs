using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amphetamine.Core.Interfaces;

namespace Amphetamine.Core.Services
{
	public class ChartLyricsService : ILyricsService
    {
        private const string ApiRootFormat = "http://api.chartlyrics.com/apiv1.asmx/SearchLyricDirect?artist={0}&song={1}";
        private int _timeoutSeconds;

        public ChartLyricsService(int timeoutSeconds)
        {
            _timeoutSeconds = timeoutSeconds;
        }

        public async Task<string> ParseResultAsync(string result)
        {
            var lyrics = string.Empty;

            await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(result))
                {
                    // http://www.chartlyrics.com/api.aspx
                    var resultXml = XDocument.Parse(result);

                    // Select elements by LocalName because ChartLyrics XML has namespace issues
                    // We know the element we want is uniquely named, so we skip all the intermediate elements
                    lyrics = (from t in resultXml.Root?.Descendants().Where(e => e.Name.LocalName == "Lyric")
                              select t.Value).FirstOrDefault();
                }
            });

            return lyrics;
        }

        public string SourceName
        {
            get
            {
                return "ChartLyrics";
            }
        }

        /// <summary>
        /// Searches for lyrics for the given artist and title
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<string> GetLyricsAsync(string artist, string title)
        {
            var uri = new Uri(string.Format(ApiRootFormat, artist, title));

            string result;

            using (var client = new HttpClient())
            {
                if (_timeoutSeconds > 0) client.Timeout = TimeSpan.FromSeconds(_timeoutSeconds);
                client.DefaultRequestHeaders.ExpectContinue = false;
                var response = await client.GetAsync(uri);
                result = await response.Content.ReadAsStringAsync();
            }

            var lyrics = await ParseResultAsync(result);

            return lyrics;
        }
    }
}
