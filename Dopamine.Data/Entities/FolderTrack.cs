using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dopamine.Data.Entities
{
    public class FolderTrack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FolderTrackID { get; set; }

        public long FolderID { get; set; }

        public long TrackID { get; set; }

        public FolderTrack()
        {
        }

        public FolderTrack(long folderId, long trackId)
        {
            this.FolderID = folderId;
            this.TrackID = trackId;
        }
    }
}
