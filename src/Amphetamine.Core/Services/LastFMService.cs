using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Amphetamine.Core.Services
{
	public class LastFMService
	{
		private const string apiRootFormat = "{0}://ws.audioscrobbler.com/2.0/?method={1}";

        /// <summary>
        /// Performs a POST request over HTTP or HTTPS
        /// </summary>
        /// <returns></returns>
        private async Task<string> PerformPostRequestAsync(string method, IEnumerable<KeyValuePair<string, string>> parameters, bool isSecure)
        {
            var protocol = isSecure ? "https" : "http";
            string result;
            var uri = new Uri(string.Format(apiRootFormat, protocol, method));

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                var response = await client.PostAsync(uri, new FormUrlEncodedContent(parameters));
                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        /// <summary>
        /// Performs a GET request over HTTP or HTTPS
        /// </summary>
        /// <returns></returns>
        private async Task<string> PerformGetRequestAsync(string method, IEnumerable<KeyValuePair<string, string>> parameters, bool isSecure)
        {
            var protocol = isSecure ? "https" : "http";
            string result;
            var dataList = new List<string>();

            // Add everything to the list
            foreach (var parameter in parameters)
            {
                dataList.Add(string.Format("{0}={1}", parameter.Key, Uri.EscapeDataString(parameter.Value)));
            }

            var uri = new Uri(string.Format(apiRootFormat + "&{2}", protocol, method, string.Join("&", dataList.ToArray())));

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                var response = await client.GetAsync(uri);
                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        /// <summary>
        /// Constructs an API method signature as described in point 4 on http://www.last.fm/api/mobileauth
        /// </summary>
        /// <returns>API method signature</returns>
        private string GenerateMethodSignature(IEnumerable<KeyValuePair<string, string>> parameters, string method)
        {
            var alphabeticalList = new List<string>();

            // Add everything to the list
            foreach (var parameter in parameters)
            {
                alphabeticalList.Add(string.Format("{0}{1}", parameter.Key, parameter.Value));
            }

            alphabeticalList.Add("method" + method);

            // Order the list alphabetically
            alphabeticalList = alphabeticalList.OrderBy((t) => t).ToList();


            // Join all parts of the list alphabetically and append API secret
            var signature = string.Format("{0}{1}", string.Join("", alphabeticalList.ToArray()), LastFmSharedSecret);

            // Create MD5 hash and return that
            return signature.Md5Hash();
        }

        public string LastFmSharedSecret { get; set; }
        public string LastFmApiKey { get; set; }

        /// <summary>
        /// Requests authorization from the user by sending a POST request over HTTPS
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Session key</returns>
        public async Task<string> GetMobileSession(string username, string password)
        {
            var method = "auth.getMobileSession";

            var parameters = new Dictionary<string, string>();

            parameters.Add("method", method);
            parameters.Add("username", username);
            parameters.Add("password", password);
            parameters.Add("api_key", LastFmApiKey);

            var apiSig = GenerateMethodSignature(parameters, method);
            parameters.Add("api_sig", apiSig);

            var result = await PerformPostRequestAsync(method, parameters, true);

            // If the status of the result is ok, get the session key.
            var sessionKey = string.Empty;

            if (!string.IsNullOrEmpty(result))
            {
                // http://www.last.fm/api/show/auth.getMobileSession
                var resultXml = XDocument.Parse(result);

                // Status
                var lfmStatus = (from t in resultXml.Elements("lfm")
                                    select t.Attribute("status")?.Value).FirstOrDefault();

                // If Status is ok, get the session key
                if (lfmStatus != null && lfmStatus.ToLower() == "ok")
                {
                    sessionKey = (from t in resultXml.Element("lfm")?.Element("session")?.Elements("key")
                                  select t.Value).FirstOrDefault();
                }
            }

            return sessionKey;
        }

        /// <summary>
        /// Scrobbles a single track
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TrackScrobble(string sessionKey, string artist, string trackTitle, string albumTitle, DateTime playbackStartTime)
        {
            var isSuccess = false;

            var method = "track.scrobble";

            var parameters = new Dictionary<string, string>();

            parameters.Add("method", method);
            parameters.Add("artist", artist);
            parameters.Add("track", trackTitle);
            if (!string.IsNullOrEmpty(albumTitle)) parameters.Add("album", albumTitle);
            parameters.Add("timestamp", playbackStartTime.ConvertToUnixTime().ToString());
            parameters.Add("api_key", LastFmApiKey);
            parameters.Add("sk", sessionKey);

            var apiSig = GenerateMethodSignature(parameters, method);
            parameters.Add("api_sig", apiSig);

            var result = await PerformPostRequestAsync(method, parameters, false);

            if (!string.IsNullOrEmpty(result))
            {
                // http://www.last.fm/api/show/track.scrobble
                var resultXml = XDocument.Parse(result);

                // Status
                var lfmStatus = (from t in resultXml.Elements("lfm")
                                    select t.Attribute("status")?.Value).FirstOrDefault();

                // If Status is ok, return true.
                if (lfmStatus != null && lfmStatus.ToLower() == "ok") isSuccess = true;
            }

            return isSuccess;
        }

        /// <summary>
        /// Scrobbles a single track
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TrackUpdateNowPlaying(string sessionKey, string artist, string trackTitle, string albumTitle)
        {
            var isSuccess = false;

            var method = "track.updateNowPlaying";

            var parameters = new Dictionary<string, string>();

            parameters.Add("method", method);
            parameters.Add("artist", artist);
            parameters.Add("track", trackTitle);
            if (!string.IsNullOrEmpty(albumTitle)) parameters.Add("album", albumTitle);
            parameters.Add("api_key", LastFmApiKey);
            parameters.Add("sk", sessionKey);

            var apiSig = GenerateMethodSignature(parameters, method);
            parameters.Add("api_sig", apiSig);

            var result = await PerformPostRequestAsync(method, parameters, false);

            if (!string.IsNullOrEmpty(result))
            {
                // http://www.last.fm/api/show/track.updateNowPlaying
                var resultXml = XDocument.Parse(result);

                // Get the status from the xml
                var lfmStatus = (from t in resultXml.Elements("lfm")
                                    select t.Attribute("status")?.Value).FirstOrDefault();

                // If the status is ok, return true.
                if (lfmStatus != null && lfmStatus.ToLower() == "ok") isSuccess = true;
            }

            return isSuccess;
        }

        /// <summary>
        /// Gets artist information
        /// </summary>
        /// <returns></returns>
        public async Task<LastFmArtist> ArtistGetInfo(string artist, bool autoCorrect, string languageCode)
        {
            var method = "artist.getInfo";

            var parameters = new Dictionary<string, string>();

            parameters.Add("method", method);
            if (!string.IsNullOrEmpty(languageCode)) parameters.Add("lang", languageCode);
            parameters.Add("artist", artist);
            parameters.Add("autocorrect", autoCorrect ? "1" : "0"); // 1 = transform misspelled artist names into correct artist names, returning the correct version instead. The corrected artist name will be returned in the response.
            parameters.Add("api_key", LastFmApiKey);

            var result = await PerformGetRequestAsync(method, parameters, false);

            var lfmArtist = new LastFmArtist();

            if (!string.IsNullOrEmpty(result))
            {
                // http://www.last.fm/api/show/artist.getInfo
                var resultXml = XDocument.Parse(result);

                // Name
                lfmArtist.Name = (from t in resultXml.Element("lfm")?.Element("artist")?.Elements("name")
                                  select t.Value).FirstOrDefault();

                // MusicBrainzId
                lfmArtist.MusicBrainzId = (from t in resultXml.Element("lfm")?.Element("artist")?.Elements("mbid")
                                           select t.Value).FirstOrDefault();

                // Url
                lfmArtist.Url = (from t in resultXml.Element("lfm")?.Element("artist")?.Elements("url")
                                 select t.Value).FirstOrDefault();

                // ImageSmall
                lfmArtist.ImageSmall = (from t in resultXml.Element("lfm")?.Element("artist")?.Elements("image")
                                        where t.Attribute("size")?.Value == "small"
                                        select t.Value).FirstOrDefault();

                // ImageMedium
                lfmArtist.ImageMedium = (from t in resultXml.Element("lfm")?.Element("artist")?.Elements("image")
                                         where t.Attribute("size")?.Value == "medium"
                                         select t.Value).FirstOrDefault();

                // ImageLarge
                lfmArtist.ImageLarge = (from t in resultXml.Element("lfm")?.Element("artist")?.Elements("image")
                                        where t.Attribute("size")?.Value == "large"
                                        select t.Value).FirstOrDefault();

                // ImageExtraLarge
                lfmArtist.ImageExtraLarge = (from t in resultXml.Element("lfm")?.Element("artist")?.Elements("image")
                                             where t.Attribute("size")?.Value == "extralarge"
                                             select t.Value).FirstOrDefault();

                // ImageMega
                lfmArtist.ImageMega = (from t in resultXml.Element("lfm")?.Element("artist")?.Elements("image")
                                       where t.Attribute("size")?.Value == "mega"
                                       select t.Value).FirstOrDefault();

                // SimilarArtists
                lfmArtist.SimilarArtists = (from t in resultXml.Element("lfm")?.Element("artist")?.Element("similar")?.Elements("artist")
                                            select new LastFmArtist
                                            {
                                                Name = t.Descendants("name").FirstOrDefault()?.Value,
                                                Url = t.Descendants("url").FirstOrDefault()?.Value,
                                                ImageSmall = t.Descendants("image").Where((i) => i.Attribute("size")?.Value == "small").FirstOrDefault()?.Value,
                                                ImageMedium = t.Descendants("image").Where((i) => i.Attribute("size")?.Value == "medium").FirstOrDefault()?.Value,
                                                ImageLarge = t.Descendants("image").Where((i) => i.Attribute("size")?.Value == "large").FirstOrDefault()?.Value,
                                                ImageExtraLarge = t.Descendants("image").Where((i) => i.Attribute("size")?.Value == "extralarge").FirstOrDefault()?.Value,
                                                ImageMega = t.Descendants("image").Where((i) => i.Attribute("size")?.Value == "mega").FirstOrDefault()?.Value
                                            }).ToList();

                // Biography
                lfmArtist.Biography = (from t in resultXml.Element("lfm")?.Element("artist")?.Elements("bio")
                                       select new LastFmBiography
                                       {
                                           Published = t.Descendants("published").FirstOrDefault()?.Value,
                                           Summary = t.Descendants("summary").FirstOrDefault()?.Value,
                                           Content = t.Descendants("content").FirstOrDefault()?.Value
                                       }).FirstOrDefault();
            }

            return lfmArtist;
        }

        /// <summary>
        /// Gets album information
        /// </summary>
        /// <returns></returns>
        public async Task<LastFmAlbum> AlbumGetInfo(string artist, string album, bool autoCorrect, string languageCode)
        {
            var method = "album.getInfo";

            var parameters = new Dictionary<string, string>();

            parameters.Add("method", method);
            if (!string.IsNullOrEmpty(languageCode)) parameters.Add("lang", languageCode);
            parameters.Add("artist", artist);
            parameters.Add("album", album);
            parameters.Add("autocorrect", autoCorrect ? "1" : "0"); // 1 = transform misspelled artist names into correct artist names, returning the correct version instead. The corrected artist name will be returned in the response.
            parameters.Add("api_key", LastFmApiKey);

            var result = await PerformGetRequestAsync(method, parameters, false);

            var lfmAlbum = new LastFmAlbum();

            if (!string.IsNullOrEmpty(result))
            {
                // http://www.last.fm/api/show/album.getInfo
                var resultXml = XDocument.Parse(result);

                var lfmStatus = (from t in resultXml.Elements("lfm")
                                    select t.Attribute("status")?.Value).FirstOrDefault();

                // If Status is ok, return true.
                if (lfmStatus != null && lfmStatus.ToLower() == "ok")
                {

                    // Artist
                    lfmAlbum.Artist = (from t in resultXml.Element("lfm")?.Element("album")?.Elements("artist")
                                       select t.Value).FirstOrDefault();

                    // Name
                    lfmAlbum.Name = (from t in resultXml.Element("lfm")?.Element("album")?.Elements("name")
                                     select t.Value).FirstOrDefault();

                    // Url
                    lfmAlbum.Url = (from t in resultXml.Element("lfm")?.Element("album")?.Elements("url")
                                    select t.Value).FirstOrDefault();

                    // ImageSmall
                    lfmAlbum.ImageSmall = (from t in resultXml.Element("lfm")?.Element("album")?.Elements("image")
                                           where t.Attribute("size")?.Value == "small"
                                           select t.Value).FirstOrDefault();

                    // ImageMedium
                    lfmAlbum.ImageMedium = (from t in resultXml.Element("lfm")?.Element("album")?.Elements("image")
                                            where t.Attribute("size")?.Value == "medium"
                                            select t.Value).FirstOrDefault();

                    // ImageLarge
                    lfmAlbum.ImageLarge = (from t in resultXml.Element("lfm")?.Element("album")?.Elements("image")
                                           where t.Attribute("size")?.Value == "large"
                                           select t.Value).FirstOrDefault();

                    // ImageExtraLarge
                    lfmAlbum.ImageExtraLarge = (from t in resultXml.Element("lfm")?.Element("album")?.Elements("image")
                                                where t.Attribute("size")?.Value == "extralarge"
                                                select t.Value).FirstOrDefault();

                    // ImageMega
                    lfmAlbum.ImageMega = (from t in resultXml.Element("lfm")?.Element("album")?.Elements("image")
                                          where t.Attribute("size")?.Value == "mega"
                                          select t.Value).FirstOrDefault();
                }
            }

            return lfmAlbum;
        }

        /// <summary>
        /// Love a track for a user profile
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="artist"></param>
        /// <param name="trackTitle"></param>
        /// <returns></returns>
        public async Task<bool> TrackLove(string sessionKey, string artist, string trackTitle)
        {
            var isSuccess = false;

            var method = "track.love";

            var parameters = new Dictionary<string, string>();

            parameters.Add("method", method);
            parameters.Add("track", trackTitle);
            parameters.Add("artist", artist);
            parameters.Add("api_key", LastFmApiKey);
            parameters.Add("sk", sessionKey);

            var apiSig = GenerateMethodSignature(parameters, method);
            parameters.Add("api_sig", apiSig);

            var result = await PerformPostRequestAsync(method, parameters, false);

            if (!string.IsNullOrEmpty(result))
            {
                // http://www.last.fm/api/show/track.love
                var resultXml = XDocument.Parse(result);

                // Status
                var lfmStatus = (from t in resultXml.Elements("lfm")
                                    select t.Attribute("status")?.Value).FirstOrDefault();

                // If Status is ok, return true.
                if (lfmStatus != null && lfmStatus.ToLower() == "ok") isSuccess = true;
            }

            return isSuccess;
        }

        /// <summary>
        /// Unlove a track for a user profile
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="artist"></param>
        /// <param name="trackTitle"></param>
        /// <returns></returns>
        public async Task<bool> TrackUnlove(string sessionKey, string artist, string trackTitle)
        {
            var isSuccess = false;

            var method = "track.unlove";

            var parameters = new Dictionary<string, string>();

            parameters.Add("method", method);
            parameters.Add("track", trackTitle);
            parameters.Add("artist", artist);
            parameters.Add("api_key", LastFmApiKey);
            parameters.Add("sk", sessionKey);

            var apiSig = GenerateMethodSignature(parameters, method);
            parameters.Add("api_sig", apiSig);

            var result = await PerformPostRequestAsync(method, parameters, false);

            if (!string.IsNullOrEmpty(result))
            {
                // http://www.last.fm/api/show/track.unlove
                var resultXml = XDocument.Parse(result);

                // Status
                var lfmStatus = (from t in resultXml.Elements("lfm")
                                    select t.Attribute("status")?.Value).FirstOrDefault();

                // If Status is ok, return true.
                if (lfmStatus != null && lfmStatus.ToLower() == "ok") isSuccess = true;
            }

            return isSuccess;
        }

        public class LastFmAlbum
        {
	        public string Name { get; set; }
	        public string Artist { get; set; }
	        public string Url { get; set; }
	        public string ImageSmall { get; set; }
	        public string ImageMedium { get; set; }
	        public string ImageLarge { get; set; }
	        public string ImageExtraLarge { get; set; }
	        public string ImageMega { get; set; }

	        public string LargestImage()
	        {
		        if (!string.IsNullOrEmpty(ImageMega))
		        {
			        return ImageMega;
		        }
		        else if (!string.IsNullOrEmpty(ImageExtraLarge))
		        {
			        return ImageExtraLarge;
		        }
		        else if (!string.IsNullOrEmpty(ImageLarge))
		        {
			        return ImageLarge;
		        }
		        else if (!string.IsNullOrEmpty(ImageMedium))
		        {
			        return ImageMedium;
		        }
		        else if (!string.IsNullOrEmpty(ImageSmall))
		        {
			        return ImageSmall;
		        }
		        else
		        {
			        return string.Empty;
		        }
	        }
        }
        public class LastFmArtist
        {
	        public string Name { get; set; }
	        public string Url { get; set; }
	        public string MusicBrainzId { get; set; }
	        public string ImageSmall { get; set; }
	        public string ImageMedium { get; set; }
	        public string ImageLarge { get; set; }
	        public string ImageExtraLarge { get; set; }
	        public string ImageMega { get; set; }
	        public List<LastFmArtist> SimilarArtists { get; set; }
	        public LastFmBiography Biography { get; set; }

	        public string LargestImage()
	        {
		        if (!string.IsNullOrEmpty(ImageMega))
		        {
			        return ImageMega;
		        }
		        else if (!string.IsNullOrEmpty(ImageExtraLarge))
		        {
			        return ImageExtraLarge;
		        }
		        else if (!string.IsNullOrEmpty(ImageLarge))
		        {
			        return ImageLarge;
		        }
		        else if (!string.IsNullOrEmpty(ImageMedium))
		        {
			        return ImageMedium;
		        }
		        else if (!string.IsNullOrEmpty(ImageSmall))
		        {
			        return ImageSmall;
		        }
		        else
		        {
			        return string.Empty;
		        }
	        }
        }

        public class LastFmBiography
        {
	        public string Published { get; set; }
	        public string Summary { get; set; }
	        public string Content { get; set; }
        }

	}
}
