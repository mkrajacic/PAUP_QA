using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QA.Models
{

        [Table("ovlasti")]
        public class Ovlast
        {
            [Key]
            public string Sifra { get; set; }
            public string Naziv { get; set; }
        }
 }
