using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QA.Models
{
    [Table("odgovor")]
    public class Odgovor
    {
       /* [Key]
        [Column("id")]
        public int id { get; set; }
        [StringLength(255, MinimumLength = 10, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Odgovor")]
        [Column("odgovor")]
        public string odgovor { get; set; }
        [Column("korisnicko_ime")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Korisničko ime")]
        [ForeignKey("korisnicko_ime")]
        public string korisnicko_ime { get; set; }
        [Display(Name = "Korisničko ime")]
        public virtual Korisnik kor_ime { get; set; }
        [Column("datum_objave")]
        [Display(Name = "Datum objave")]
        [Required(ErrorMessage = "{0} je obavezan")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime datumObjave { get; set; }
        [Column("pitanje_id")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Pitanje")]
        [ForeignKey("pitanje_id")]
        public int pitanje_id { get; set; }
        [Display(Name = "Pitanje")]
        public virtual Pitanje pit { get; set; }
        public bool is_favorite { get; set; }*/
    }
}