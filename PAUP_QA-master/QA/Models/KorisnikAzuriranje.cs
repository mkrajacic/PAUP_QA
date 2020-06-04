using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA.Models
{
    public class KorisnikAzuriranje
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Korisničko ime")]
        [Required]
        public string KorisnickoIme { get; set; }

        [Display(Name = "Ovlast")]
        [Required]
        public string Ovlast { get; set; }

    }
}