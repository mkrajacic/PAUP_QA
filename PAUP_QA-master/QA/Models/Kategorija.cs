using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QA.Models
{
    [Table("kategorija")]
    public class Kategorija
    {
        [Key]
        public int id { get; set; }
        [StringLength(15, MinimumLength = 3, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Kategorija")]
        public string kategorija { get; set; }
        [Display(Name = "Pitanje")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [StringLength(80, MinimumLength = 10, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        public string pitanje { get; set; }

    }
}