using Antlr.Runtime.Tree;
using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using QA.Misc;
using QA.Models;
using QA.Reports;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading;
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
            model.Kategorije = bazaPodataka.PopisKategorija.ToList().OrderBy(x=>x.kategorija);
            model.Pitanja = bazaPodataka.PopisPitanja.ToList().OrderByDescending(x=>x.datumObjave);
            model.Odgovori = bazaPodataka.PopisOdgovora.ToList();

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
                TempData["kat"] = kategorija;
            }

            var model = new MixModel();
            model.Kategorije = bazaPodataka.PopisKategorija.ToList().OrderBy(x=>x.kategorija);
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
            if (id == null || id==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (HttpContext.Request.UrlReferrer != null)
            {
                ViewBag.Return = HttpContext.Request.UrlReferrer.ToString();
            }
            var model = new MixModel();
            model.Pitanja = bazaPodataka.PopisPitanja.ToList().Where(x => x.id == id);
            model.Odgovori = bazaPodataka.PopisOdgovora.ToList().OrderByDescending(x=>x.najdraze).ThenByDescending(x=>x.datumObjave);
            model.Korisnici = bazaPodataka.PopisKorisnika.ToList();

            model.Korisnici.Where(x => x.PutanjaSlike == null || x.PutanjaSlike.Length == 0).ForEach(x => x.PutanjaSlike = "~/img/user.png");

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
            var dosaoSa = HttpContext.Request.UrlReferrer;
            if (dosaoSa != null)
            {
                TempData["dosaoSa"] = dosaoSa;
            }
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
                return View(model);
            }
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }

        }
        [Authorize]
        [HttpGet]
        public ActionResult DodajKategoriju()
        {
            Kategorija model = new Kategorija();
            var dosaoSa = HttpContext.Request.UrlReferrer;
            if (dosaoSa != null)
            {
                TempData["dosaoSa"] = dosaoSa;
            }


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
                    Response.StatusCode = 400;
                    return View(model);
                }
                var kategorijaPostoji = bazaPodataka.PopisKategorija.Any(x => x.kategorija.ToUpper() == model.kategorija.ToUpper());
                if (kategorijaPostoji)
                {
                    ModelState.AddModelError("kategorija", "Kategorija sa istim nazivom već postoji!");
                    Response.StatusCode = 400;
                    return View(model);
                }
            }

            if (ModelState.IsValid)
            {
                bazaPodataka.PopisKategorija.Add(model);
                bazaPodataka.SaveChanges();
                return View(model);
            }
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }

        }
        [Authorize]
        [HttpGet]
        public ActionResult DodajOdgovor()
        {
            Odgovor model = new Odgovor();

            var dosaoSa = HttpContext.Request.UrlReferrer;
            if (dosaoSa != null)
            {
                TempData["dosaoSa"] = dosaoSa;
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
                var pit_id = model.pitanje_id;

                return RedirectToAction("OtvoriPitanje", new { id = pit_id });
            }
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }
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

            if ((ostali_odgovori.Any(x => x.najdraze == true)))
            {
                ModelState.AddModelError("najdraze", "Već postoji odabran odgovor!");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                model.najdraze = true;
            }

            if (ModelState.IsValid)
            {
                bazaPodataka.PopisOdgovora.AddOrUpdate(model);
                bazaPodataka.SaveChanges();
                return RedirectToAction("OtvoriPitanje", new { id = model.Pit.id });
            }
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }
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
        public ActionResult AzurirajPitanje(int pit_id,int user_id)
        {
            if (HttpContext.Request.UrlReferrer != null)
            {
                var dosaoSa = HttpContext.Request.UrlReferrer;
                TempData["dosaoSa"] = dosaoSa;
            }

            Pitanje pitanje = bazaPodataka.PopisPitanja.Find(pit_id);
            var admin = bazaPodataka.PopisKorisnika.Find(user_id);

            if (admin.ovlast_sifra == "AD" || user_id == pitanje.korisnicko_ime)
            {
                var kategorije = bazaPodataka.PopisKategorija.OrderBy(x => x.kategorija).ToList();

                PitanjeAzuriranje model = new PitanjeAzuriranje();
                model.PitanjeTekst = pitanje.pitanjeTekst;
                model.KategorijaId = pitanje.id_kategorija;

                ViewBag.Kategorije = kategorije.Select(x => x.kategorija);
                ViewBag.Id = pit_id;

                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AzurirajPitanje(PitanjeAzuriranje model)
        {
            var pitanje = bazaPodataka.PopisPitanja.Find(model.Id);
            var kategorije = bazaPodataka.PopisKategorija.ToList().OrderBy(x=>x.kategorija);

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
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult BrisiPitanje(int pit_id,int user_id)
        {
            var dosaoSa = HttpContext.Request.UrlReferrer;
            if (dosaoSa != null)
            {
                TempData["dosaoSa"] = dosaoSa;
            }
            var pitanje = bazaPodataka.PopisPitanja.Find(pit_id);
            var admin = bazaPodataka.PopisKorisnika.Find(user_id);

            if (admin.ovlast_sifra == "AD" || user_id == pitanje.korisnicko_ime)
            {

                if (pitanje == null || pit_id == 0)
                {
                    return HttpNotFound();
                }

                return View(pitanje);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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
        public ActionResult AzurirajOdgovor(int odg_id,int user_id)
        {
            if (HttpContext.Request.UrlReferrer != null)
            {
                var dosaoSa = HttpContext.Request.UrlReferrer;
                TempData["dosaoSa"] = dosaoSa;
            }

            Odgovor odgovor = bazaPodataka.PopisOdgovora.Find(odg_id);
            var admin = bazaPodataka.PopisKorisnika.Find(user_id);

            if (admin.ovlast_sifra == "AD" || user_id == odgovor.korisnicko_ime)
            {
                OdgovorAzuriranje model = new OdgovorAzuriranje();
                model.Odgovor = odgovor.odgovor;
                ViewBag.Id = odg_id;
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }

        }
        [Authorize]
        [HttpGet]
        public ActionResult BrisiOdgovor(int odg_id,int user_id)
        {
            var dosaoSa = HttpContext.Request.UrlReferrer;
            if (dosaoSa != null)
            {
                TempData["dosaoSa"] = dosaoSa;
            }

            var odgovor = bazaPodataka.PopisOdgovora.Find(odg_id);
            var admin = bazaPodataka.PopisKorisnika.Find(user_id);

            if (admin.ovlast_sifra == "AD" || user_id == odgovor.korisnicko_ime)
            {
                if (odgovor == null || odg_id == 0)
                {
                    return HttpNotFound();
                }
               return View(odgovor);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
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
        public ActionResult AzurirajKategoriju(int kat_id,int user_id)
        {
            if (HttpContext.Request.UrlReferrer != null)
            {
                var dosaoSa = HttpContext.Request.UrlReferrer;
                TempData["dosaoSa"] = dosaoSa;
            }

            Kategorija kategorija = bazaPodataka.PopisKategorija.Find(kat_id);
            var admin = bazaPodataka.PopisKorisnika.Find(user_id);

            if (admin.ovlast_sifra == "AD")
            {
                KategorijaAzuriranje model = new KategorijaAzuriranje();
                model.Kategorija = kategorija.kategorija;
                ViewBag.Id = kat_id;
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AzurirajKategoriju(KategorijaAzuriranje model)
        {
            var kategorija = bazaPodataka.PopisKategorija.Find(model.Id);

            var kategorijaPostoji = bazaPodataka.PopisKategorija.Any(x => x.kategorija.ToUpper() == model.Kategorija.ToUpper());

            if(kategorijaPostoji)
            {
                ModelState.AddModelError("kategorija", "Kategorija sa istim nazivom već postoji!");
                Response.StatusCode = 400;
                return View(model);
            }

            if (ModelState.IsValid)
            {
                kategorija.kategorija = model.Kategorija;
                bazaPodataka.Entry(kategorija).State = EntityState.Modified;
                bazaPodataka.Configuration.ValidateOnSaveEnabled = false;
                bazaPodataka.SaveChanges();

                return RedirectToAction("Index", "App");
            }
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }

        }
        [Authorize]
        [HttpGet]
        public ActionResult BrisiKategoriju(int kat_id,int user_id)
        {
            var dosaoSa = HttpContext.Request.UrlReferrer;
            if (dosaoSa != null)
            {
                TempData["dosaoSa"] = dosaoSa;
            }

            var kategorija = bazaPodataka.PopisKategorija.Find(kat_id);
            var admin = bazaPodataka.PopisKorisnika.Find(user_id);

            if (admin.ovlast_sifra == "AD")
            {
                if (kategorija == null || kat_id == 0)
                {
                    return HttpNotFound();
                }
                return View(kategorija);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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

        [AllowAnonymous]
        public ActionResult IspisPitanja()
        {
            MixModel model = new MixModel();
            model.Pitanja = bazaPodataka.PopisPitanja.ToList().OrderByDescending(x=>x.datumObjave).ThenBy(x=>x.kategorijaId.kategorija);
            model.Odgovori = bazaPodataka.PopisOdgovora.ToList().OrderByDescending(x => x.datumObjave).ThenBy(x => x.Pit.pitanjeTekst);

            if (TempData["kat"] != null)
            {
                var kat = TempData["kat"].ToString();
                model.Pitanja = bazaPodataka.PopisPitanja.ToList().Where(x => x.kategorijaId.kategorija == kat).OrderByDescending(x => x.datumObjave).ThenBy(x => x.kategorijaId.kategorija);
            }


            System.Threading.Thread.Sleep(1000);

            AppReport appReport = new AppReport();
            appReport.pdfSvaPitanja(model);

            return File(appReport.Podatci, System.Net.Mime.MediaTypeNames.Application.Pdf, "PopisSvihPitanja.pdf");

        }

        public ActionResult IspisKorisnika()
        {
            var listaKorisnika = bazaPodataka.PopisKorisnika.OrderBy(x => x.ovlast_sifra).ThenBy(x => x.korisnicko_ime).ToList();

            System.Threading.Thread.Sleep(1000);

            AppReport appReport = new AppReport();
            appReport.pdfSviKorisnici(listaKorisnika);

            return File(appReport.Podatci, System.Net.Mime.MediaTypeNames.Application.Pdf, "PopisSvihKorisnika.pdf");
        }

        public ActionResult KorisnikIspisPit(int id)
        {
            Korisnik korisnik = bazaPodataka.PopisKorisnika.Find(id);

            System.Threading.Thread.Sleep(1000);

            AppReport appReport = new AppReport();
            appReport.pdfKorisnikPitanja(korisnik);

            return File(appReport.Podatci, System.Net.Mime.MediaTypeNames.Application.Pdf, "KorisnikPitanja.pdf");
        }

        public ActionResult KorisnikIspisOdg(int id)
        {
            Korisnik korisnik = bazaPodataka.PopisKorisnika.Find(id);

            System.Threading.Thread.Sleep(1000);

            AppReport appReport = new AppReport();
            appReport.pdfKorisnikOdgovori(korisnik);

            return File(appReport.Podatci, System.Net.Mime.MediaTypeNames.Application.Pdf, "KorisnikOdgovori.pdf");
        }
    }
}