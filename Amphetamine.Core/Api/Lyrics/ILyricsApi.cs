using System.Threading.Tasks;

namespace Amphetamine.Core.Api.Lyrics
{
    public interface ILyricsApi
    {
        string SourceName { get; }
        Task<string> GetLyricsAsync(string artist, string title);
    }
}