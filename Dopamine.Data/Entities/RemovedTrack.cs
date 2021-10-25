using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dopamine.Data.Entities
{
    public class RemovedTrack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TrackID { get; set; }

        public string Path { get; set; }

        public string SafePath { get; set; }

        public long DateRemoved { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this.SafePath.Equals(((RemovedTrack)obj).SafePath);
        }

        public override int GetHashCode()
        {
            return new { this.SafePath }.GetHashCode();
        }
    }
}
