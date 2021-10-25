using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dopamine.Data.Entities
{
    public class QueuedTrack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long QueuedTrackID { get; set; }

        public string Path { get; set; }

        public string SafePath { get; set; }

        public long IsPlaying { get; set; }

        public long ProgressSeconds { get; set; }

        public long OrderID { get; set; }
      
        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this.QueuedTrackID.Equals(((QueuedTrack)obj).QueuedTrackID);
        }

        public override int GetHashCode()
        {
            return new { this.QueuedTrackID }.GetHashCode();
        }
    }
}
