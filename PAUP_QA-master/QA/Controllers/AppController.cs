using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using QA.Misc;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;


namespace QA.Controllers
{
    [Authorize(Roles = OvlastiKorisnik.Administrator + "," + OvlastiKorisnik.Registriran)]
    public class AppController : Controller
    {
        // GET: App
        BazaDbContext bazaPodataka = new BazaDbContext();
        [AllowAnonymous]
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
        [AllowAnonymous]
        public ActionResult Pretrazivanje (string pitanjeTekst,string kategorija)
        {
            if (!String.IsNullOrWhiteSpace(pitanjeTekst)) {
                ViewBag.Pojam = pitanjeTekst;
            }
            if (!String.IsNullOrWhiteSpace(kategorija)) {
                ViewBag.Kategorija = kategorija;
            }

            var model = new MixModel();
            model.Kategorije = bazaPodataka.PopisKategorija.ToList();
            model.Pitanja = bazaPodataka.PopisPitanja.ToList().OrderByDescending(x => x.datumObjave);
            model.Odgovori = bazaPodataka.PopisOdgovora.ToList();

            if (!String.IsNullOrWhiteSpace(pitanjeTekst))
            {
                model.Pitanja = model.Pitanja.Where(x => x.pitanjeTekst.ToUpper().Contains(pitanjeTekst.ToUpper())).ToList();
                if (!String.IsNullOrWhiteSpace(kategorija))
                {
                    model.Pitanja = model.Pitanja.Where(x => x.pitanjeTekst.ToUpper().Contains(pitanjeTekst.ToUpper()) && x.kategorijaId.kategorija==kategorija).ToList();
                }
            }
            else if ((String.IsNullOrWhiteSpace(pitanjeTekst)) && (!String.IsNullOrWhiteSpace(kategorija)))
            {
                model.Pitanja = model.Pitanja.Where(x => x.kategorijaId.kategorija == kategorija).ToList();
            }
            foreach(var kat in model.Kategorije)
            {
                if(kat.kategorija==kategorija)
                {
                    ViewBag.Id = kat.id;
                }
            }


                return PartialView("_PartialKategorija",model);
        }
        [AllowAnonymous]
        public ActionResult OtvoriPitanje(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new MixModel();
            model.Pitanja = bazaPodataka.PopisPitanja.ToList().Where(x => x.id == id);
            model.Odgovori = bazaPodataka.PopisOdgovora.ToList().OrderByDescending(x=>x.najdraze).ThenByDescending(x=>x.datumObjave);
            model.Korisnici = bazaPodataka.PopisKorisnika.ToList();

            if (model.Pitanja == null)
            {
                return HttpNotFound();
            }
             var odgovori = bazaPodataka.PopisOdgovora.Where(x=>x.pitanje_id==id).ToList();
            int c = odgovori.Count();
             ViewBag.Broj = c;
            ViewBag.Id = id;

            return PartialView("_PartialPitanje",model);
        }
        [AllowAnonymous]
        public ActionResult OtvoriKategoriju(int? id)
        {
            var kategorija = bazaPodataka.PopisKategorija.Find(id);
            var naziv = kategorija.kategorija;
            ViewBag.Kategorija = naziv;
            var model = new MixModel();
            model.Pitanja = bazaPodataka.PopisPitanja.Where(x=>x.id_kategorija==kategorija.id).OrderByDescending(p=>p.datumObjave).ToList();
            model.Odgovori = bazaPodataka.PopisOdgovora.ToList();
            ViewBag.Id = id;

            return PartialView("_PartialKategorija", model);
        }
        [Authorize]
        [HttpGet]
        public ActionResult DodajPitanje()
        {
            Pitanje model = new Pitanje();
            var kategorije = bazaPodataka.PopisKategorija.OrderBy(x => x.kategorija).ToList();
            ViewBag.Kategorije = kategorije.Select(x=>x.kategorija);

            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajPitanje(Pitanje model)
        {
            var kategorije = bazaPodataka.PopisKategorija.OrderBy(x=>x.kategorija).ToList();
            ViewBag.Kategorije = kategorije.Select(x => x.kategorija);

            foreach (var kat in kategorije)
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
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.GetModelErrors();
            //    return Json(new { errors });
            //}
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public ActionResult DodajKategoriju()
        {
            Kategorija model = new Kategorija();



            return View(model);
        }
        [Authorize]
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
                var kategorijaPostoji = bazaPodataka.PopisKategorija.Any(x => x.kategorija.ToUpper() == model.kategorija.ToUpper());
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [HttpGet]
        public ActionResult OdaberiOdgovor(int id)
        {
            Odgovor model = bazaPodataka.PopisOdgovora.Find(id);
            Pitanje pit = bazaPodataka.PopisPitanja.Find(model.pitanje_id);

            if (model == null)
            {
                return HttpNotFound();
            }
            var ostali_odgovori = bazaPodataka.PopisOdgovora.Where(x => x.pitanje_id == pit.id);

            if (!(ostali_odgovori.Any(x => x.najdraze == true)))
            {
                model.najdraze = true;
            }
            else
            {
                ModelState.AddModelError("najdraze", "Već postoji odabran odgovor!");
            }
            if (ModelState.IsValid)
            {
                bazaPodataka.PopisOdgovora.AddOrUpdate(model);
                bazaPodataka.SaveChanges();
            }

            return RedirectToAction("OtvoriPitanje", new { id = model.Pit.id });
        }
        [Authorize]
        [HttpGet]
        public ActionResult OdznaciOdgovor(int id)
        {
            Odgovor model = bazaPodataka.PopisOdgovora.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            model.najdraze = false;
            if (ModelState.IsValid)
            {
                bazaPodataka.PopisOdgovora.AddOrUpdate(model);
                bazaPodataka.SaveChanges();
            }

            return RedirectToAction("OtvoriPitanje", new { id = model.Pit.id });
        }
        [Authorize]
        [HttpGet]
        public ActionResult AzurirajPitanje(int pit_id)
        {
            Pitanje pitanje = bazaPodataka.PopisPitanja.Find(pit_id);
            var kategorije = bazaPodataka.PopisKategorija.OrderBy(x => x.kategorija).ToList();

            PitanjeAzuriranje model = new PitanjeAzuriranje();
            model.PitanjeTekst = pitanje.pitanjeTekst;
            model.KategorijaId = pitanje.id_kategorija;

            ViewBag.Kategorije = kategorije.Select(x => x.kategorija);
            ViewBag.Id = pit_id;

            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AzurirajPitanje(PitanjeAzuriranje model)
        {
            var pitanje = bazaPodataka.PopisPitanja.Find(model.Id);
            var kategorije = bazaPodataka.PopisKategorija.ToList();

            foreach(var kat in kategorije)
            {
                if (kat.kategorija==model.Kategorija)
                {
                    model.KategorijaId = kat.id;
                }
            }

            if (ModelState.IsValid)
            {
                pitanje.pitanjeTekst = model.PitanjeTekst;
                pitanje.id_kategorija = model.KategorijaId;
                bazaPodataka.Entry(pitanje).State = EntityState.Modified;
                bazaPodataka.Configuration.ValidateOnSaveEnabled = false;
                bazaPodataka.SaveChanges();

                return RedirectToAction("Index", "App");
            }

            return RedirectToAction("Index", "App");
        }
        [Authorize]
        [HttpGet]
        public ActionResult BrisiPitanje(int pit_id)
        {
            if (pit_id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pitanje = bazaPodataka.PopisPitanja.Find(pit_id);
            if (pitanje == null)
            {
                return HttpNotFound();
            }

            return View(pitanje);
        }
        [Authorize]
        [HttpPost, ActionName("BrisiPitanje")]
        [ValidateAntiForgeryToken]
        public ActionResult BrisiPotvrda(int pit_id)
        {
            try
            {
                var pitanje = bazaPodataka.PopisPitanja.Find(pit_id);
                if (pitanje == null)
                {
                    return HttpNotFound();
                }

                bazaPodataka.PopisPitanja.Remove(pitanje);
                bazaPodataka.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var sqlex = ex.InnerException.InnerException as MySqlException;
                string error = "";

                if (sqlex.Number == 1451)
                {
                    error = "Sifra pitanja " + " je vanjski kljuc u nekoj od tablica!";
                }
                else
                {
                    error = sqlex.Message;
                }

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "MySQL greska broj: " + sqlex.Number + " - " + error);
            }
            catch (Exception es)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Greska " + es.Message);
            }

            return RedirectToAction("Index", "App");
        }
        [Authorize]
        [HttpGet]
        public ActionResult AzurirajOdgovor(int odg_id)
        {
            Odgovor odgovor = bazaPodataka.PopisOdgovora.Find(odg_id);

            OdgovorAzuriranje model = new OdgovorAzuriranje();
            model.Odgovor = odgovor.odgovor;
            ViewBag.Id = odg_id;

            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AzurirajOdgovor(OdgovorAzuriranje model)
        {
            var odgovor = bazaPodataka.PopisOdgovora.Find(model.Id);

            if (ModelState.IsValid)
            {
                odgovor.odgovor = model.Odgovor;
                bazaPodataka.Entry(odgovor).State = EntityState.Modified;
                bazaPodataka.Configuration.ValidateOnSaveEnabled = false;
                bazaPodataka.SaveChanges();

                return RedirectToAction("Index", "App");
            }

            return RedirectToAction("Index", "App");
        }
        [Authorize]
        [HttpGet]
        public ActionResult BrisiOdgovor(int odg_id)
        {
            if (odg_id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odgovor = bazaPodataka.PopisOdgovora.Find(odg_id);
            if (odgovor == null)
            {
                return HttpNotFound();
            }

            return View(odgovor);
        }
        [Authorize]
        [HttpPost, ActionName("BrisiOdgovor")]
        [ValidateAntiForgeryToken]
        public ActionResult BrisiOdgPotvrda(int odg_id)
        {
            try
            {
                var odgovor = bazaPodataka.PopisOdgovora.Find(odg_id);
                if (odgovor == null)
                {
                    return HttpNotFound();
                }

                bazaPodataka.PopisOdgovora.Remove(odgovor);
                bazaPodataka.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var sqlex = ex.InnerException.InnerException as MySqlException;
                string error = "";

                if (sqlex.Number == 1451)
                {
                    error = "Sifra odgovora " + " je vanjski kljuc u nekoj od tablica!";
                }
                else
                {
                    error = sqlex.Message;
                }

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "MySQL greska broj: " + sqlex.Number + " - " + error);
            }
            catch (Exception es)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Greska " + es.Message);
            }

            return RedirectToAction("Index", "App");
        }
        [Authorize]
        [HttpGet]
        public ActionResult AzurirajKategoriju(int kat_id)
        {
            Kategorija kategorija = bazaPodataka.PopisKategorija.Find(kat_id);

            KategorijaAzuriranje model = new KategorijaAzuriranje();
            model.Kategorija = kategorija.kategorija;
            ViewBag.Id = kat_id;

            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AzurirajKategoriju(KategorijaAzuriranje model)
        {
            var kategorija = bazaPodataka.PopisKategorija.Find(model.Id);

            if (ModelState.IsValid)
            {
                kategorija.kategorija = model.Kategorija;
                bazaPodataka.Entry(kategorija).State = EntityState.Modified;
                bazaPodataka.Configuration.ValidateOnSaveEnabled = false;
                bazaPodataka.SaveChanges();

                return RedirectToAction("Index", "App");
            }

            return RedirectToAction("Index", "App");
        }
        [Authorize]
        [HttpGet]
        public ActionResult BrisiKategoriju(int kat_id)
        {
            if (kat_id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var kategorija = bazaPodataka.PopisKategorija.Find(kat_id);
            if (kategorija == null)
            {
                return HttpNotFound();
            }

            return View(kategorija);
        }
        [Authorize]
        [HttpPost, ActionName("BrisiKategoriju")]
        [ValidateAntiForgeryToken]
        public ActionResult BrisiKatPotvrda(int kat_id)
        {
            try
            {
                var kategorija = bazaPodataka.PopisKategorija.Find(kat_id);
                if (kategorija == null)
                {
                    return HttpNotFound();
                }

                bazaPodataka.PopisKategorija.Remove(kategorija);
                bazaPodataka.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var sqlex = ex.InnerException.InnerException as MySqlException;
                string error = "";

                if (sqlex.Number == 1451)
                {
                    error = "Sifra kategorije " + " je vanjski kljuc u nekoj od tablica!";
                }
                else
                {
                    error = sqlex.Message;
                }

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "MySQL greska broj: " + sqlex.Number + " - " + error);
            }
            catch (Exception es)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Greska " + es.Message);
            }

            return RedirectToAction("Index", "App");
        }
    }
}