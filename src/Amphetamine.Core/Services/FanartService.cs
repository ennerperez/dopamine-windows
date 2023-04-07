using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
// ReSharper disable IdentifierTypo

namespace Amphetamine.Core.Services
{
    public class FanartService
    {

        private const string ApiRootFormat = "http://webservice.fanart.tv/v3/music/{0}?api_key={1}";

        public string FanartApiKey { get; set; }

        public async Task<string> GetArtistThumbnailAsync(string musicBrainzId)
        {
            var jsonResult = await GetArtistImages(musicBrainzId);
            dynamic dynamicObject = JsonConvert.DeserializeObject(jsonResult);

            return dynamicObject?.artistthumb[0].url;
        }

        private async Task<string> GetArtistImages(string musicBrainzId)
        {
            string result;

            var uri = new Uri(string.Format(ApiRootFormat, musicBrainzId, FanartApiKey));

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
                var response = await client.GetAsync(uri);
                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }
    }
}
