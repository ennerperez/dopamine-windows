using System.Threading.Tasks;

namespace Amphetamine.Core.Interfaces
{
	public interface ILyricsService
	{
		string SourceName { get; }
		Task<string> GetLyricsAsync(string artist, string title);
	}
}
