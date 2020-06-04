using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QA.Models
{
    [Table("questions")]
    public class Pitanje
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [StringLength(100, MinimumLength = 10, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Pitanje")]
        [Column("question")]
        public string pitanjeTekst { get; set; }
       /* [StringLength(100, MinimumLength = 10, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Odgovor")]
        [Column("odgovor")]
        public string odgovor { get; set; }*/
        [Column("user_id")]
        [Required(ErrorMessage = "{0} je obavezno polje!")]
        [Display(Name = "Korisničko ime")]
        [ForeignKey("korisnickoIme")]
        public int korisnicko_ime { get; set; }
        [Display(Name = "Korisničko ime")]
        public virtual Korisnik korisnickoIme { get; set; }
        [Column("datetime_created")]
        [Display(Name = "Datum objave")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime datumObjave { get; set; }

        [Required(ErrorMessage = "{0} nije odabrana!")]
        [Column("category_id")]
        [Display(Name = "Kategorija")]
        [ForeignKey("kategorijaId")]
        public int id_kategorija { get; set; }
        [Display(Name = "Kategorija")]
        public virtual Kategorija kategorijaId { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "{0} nije odabrana!")]
        [Display(Name = "Kategorija")]
        public string kategorija { get; set; }

    }
}