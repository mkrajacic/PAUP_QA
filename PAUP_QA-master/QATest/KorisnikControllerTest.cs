using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Controllers;
using QA.Models;

namespace QATest
{
    [TestClass]
    public class KorisnikControllerTest
    {
        [TestMethod]
        public void KorisnickoImeValidationRequired()
        {
            Korisnik korisnik = new Korisnik();
            korisnik.korisnicko_ime = null;

            var context = new ValidationContext(korisnik) { MemberName = "korisnicko_ime" };
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateProperty(korisnik.korisnicko_ime, context, results);

            Assert.IsFalse(valid);
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual("Korisničko ime je obavezno polje!", results[0].ErrorMessage);
        }
        
    }
}
