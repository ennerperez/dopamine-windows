using System;
using Amphetamine.Data.Abstractions;

namespace Amphetamine.Data.Entities
{
	public class Track : Entity
	{
		public string Artist { get; set; }

		public string Genre { get; set; }

		public string Album { get; set; }

		public string AlbumArtist { get; set; }

		public string AlbumKey { get; set; }

		public string Path { get; set; }

		public string SafePath { get; set; }

		public string FileName { get; set; }

		public string MimeType { get; set; }

		public long? FileSize { get; set; }

		public long? BitRate { get; set; }

		public long? SampleRate { get; set; }

		public string Title { get; set; }

		public long? Number { get; set; }

		public long? Count { get; set; }

		public long? DiscNumber { get; set; }

		public long? DiscCount { get; set; }

		public long? Duration { get; set; }

		public long? Year { get; set; }

		public long? HasLyrics { get; set; }

		public DateTime DateAdded { get; set; }

		public DateTime DateFileCreated { get; set; }

		public DateTime DateLastSynced { get; set; }

		public DateTime DateFileModified { get; set; }

		public long? NeedsIndexing { get; set; }

		public long? NeedsAlbumArtworkIndexing { get; set; }

		public long? IndexingSuccess { get; set; }

		public string IndexingFailureReason { get; set; }

		public int Rating { get; set; }

		public long? Love { get; set; }

		public long? PlayCount { get; set; }

		public long? SkipCount { get; set; }

		public long? DateLastPlayed { get; set; }

		public static Track CreateDefault(string path)
		{
			var track = new Track()
			{
				Path = path,
				SafePath = path?.ToLower(),
				FileName = System.IO.Path.GetFileNameWithoutExtension(path),
				IndexingSuccess = 0,
				DateAdded = DateTime.Now
			};

			return track;
		}

		public Track ShallowCopy()
		{
			return (Track)MemberwiseClone();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !GetType().Equals(obj.GetType()))
			{
				return false;
			}

			return SafePath.Equals(((Track)obj).SafePath);
		}

		public override int GetHashCode()
		{
			return new {SafePath}.GetHashCode();
		}

		public override string ToString() => Title;
	}
}
