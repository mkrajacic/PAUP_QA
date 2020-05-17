using QA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QA.Controllers
{
    public class AppController : Controller
    {
        // GET: App
        BazaDbContext bazaPodataka = new BazaDbContext();
        public ActionResult Index(string pitanjeTekst)
        {

            var pitanja = bazaPodataka.PopisPitanja.ToList();
            if (!String.IsNullOrWhiteSpace(pitanjeTekst))
            {
                pitanja = pitanja.Where(x => x.pitanjeTekst.ToUpper().Contains(pitanjeTekst.ToUpper())).ToList();
            }

            return View(pitanja);
        }

        public ActionResult Prijava()
        {
            return View();
        }

        public ActionResult Registracija()
        {
            return View();
        }
    }
}