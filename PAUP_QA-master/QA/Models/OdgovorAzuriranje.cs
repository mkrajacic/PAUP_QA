using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Models
{
    public class OdgovorAzuriranje
    {
        [Required]
        public int Id { get; set; }
        [StringLength(255, MinimumLength = 10, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Odgovor")]
        public string Odgovor { get; set; }

    }
}