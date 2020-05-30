using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QA.Models
{

        [Table("user_level")]
        public class Ovlast
        {
            [Key]
            [Column("code")]
            public string Sifra { get; set; }

         [Column("name")]
        public string Naziv { get; set; }
        }
 }
