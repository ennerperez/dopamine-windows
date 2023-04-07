using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amphetamine.Core.Interfaces;
// ReSharper disable IdentifierTypo

namespace Amphetamine.Core.Services
{
    public class LololyricsService : ILyricsService
    {
        private const string ApiRootFormat = "http://api.lololyrics.com/0.5/getLyric?artist={0}&track={1}";
        private int timeoutSeconds;

        public LololyricsService(int timeoutSeconds)
        {
            this.timeoutSeconds = timeoutSeconds;
        }

        public async Task<string> ParseResultAsync(string result)
        {
            var lyrics = string.Empty;

            await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(result))
                {
                    // http://api.lololyrics.com/
                    var resultXml = XDocument.Parse(result);

                    // Status
                    var status = (from t in resultXml.Element("result")?.Elements("status")
                                     select t.Value).FirstOrDefault();

                    if (status != null && status.ToLower() == "ok")
                    {
                        lyrics = (from t in resultXml.Element("result")?.Elements("response")
                                  select t.Value).FirstOrDefault();
                    }
                }
            });

            return lyrics;
        }

        public string SourceName
        {
            get
            {
                return "LoloLyrics";
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
                if (timeoutSeconds > 0) client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
                client.DefaultRequestHeaders.ExpectContinue = false;
                var response = await client.GetAsync(uri);
                result = await response.Content.ReadAsStringAsync();
            }

            var lyrics = await ParseResultAsync(result);

            return lyrics;
        }
    }
}