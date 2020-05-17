using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QA.Models
{
    [Table("korisnici")]
    public class Korisnik
    {
        [Key]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Korisničko ime")]
        public string korisnicko_ime { get; set; }
        public string lozinka { get; set; }
        public string salt { get; set; }

        [Column("image")]
        [Display(Name = "image")]
        public byte[] image { get; set; }
        [Column("ovlast")]
        [Display(Name = "Ovlast")]
        [Required]
        [ForeignKey("Ovlast")]
        public string ovlast_sifra { get; set; }
        [Display(Name = "Ovlast")]
        public virtual Ovlast Ovlast { get; set; }
        [Display(Name = "Lozinka")]
        [DataType(DataType.Password)]
        [Required]
        [NotMapped]
        public string LozinkaUnos { get; set; }

        [Display(Name = "Lozinka ponovljena")]
        [DataType(DataType.Password)]
        [Required]
        [NotMapped]
        [Compare("LozinkaUnos", ErrorMessage = "Lozinke moraju biti jednake")]
        public string LozinkaUnos2 { get; set; }

    }
}