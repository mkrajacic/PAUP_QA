using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Models
{
    public class PitanjeAzuriranje
    {
        [Required]
        [Range(1, 9999, ErrorMessage = "Pitanje ne postoji!")]
        public int Id { get; set; }
        [Display(Name = "Pitanje")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        public string PitanjeTekst { get; set; }

        [Display(Name = "Kategorija")]
        [Required(ErrorMessage = "{0} nije odabrana!")]
        public string Kategorija { get; set; }
        [Required(ErrorMessage = "{0} nije odabrana!")]
        //[Range(1, 9999, ErrorMessage = "Kategorija ne postoji!")]
        [Display(Name = "Kategorija")]
        public int KategorijaId { get; set; }
    }
}