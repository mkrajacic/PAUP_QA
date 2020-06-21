using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using QA.Misc;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;


namespace QA.Controllers
{
    [Authorize]
    public class KorisnikController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();
        [Authorize(Roles = OvlastiKorisnik.Administrator)]
        public ActionResult Index()
        {

            var listaKorisnika = bazaPodataka.PopisKorisnika.OrderBy(x => x.ovlast_sifra).ThenBy(x => x.korisnicko_ime).ToList();
            return View(listaKorisnika);
        }
        [Authorize]
        public ActionResult DetaljiKorisnik(int id)
        {
            Korisnik user = bazaPodataka.PopisKorisnika.Find(id);
            if (user == null || id==0)
            {
                return HttpNotFound();
            }

            if (HttpContext.Request.UrlReferrer != null)
            {
                ViewBag.Return = HttpContext.Request.UrlReferrer.ToString();
            }
            var pitanja = bazaPodataka.PopisPitanja.Where(x => x.korisnicko_ime == id).ToList().OrderByDescending(x=>x.datumObjave);
            ViewBag.Pitanja = pitanja;
            ViewBag.Broj = pitanja.Count();
            if (user.PutanjaSlike == null)
            {
                user.PutanjaSlike = "~/img/user.png";
            }
            return PartialView("_PartialKorisnik",user);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Registracija()
        {
            if (HttpContext.Request.UrlReferrer != null)
            {
                var dosaoSa = HttpContext.Request.UrlReferrer;
                TempData["dosaoSa"] = dosaoSa;
            }

            var prijavljen = User.Identity.IsAuthenticated;

            if (!prijavljen)
            {
                Korisnik model = new Korisnik();

                var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
                ViewBag.Ovlasti = ovlasti;

                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Registracija(Korisnik model, HttpPostedFileBase file)
        {
            if (!String.IsNullOrWhiteSpace(model.korisnicko_ime))
            {
                var korImeZauzeto = bazaPodataka.PopisKorisnika.Any(x => x.korisnicko_ime == model.korisnicko_ime);
                if (korImeZauzeto)
                {
                    ModelState.AddModelError("korisnickoime", "Korisničko ime je vec zauzeto");
                    Response.StatusCode = 400;
                    return View(model);
                }
            }

            if (file != null)
            {
                string putanja = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file.FileName));
                file.SaveAs(putanja);
                model.PutanjaSlike = "~/img/" + file.FileName;
            }

            if (ModelState.IsValid)
            {
                var hashLozinke = Misc.PasswordHelper.HashPassword(model.LozinkaUnos);
                model.salt = hashLozinke.Item1;
                model.lozinka = hashLozinke.Item2;

                bazaPodataka.PopisKorisnika.Add(model);
                bazaPodataka.SaveChanges();

                return View(model);
            }
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }

        }

