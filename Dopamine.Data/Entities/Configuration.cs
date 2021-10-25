
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dopamine.Data.Entities
{
    public class Configuration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ConfigurationID { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
