using QA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var pitanja = bazaPodataka.PopisPitanja.OrderBy(j=>j.datumObjave).ToList();
            if (!String.IsNullOrWhiteSpace(pitanjeTekst))
            {
                pitanja = pitanja.Where(x => x.pitanjeTekst.ToUpper().Contains(pitanjeTekst.ToUpper())).ToList();
            }

            var kategorije = bazaPodataka.PopisKategorija.ToList();
            ViewBag.Kategorije = kategorije;

            return View(pitanja);
        }

        public ActionResult OtvoriPitanje(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pitanje question = bazaPodataka.PopisPitanja.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialPitanje",question);
        }

        /*[HttpGet]
        public ActionResult DodajPitanje()
        {
            Pitanje model = new Pitanje();
            var odgovori = bazaPodataka.PopisOdgovora.Where(x => x.pitanje_id == model.id).ToList();
            ViewBag.Odgovori = odgovori;

            return View(model);
        }

        [HttpPost]
        public ActionResult DodajPitanje(Pitanje model)
        {
            if (!String.IsNullOrWhiteSpace(model.pitanjeTekst))
            {
                var pitanjePostoji = bazaPodataka.PopisPitanja.Any(x => x.id == model.id);
                if (pitanjePostoji)
                {
                    ModelState.AddModelError("id", "Pitanje sa istim ID-jem već postoji!");
                }
            }

            if (ModelState.IsValid)
            {
                bazaPodataka.PopisPitanja.Add(model);
                bazaPodataka.SaveChanges();

                return RedirectToAction("Index", "App");
            }

            var odgovori = bazaPodataka.PopisOdgovora.Where(x => x.pitanje_id == model.id).ToList();
            ViewBag.Odgovori = odgovori;

            return View(model);
        }*/
    }
}