using Microsoft.Ajax.Utilities;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
        public ActionResult Index()
        {
            var model = new MixModel();
            model.Kategorije = bazaPodataka.PopisKategorija.ToList();
            model.Pitanja = bazaPodataka.PopisPitanja.ToList().OrderByDescending(x=>x.datumObjave);
            model.Odgovori = bazaPodataka.PopisOdgovora.ToList();
           
           /* if (!String.IsNullOrWhiteSpace(pitanjeTekst))
            {
                model.Pitanja = model.Pitanja.Where(x => x.pitanjeTekst.ToUpper().Contains(pitanjeTekst.ToUpper())).ToList();
            }*/


            return View(model);
        }

        public ActionResult Pretrazivanje (string pitanjeTekst,string kategorija)
        {
            var kategorije = bazaPodataka.PopisKategorija.ToList();
            int c = 0;
            foreach (var kat in kategorije)
            {
                if(kategorija==kat.kategorija)
                {
                    break;
                }

                c++;
            }

            /*Kategorija odabrana = new Kategorija();
            odabrana =  bazaPodataka.PopisKategorija.Where(x => x.kategorija == kategorija).FirstOrDefault();*/
            var odabrana = bazaPodataka.PopisKategorija.Find(c+1);
            var pitanja = bazaPodataka.PopisPitanja.OrderByDescending(p => p.datumObjave).ToList();

            Tuple<List<Pitanje>, Kategorija> tuple;
            tuple = new Tuple<List<Pitanje>, Kategorija>(pitanja, odabrana);

            if (String.IsNullOrWhiteSpace(kategorija))
            {
                if (!String.IsNullOrWhiteSpace(pitanjeTekst))
                {
                    pitanja = pitanja.Where(x => x.pitanjeTekst.ToUpper().Contains(pitanjeTekst.ToUpper())).ToList();
                }
            }else
            {
                if (!String.IsNullOrWhiteSpace(pitanjeTekst))
                {
                    pitanja = pitanja.Where(x => x.pitanjeTekst.ToUpper().Contains(pitanjeTekst.ToUpper()) && odabrana.id == x.id_kategorija).ToList();
                }
                else
                {
                    pitanja = bazaPodataka.PopisPitanja.Where(x => x.id_kategorija == odabrana.id).OrderByDescending(p => p.datumObjave).ToList();
                }
            }

            //else if ((!String.IsNullOrWhiteSpace(pitanjeTekst)) && !String.IsNullOrWhiteSpace(kategorija))
            //{
            //    model.Pitanja = bazaPodataka.PopisPitanja.Where(x => x.id_kategorija == kat.Select(kat.))
            //}

            return PartialView("_PartialKategorija",tuple);
        }

        public ActionResult OtvoriPitanje(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new MixModel();
            model.Pitanja = bazaPodataka.PopisPitanja.ToList().Where(x => x.id == id);
            model.Odgovori = bazaPodataka.PopisOdgovora.ToList();
            model.Korisnici = bazaPodataka.PopisKorisnika.ToList();

            if (model.Pitanja == null)
            {
                return HttpNotFound();
            }
             var odgovori = bazaPodataka.PopisOdgovora.Where(x=>x.pitanje_id==id).ToList();
             int c = 0;
             foreach (var odg in odgovori)
             {
                 c++;
             }
             ViewBag.Broj = c;
            ViewBag.Id = id;

            return PartialView("_PartialPitanje",model);
        }

        public ActionResult OtvoriKategoriju(int? id)
        {
            var kategorija = bazaPodataka.PopisKategorija.Find(id);
            var naziv = kategorija.kategorija;
            ViewBag.Kategorija = naziv;
            var model = new MixModel();
            model.Pitanja = bazaPodataka.PopisPitanja.Where(x=>x.id_kategorija==kategorija.id).OrderByDescending(p=>p.datumObjave).ToList();
            model.Odgovori = bazaPodataka.PopisOdgovora.ToList();
            //ViewBag.Odgovora = model.Odgovori.Count();
            //Tuple<List<Pitanje>, Kategorija> tuple;
            //tuple = new Tuple<List<Pitanje>, Kategorija>(pitanja,kategorija);

            return PartialView("_PartialKategorija", model);
        }

        [HttpGet]
        public ActionResult DodajPitanje()
        {
            Pitanje model = new Pitanje();
            var kategorije = bazaPodataka.PopisKategorija.OrderBy(x => x.kategorija).ToList();
            ViewBag.Kategorije = kategorije.Select(x=>x.kategorija);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajPitanje(Pitanje model)
        {
            
            var kategorije = bazaPodataka.PopisKategorija.ToList();
            ViewBag.Kategorije = kategorije;

            foreach(var kat in ViewBag.Kategorije)
            {
                if(kat.kategorija == model.kategorija)
                {
                    model.id_kategorija = kat.id;
                }
            }

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
            return View(model);
        }

        [HttpGet]
        public ActionResult DodajKategoriju()
        {
            Kategorija model = new Kategorija();



            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajKategoriju(Kategorija model)
        {
            if (!String.IsNullOrWhiteSpace(model.kategorija))
            {
                var kategorijaIdPostoji = bazaPodataka.PopisKategorija.Any(x => x.id == model.id);
                if (kategorijaIdPostoji)
                {
                    ModelState.AddModelError("id", "Kategorija već postoji!");
                }
                var kategorijaPostoji = bazaPodataka.PopisKategorija.Any(x => x.kategorija == model.kategorija);
                if (kategorijaIdPostoji)
                {
                    ModelState.AddModelError("kategorija", "Kategorija sa istim nazivom već postoji!");
                }
            }

            if (ModelState.IsValid)
            {
                bazaPodataka.PopisKategorija.Add(model);
                bazaPodataka.SaveChanges();
                return RedirectToAction("Index", "App");
            }

            return View(model);
        }
        [HttpGet]
        public ActionResult DodajOdgovor()
        {
            Odgovor model = new Odgovor();

            if (ViewBag.Korisnik == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(ViewBag.Id==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pitanje = bazaPodataka.PopisPitanja.Find(ViewBag.Id);
            var korisnik = bazaPodataka.PopisKorisnika.Find(ViewBag.Korisnik);
            if (korisnik == null)
            {
                return HttpNotFound();
            }
            else
            {
                model.korisnicko_ime = ViewBag.Korisnik;
            }
            if(pitanje==null)
            {
                return HttpNotFound();
            }
            else
            {
                model.pitanje_id = ViewBag.Id;
            }

            return RedirectToAction("DodajOdgovor", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajOdgovor(Odgovor model)
        {
            if (!String.IsNullOrWhiteSpace(model.odgovor))
            {
                var odgovorPostoji = bazaPodataka.PopisOdgovora.Any(x => x.id == model.id);
                if (odgovorPostoji)
                {
                    ModelState.AddModelError("id", "Odgovor sa istim ID-jem već postoji!");
                }
            }

            if (ModelState.IsValid)
            {
                bazaPodataka.PopisOdgovora.Add(model);
                bazaPodataka.SaveChanges();
            }
            var pit_id = model.pitanje_id;

            return RedirectToAction("OtvoriPitanje",new {id = pit_id});
        }

        public ActionResult OdaberiOdgovor(int id)
        {
            Odgovor model = bazaPodataka.PopisOdgovora.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            model.najdraze = true;
            bazaPodataka.PopisOdgovora.AddOrUpdate(model);

            return RedirectToAction("OtvoriPitanje", new { id = model.Pit.id });
        }

    }
}