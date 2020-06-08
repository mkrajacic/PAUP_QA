using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QA.Models
{
    [Table("question_answers")]
    public class Odgovor
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [StringLength(255, MinimumLength = 5, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Odgovor")]
        [Column("answer")]
        public string odgovor { get; set; }
        [Column("user_id")]
        [Range(1, 9999, ErrorMessage = "Korisnik ne postoji!")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Korisničko ime")]
        [ForeignKey("korisnickoIme")]
        public int korisnicko_ime { get; set; }
        [Display(Name = "Korisničko ime")]
        public virtual Korisnik korisnickoIme { get; set; }
        [Column("datetime_created")]
        [Display(Name = "Datum objave")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime datumObjave { get; set; }
        [Column("question_id")]
        [Range(1, 9999, ErrorMessage = "Pitanje ne postoji!")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Pitanje")]
        [ForeignKey("Pit")]
        public int pitanje_id { get; set; }
        [Display(Name = "Pitanje")]
        public virtual Pitanje Pit { get; set; }
        [Column("is_favorite")]
        public bool? najdraze { get; set; }
    }
}