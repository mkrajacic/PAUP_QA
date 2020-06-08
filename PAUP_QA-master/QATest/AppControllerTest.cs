using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATest
{
    [TestClass]
    public class AppControllerTest
    {
        [TestMethod]
        public void PitanjeKategorijaValidationRequired()
        {
            Pitanje pitanje = new Pitanje();
            pitanje.korisnicko_ime = 0;

            var context = new ValidationContext(pitanje) { MemberName = "korisnicko_ime" };
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateProperty(pitanje.korisnicko_ime, context, results);

            Assert.IsFalse(valid);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual("Korisnik ne postoji!", results[0].ErrorMessage);
        }

        [TestMethod]
        public void OdgovorValidationLengthMin5()
        {
            Odgovor odgovor = new Odgovor();
            odgovor.odgovor = "o";

            var context = new ValidationContext(odgovor) { MemberName = "odgovor" };
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateProperty(odgovor.odgovor, context, results);

            Assert.IsFalse(valid);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual("Odgovor mora biti duljine minimalno 5 a maksimalno 255 znakova", results[0].ErrorMessage);
        }
        [TestMethod]
        public void OdgovorValidationLengthMax255()
        {
            Odgovor odgovor = new Odgovor();
            odgovor.odgovor = new string('o', 256);

            var context = new ValidationContext(odgovor) { MemberName = "odgovor" };
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateProperty(odgovor.odgovor, context, results);

            Assert.IsFalse(valid);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual("Odgovor mora biti duljine minimalno 5 a maksimalno 255 znakova", results[0].ErrorMessage);
        }
    }
}
