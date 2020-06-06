using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Models
{
    public class KorisnikResetLozinke
    {
        [Display(Name = "Korisničko ime")]
        [Required]
        public string KorisnickoIme { get; set; }

        [StringLength(15, MinimumLength = 5, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Display(Name = "Nova lozinka")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        public string Lozinka { get; set; }

        [Display(Name = "Ponovite novu lozinku")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Compare("Lozinka", ErrorMessage = "Lozinke moraju biti jednake")]
        public string Lozinka2 { get; set; }
    }
}