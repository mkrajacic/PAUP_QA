using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QA.Models
{
    public class KorisnikAzuriranje
    {
        [Required]
        [Range(1, 9999, ErrorMessage = "Korisnik ne postoji!")]
        public int Id { get; set; }
        [Display(Name = "Korisničko ime")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        public string KorisnickoIme { get; set; }

        [Display(Name = "Ovlast")]
        public string Ovlast { get; set; }
        [Required]
        public string KorisnickoImeStaro { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "{0} nije odabrana!")]
        [Display(Name = "Ovlast")]
        public string UpisOvlast { get; set; }
    }
}