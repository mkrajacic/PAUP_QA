using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QA.Models
{
    [Table("korisnik")]
    public class Korisnik
    {
        [Key]
        public int id { get; set; }
        [StringLength(15, MinimumLength = 3, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Korisničko ime")]
        public string korisnickoIme { get; set; }
        [Display(Name = "Lozinka")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        public string lozinka { get; set; }
        [StringLength(35, MinimumLength = 10, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "E-mail adresa")]
        public string emailAdresa { get; set; }

    }
}