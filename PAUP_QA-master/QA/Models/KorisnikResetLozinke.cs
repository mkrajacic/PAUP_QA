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

        [Display(Name = "Nova lozinka")]
        [DataType(DataType.Password)]
        [Required]
        public string Lozinka { get; set; }

        [Display(Name = "Ponovite novu lozinku")]
        [DataType(DataType.Password)]
        [Required]
        [Compare("Lozinka", ErrorMessage = "Lozinke moraju biti jednake")]
        public string Lozinka2 { get; set; }
    }
}