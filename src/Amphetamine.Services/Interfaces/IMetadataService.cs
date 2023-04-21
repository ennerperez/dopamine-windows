using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amphetamine.Services.Models;

namespace Amphetamine.Services.Interfaces
{
	public interface IMetadataService
	{
		Task UpdateTrackRatingAsync(string path, int rating);

		Task UpdateTrackLoveAsync(string path, bool love);

		Task UpdateTracksAsync(IList<FileMetadata> fileMetadatas, bool updateAlbumArtwork);

		Task UpdateAlbumAsync(Album albumViewModel, MetadataArtworkValue artwork, bool updateFileArtwork);

		FileMetadata GetFileMetadata(string path);

		Task<FileMetadata> GetFileMetadataAsync(string path);

		Task<byte[]> GetArtworkAsync(string path, int size = 0);

		Task ForceSaveFileMetadataAsync();

		event Action<MetadataChangedEventArgs> MetadataChanged;
		event Action<RatingChangedEventArgs> RatingChanged;
		event Action<LoveChangedEventArgs> LoveChanged;
	}
}
