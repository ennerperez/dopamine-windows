using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amphetamine.Core.Interfaces;

namespace Amphetamine.Core.Services
{
    public class MetroLyricsService : ILyricsService
    {
        private const string ApiRootFormat = "http://www.metrolyrics.com/{0}-lyrics-{1}.html";
        private int timeoutSeconds;

        public MetroLyricsService(int timeoutSeconds)
        {
            this.timeoutSeconds = timeoutSeconds;
        }

        /// <summary>
        /// The url must have the format: http://www.metrolyrics.com/teardrop-lyrics-massive-attack.html
        /// All parts must be lowercase and split by "-"
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private async Task<string> BuildUrlAsync(string artist, string title)
        {
            var url = string.Empty;

            await Task.Run(() =>
            {
                var artistPieces = artist.ToLower().Split(' ');
                var titlePieces = title.ToLower().Split(' ');

                var joinedArtist = string.Join("-", artistPieces.Select(p => p.FirstCharToUpper()));
                var joinedTitle = string.Join("-", titlePieces.Select(p => p.FirstCharToUpper()));

                url = string.Format(ApiRootFormat, joinedTitle, joinedArtist);
            });

            return url;
        }

        private async Task<string> ParseLyricsFromHtmlAsync(string html, string originalArtist, string originalTitle)
        {
            var lyrics = string.Empty;

            await Task.Run(() =>
            {
                int[] possibleStarts = {
	                html.IndexOf("<div id=\"lyrics-body-text\" class=\"js-lyric-text\">", StringComparison.Ordinal),
	                html.IndexOf("<!-- First Section -->", StringComparison.Ordinal) };

                var start = possibleStarts.Max();

                int[] possibleEnds = {
	                html.IndexOf("</div>", start, StringComparison.Ordinal),
	                html.IndexOf("<div style=", start, StringComparison.Ordinal),
	                html.IndexOf("<!--WIDGET - RELATED-->", start, StringComparison.Ordinal)
                };

                var end = possibleEnds.Min();

                if (start > 0 && end > 0)
                {
                    lyrics = html.Substring(start, end - start)
                    .Replace("<div id=\"lyrics-body-text\" class=\"js-lyric-text\">", "")
                    .Replace("<!-- First Section -->", "")
                    .Replace("<p class='verse'>", "")
                    .Replace("</p>", Environment.NewLine + Environment.NewLine).Replace("<br>", "")
                    .Trim();
                }
            });

            return lyrics;
        }

        public string SourceName
        {
            get
            {
                return "MetroLyrics";
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
            var uri = new Uri(await BuildUrlAsync(artist, title));

            string result;

            using (var client = new HttpClient())
            {
                if (timeoutSeconds > 0) client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
                client.DefaultRequestHeaders.ExpectContinue = false;
                var response = await client.GetAsync(uri);
                result = await response.Content.ReadAsStringAsync();
            }

            var lyrics = await ParseLyricsFromHtmlAsync(result, artist, title);

            return lyrics;
        }
    }
}