        [HttpGet]
        [Authorize(Roles = OvlastiKorisnik.Administrator)]
        public ActionResult DodajKorisnika()
        {
            if (HttpContext.Request.UrlReferrer != null)
            {
                var dosaoSa = HttpContext.Request.UrlReferrer;
                TempData["dosaoSa"] = dosaoSa;
            }

            Korisnik model = new Korisnik();

            var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti.Select(x=>x.Naziv);
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = OvlastiKorisnik.Administrator)]
        [ValidateAntiForgeryToken]
        public ActionResult DodajKorisnika(Korisnik model, HttpPostedFileBase file)
        {
            if (!String.IsNullOrWhiteSpace(model.korisnicko_ime))
            {
                var korImeZauzeto = bazaPodataka.PopisKorisnika.Any(x => x.korisnicko_ime == model.korisnicko_ime);
                if (korImeZauzeto)
                {
                    ModelState.AddModelError("korisnicko_ime", "Korisničko ime je vec zauzeto");
                    Response.StatusCode = 400;
                    return View(model);
                }
            }
            var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti.Select(x => x.Naziv);

            foreach (var ovl in ovlasti)
            {
                if (ovl.Naziv == model.upis_ovlast)
                {
                    model.ovlast_sifra = ovl.Sifra;
                }
            }

            if (file != null)
            {
                string putanja = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file.FileName));
                file.SaveAs(putanja);
                model.PutanjaSlike = "~/img/" + file.FileName;
            }

            if (ModelState.IsValid)
            {
                var hashLozinke = Misc.PasswordHelper.HashPassword(model.LozinkaUnos);
                model.salt = hashLozinke.Item1;
                model.lozinka = hashLozinke.Item2;

                bazaPodataka.PopisKorisnika.Add(model);
                bazaPodataka.SaveChanges();

                return View(model);

            }
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Azuriraj(int id,string returnUrl,int user_id)
        {
            if (HttpContext.Request.UrlReferrer != null)
            {
                var dosaoSa = HttpContext.Request.UrlReferrer;
                TempData["dosaoSa"] = dosaoSa;
            }

            var admin = bazaPodataka.PopisKorisnika.Find(user_id);

            if (admin.ovlast_sifra == "AD" || user_id == id)
            {
                var korisnik = bazaPodataka.PopisKorisnika.Find(id);
                if (korisnik == null)
                {
                    return HttpNotFound();
                }

                KorisnikAzuriranje model = new KorisnikAzuriranje();
                ViewBag.ReturnUrl = returnUrl;
                model.KorisnickoImeStaro = korisnik.korisnicko_ime;
                model.KorisnickoIme = korisnik.korisnicko_ime;
                model.Ovlast = korisnik.ovlast_sifra;
                model.Id = korisnik.id;

                var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
                ViewBag.Ovlasti = ovlasti.Select(x => x.Naziv);

                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Azuriraj(KorisnikAzuriranje model, HttpPostedFileBase file)
        {
            var korisnik = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.id == model.Id);

            if (!String.IsNullOrWhiteSpace(model.KorisnickoIme))
            {
                if (!(model.KorisnickoIme==model.KorisnickoImeStaro))
                {
                    var usernameZauzet = bazaPodataka.PopisKorisnika.Any(j => j.korisnicko_ime == model.KorisnickoIme);
                    if (usernameZauzet)
                    {
                        ModelState.AddModelError("KorisnickoIme", "Korisničko ime je već zauzeto!");
                        Response.StatusCode = 400;
                        return View(model);
                    }

                }
            }

            var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti.Select(x => x.Naziv);

            foreach (var ovl in ovlasti)
            {
                if (ovl.Naziv == model.UpisOvlast)
                {
                    model.Ovlast = ovl.Sifra;
                }
            }

            if (file != null)
            {
                string putanja = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(file.FileName));
                file.SaveAs(putanja);
                korisnik.PutanjaSlike = "~/img/" + file.FileName;
            }

            if (ModelState.IsValid)
            {
                    korisnik.korisnicko_ime = model.KorisnickoIme;
                    korisnik.ovlast_sifra = model.Ovlast;

                bazaPodataka.Entry(korisnik).State = EntityState.Modified;
                    bazaPodataka.Configuration.ValidateOnSaveEnabled = false;
                    bazaPodataka.SaveChanges();
                return View(model);
            }
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }

        }

        [HttpGet]
        [Authorize]
        public ActionResult ResetLozinke(int id, int user_id)
        {
            if (HttpContext.Request.UrlReferrer != null)
            {
                var dosaoSa = HttpContext.Request.UrlReferrer;
                TempData["dosaoSa"] = dosaoSa;
            }

            var admin = bazaPodataka.PopisKorisnika.Find(user_id);

            if (admin.ovlast_sifra == "AD" || user_id == id)
            {
                var korisnik = bazaPodataka.PopisKorisnika.Find(id);
                if (korisnik == null || id == 0)
                {
                    return HttpNotFound();
                }

                KorisnikResetLozinke model = new KorisnikResetLozinke();
                model.KorisnickoIme = korisnik.korisnicko_ime;

                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult ResetLozinke(KorisnikResetLozinke model)
        {
            var korisnik = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.korisnicko_ime == model.KorisnickoIme);

            if (ModelState.IsValid)
            {
                var pass = Misc.PasswordHelper.HashPassword(model.Lozinka);
                korisnik.salt = pass.Item1;
                korisnik.lozinka = pass.Item2;

                bazaPodataka.Entry(korisnik).State = EntityState.Modified;
                bazaPodataka.Configuration.ValidateOnSaveEnabled = false;
                bazaPodataka.SaveChanges();

                return View(model);
            }
            else
            {
                var errors = ModelState.GetModelErrors();
                return Json(new { errors });
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Brisi(int id,int user_id)
        {
            var dosaoSa = HttpContext.Request.UrlReferrer;
            if (dosaoSa != null)
            {
                TempData["dosaoSa"] = dosaoSa;
            }

            var admin = bazaPodataka.PopisKorisnika.Find(user_id);

            if (admin.ovlast_sifra == "AD" || user_id == id)
            {
                var korisnik = bazaPodataka.PopisKorisnika.Find(id);

                if (korisnik == null || id == 0)
                {
                    return HttpNotFound();
                }

                return View(korisnik);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [Authorize]
        [HttpPost, ActionName("Brisi")]
        [ValidateAntiForgeryToken]
        public ActionResult BrisiPotvrda(int id)
        {
            try
            {
                var korisnik = bazaPodataka.PopisKorisnika.Find(id);
                if (korisnik == null)
                {
                    return HttpNotFound();
                }

                bazaPodataka.PopisKorisnika.Remove(korisnik);
                bazaPodataka.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var sqlex = ex.InnerException.InnerException as MySqlException;
                string error = "";

                if (sqlex.Number == 1451)
                {
                    error = "MySQL Error";
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

            return RedirectToAction("Index","Korisnik");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Prijava(string returnUrl)
        {
            KorisnikPrijava model = new KorisnikPrijava();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Prijava(KorisnikPrijava model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var korisnikBaza = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.korisnicko_ime == model.KorisnickoIme);
                if (korisnikBaza != null)
                {
                    var passwordOK = Misc.PasswordHelper.ValidatePassword(model.Lozinka, korisnikBaza.lozinka, korisnikBaza.salt);
                    if (passwordOK)
                    {
                        LogiraniKorisnik prijavljeniKorisnik = new LogiraniKorisnik(korisnikBaza);
                        LogiraniKorisnikSerializeModel serializeModel = new LogiraniKorisnikSerializeModel();
                        serializeModel.CopyFromUser(prijavljeniKorisnik);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string korisnickiPodaci = serializer.Serialize(serializeModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                    1,
                                    prijavljeniKorisnik.Identity.Name,
                                    DateTime.Now,
                                    DateTime.Now.AddDays(1),
                                    false,
                                    korisnickiPodaci
                            );
                        string ticketEncrypted = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncrypted);
                        Response.Cookies.Add(cookie);
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "App");
                    }
                }
              
            }
            ModelState.AddModelError("", "Neispravno korisničko ime ili lozinka!");
            return View(model);
        }
        [OverrideAuthorization()]
        [Authorize]
        public ActionResult Odjava()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}