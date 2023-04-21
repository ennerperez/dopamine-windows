using Amphetamine.Data.Abstractions;
using Amphetamine.Data.Interfaces;

namespace Amphetamine.Data.Entities
{
	public class Folder : IEntity
	{
		public int Id { get; set; }

		public string Path { get; set; }

		public string SafePath { get; set; }

		public long ShowInCollection { get; set; }

	}
}
