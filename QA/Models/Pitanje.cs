using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QA.Models
{
    [Table("pitanje")]
    public class Pitanje
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [StringLength(100, MinimumLength = 10, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Pitanje")]
        [Column("pitanje_tekst")]
        public string pitanjeTekst { get; set; }
        [StringLength(255, MinimumLength = 10, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Display(Name = "Odgovor")]
        [Column("odgovor")]
        public string odgovor { get; set; }
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Korisničko ime")]
        [Column("korisnicko_ime")]
        public string korisnickoIme { get; set; }
        [Column("datum_objave")]
        [Display(Name = "Datum objave")]
        [Required(ErrorMessage = "{0} je obavezan")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime datumObjave { get; set; }
        //[Required(ErrorMessage = "{0} nije odabrana!")]
        //[Column("kategorija")]
        //[Display(Name = "Kategorija")]
        //public Kategorija kategorija { get; set; }
        // zasad komentirano jer u suprotnom ne radi
    }
}