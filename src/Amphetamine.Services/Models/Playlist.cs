using System;
using Amphetamine.Data.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Amphetamine.Services.Models
{
	public class Playlist : ObservableObject
	{
		public string Name { get; }

		public string Path { get; }

		public PlaylistType Type { get; }

		public bool IsSmartPlaylist => this.Type.Equals(PlaylistType.Smart);

		public string SortName => Name.ToLowerInvariant();

		public Playlist(string name, string path, PlaylistType type)
		{
			this.Name = name;
			this.Path = path;
			this.Type = type;
		}

		public override string ToString()
		{
			return this.Name;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !GetType().Equals(obj.GetType()))
			{
				return false;
			}

			return this.Name.Equals(((Playlist)obj).Name, StringComparison.OrdinalIgnoreCase) & this.Type.Equals(((Playlist)obj).Type);
		}

		public override int GetHashCode()
		{
			return this.Path.GetHashCode();
		}
	}
}
