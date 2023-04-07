using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amphetamine.Data.Migrations.Default.Sqlite
{
    public partial class _638164334045427425 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "db_default_Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 7),
                    Value = table.Column<string>(type: "varchar(500)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_default_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "db_default_Tracks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Artist = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Genre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Album = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    AlbumArtist = table.Column<string>(type: "varchar(500)", nullable: true),
                    AlbumKey = table.Column<string>(type: "varchar(500)", nullable: true),
                    Path = table.Column<string>(type: "varchar(500)", nullable: true),
                    SafePath = table.Column<string>(type: "varchar(500)", nullable: true),
                    FileName = table.Column<string>(type: "varchar(500)", nullable: true),
                    MimeType = table.Column<string>(type: "varchar(500)", nullable: true),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: true),
                    BitRate = table.Column<long>(type: "INTEGER", nullable: true),
                    SampleRate = table.Column<long>(type: "INTEGER", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Number = table.Column<long>(type: "INTEGER", nullable: true),
                    Count = table.Column<long>(type: "INTEGER", nullable: true),
                    DiscNumber = table.Column<long>(type: "INTEGER", nullable: true),
                    DiscCount = table.Column<long>(type: "INTEGER", nullable: true),
                    Duration = table.Column<long>(type: "INTEGER", nullable: true),
                    Year = table.Column<long>(type: "INTEGER", nullable: true),
                    HasLyrics = table.Column<long>(type: "INTEGER", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateFileCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateLastSynced = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateFileModified = table.Column<DateTime>(type: "datetime", nullable: false),
                    NeedsIndexing = table.Column<long>(type: "INTEGER", nullable: true),
                    NeedsAlbumArtworkIndexing = table.Column<long>(type: "INTEGER", nullable: true),
                    IndexingSuccess = table.Column<long>(type: "INTEGER", nullable: true),
                    IndexingFailureReason = table.Column<string>(type: "varchar(500)", nullable: true),
                    Rating = table.Column<long>(type: "INTEGER", nullable: true),
                    Love = table.Column<long>(type: "INTEGER", nullable: true),
                    PlayCount = table.Column<long>(type: "INTEGER", nullable: true),
                    SkipCount = table.Column<long>(type: "INTEGER", nullable: true),
                    DateLastPlayed = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_default_Tracks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_db_default_Settings_Key",
                table: "db_default_Settings",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_db_default_Tracks_Album",
                table: "db_default_Tracks",
                column: "Album");

            migrationBuilder.CreateIndex(
                name: "IX_db_default_Tracks_Artist",
                table: "db_default_Tracks",
                column: "Artist");

            migrationBuilder.CreateIndex(
                name: "IX_db_default_Tracks_DateAdded",
                table: "db_default_Tracks",
                column: "DateAdded");

            migrationBuilder.CreateIndex(
                name: "IX_db_default_Tracks_DateFileModified",
                table: "db_default_Tracks",
                column: "DateFileModified");

            migrationBuilder.CreateIndex(
                name: "IX_db_default_Tracks_DateLastPlayed",
                table: "db_default_Tracks",
                column: "DateLastPlayed");

            migrationBuilder.CreateIndex(
                name: "IX_db_default_Tracks_DateLastSynced",
                table: "db_default_Tracks",
                column: "DateLastSynced");

            migrationBuilder.CreateIndex(
                name: "IX_db_default_Tracks_Genre",
                table: "db_default_Tracks",
                column: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_db_default_Tracks_Title",
                table: "db_default_Tracks",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_db_default_Tracks_Year",
                table: "db_default_Tracks",
                column: "Year");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "db_default_Settings");

            migrationBuilder.DropTable(
                name: "db_default_Tracks");
        }
    }
}
