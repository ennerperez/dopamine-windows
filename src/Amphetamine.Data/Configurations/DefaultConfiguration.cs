using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Amphetamine.Data.Contexts;
using Amphetamine.Data.Entities;

namespace Amphetamine.Data.Configurations
{
	internal static class DefaultConfiguration
	{
		internal static string Schema => DefaultContext.HasSchema ? Schemas.Default : string.Empty;
		internal static string Prefix => !DefaultContext.HasSchema ? Schemas.Default : string.Empty;
	}

	[DbContext(typeof(DefaultContext))]
	public sealed class SettingConfiguration : IEntityTypeConfiguration<Setting>
	{
		public void Configure(EntityTypeBuilder<Setting> e)
		{
			e.ToTable("Settings", DefaultConfiguration.Schema, DefaultConfiguration.Prefix);

			e.Property(m => m.Id).ValueGeneratedOnAdd();
			e.Property(m => m.Key).HasMaxLength(Lengths.Code).IsRequired();
			e.Property(m => m.Type).HasDefaultValue(Enums.Data.Text).IsRequired();

			e.HasIndex(m => m.Key).IsUnique();
		}
	}

	[DbContext(typeof(DefaultContext))]
	public sealed class TrackConfiguration : IEntityTypeConfiguration<Track>
	{
		public void Configure(EntityTypeBuilder<Track> e)
		{
			e.ToTable("Tracks", DefaultConfiguration.Schema, DefaultConfiguration.Prefix);

			e.Property(m => m.Id).ValueGeneratedOnAdd();
			e.Property(m => m.Artist).HasMaxLength(Lengths.Name);
			e.Property(m => m.Genre).HasMaxLength(Lengths.Name);
			e.Property(m => m.Album).HasMaxLength(Lengths.Name);
			e.Property(m => m.Title).HasMaxLength(Lengths.Name);

			e.HasIndex(m => m.Artist);
			e.HasIndex(m => m.Genre);
			e.HasIndex(m => m.Album);
			e.HasIndex(m => m.Title);
			e.HasIndex(m => m.Year);

			e.HasIndex(m => m.DateAdded);
			e.HasIndex(m => m.DateLastPlayed);
			e.HasIndex(m => m.DateLastSynced);
			e.HasIndex(m => m.DateFileModified);
		}
	}
}
