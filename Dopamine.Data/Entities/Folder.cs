
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dopamine.Data.Entities
{
    public class Folder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FolderID { get; set; }

        public string Path { get; set; }

        public string SafePath { get; set; }

        public long ShowInCollection { get; set; }
    }
}
