using QA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace QA.Controllers
{
    public class AppController : Controller
    {
        // GET: App
        BazaDbContext bazaPodataka = new BazaDbContext();
        public ActionResult Index(string pitanjeTekst)
        {
            var model = new MixModel();
            model.Kategorije = bazaPodataka.PopisKategorija.ToList();
            model.Pitanja = bazaPodataka.PopisPitanja.ToList();
            model.Odgovori = bazaPodataka.PopisOdgovora.ToList();
           
            if (!String.IsNullOrWhiteSpace(pitanjeTekst))
            {
                model.Pitanja = model.Pitanja.Where(x => x.pitanjeTekst.ToUpper().Contains(pitanjeTekst.ToUpper())).ToList();
            }


            return View(model);
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
             var odgovori = bazaPodataka.PopisOdgovora.Where(x=>x.pitanje_id==question.id).ToList();
             int c = 0;
             foreach (var odg in odgovori)
             {
                 c++;
             }
             ViewBag.Broj = c;

            var svi_odgovori = bazaPodataka.PopisOdgovora.ToList();
            ViewBag.SviOdgovori = odgovori;

            var korisnici = bazaPodataka.PopisKorisnika.ToList();
            ViewBag.SviKorisnici = korisnici;
            return PartialView("_PartialPitanje",question);
        }

        public ActionResult OtvoriKategoriju(int? id, string pitanjeTekst)
        {
            var kategorija = bazaPodataka.PopisKategorija.Find(id);
            var pitanja = bazaPodataka.PopisPitanja.Where(x=>x.id_kategorija==kategorija.id).ToList();
            Tuple<List<Pitanje>, Kategorija> tuple;
            tuple = new Tuple<List<Pitanje>, Kategorija>(pitanja,kategorija);

            //if (!String.IsNullOrWhiteSpace(pitanjeTekst))
            //{
            //    pitanja = pitanja.Where(x => x.id_kategorija == kategorija.id && x.pitanjeTekst.ToUpper().Contains(pitanjeTekst.ToUpper())).ToList();
            //}

            //var question = bazaPodataka.PopisPitanja.Where(x => x.id_kategorija == kategorija.id).ToList();
            /*foreach (var qu in question) {
                var odgovori = bazaPodataka.PopisOdgovora.Where(x => x.pitanje_id == qu.id).ToList();
                ViewBag.Broj = odgovori.Count();*/
            // }
            /*int c = 0;
            foreach (var odg in odgovori)
            {
                c++;
            }*/

            //var svi_odgovori = bazaPodataka.PopisOdgovora.ToList();
            // ViewBag.SviOdgovori = odgovori;

            return PartialView("_PartialKategorija", tuple);
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