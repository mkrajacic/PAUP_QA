using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Models
{
        public class KorisnikPrijava
        {
            [Display(Name = "Korisničko ime")]
            [Required(ErrorMessage = "{0} je obavezno polje!")]
            public string KorisnickoIme { get; set; }
            [Display(Name = "Lozinka")]
            [DataType(DataType.Password)]
            [Required(ErrorMessage = "{0} je obavezno polje!")]
            public string Lozinka { get; set; }
        }
    }