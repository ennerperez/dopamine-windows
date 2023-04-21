using Amphetamine.Core.Interfaces;

namespace Amphetamine.Core.Services
{
	public class LocalizationInfo : ILocalizationInfo
	{
		public string NeteaseLyrics => "NeteaseLyrics"; //ResourceUtils.GetString("Language_NeteaseLyrics");
		public string XiamiLyrics => "XiamiLyrics"; //ResourceUtils.GetString("Language_XiamiLyrics");
	}
}
