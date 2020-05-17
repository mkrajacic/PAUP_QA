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
            [Required]
            public string KorisnickoIme { get; set; }
            [Display(Name = "Lozinka")]
            [DataType(DataType.Password)]
            [Required]
            public string Lozinka { get; set; }
        }
    }