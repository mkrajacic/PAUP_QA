using QA.Misc;
using QA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;


namespace QA.Controllers
{
    [Authorize(Roles = OvlastiKorisnik.Administrator)]
    public class KorisnikController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();
        [AllowAnonymous]
        public ActionResult Index()
        {

            var listaKorisnika = bazaPodataka.PopisKorisnika.OrderBy(x => x.ovlast_sifra).ThenBy(x => x.korisnicko_ime).ToList();
            return View(listaKorisnika);
        }


        public ActionResult DetaljiKorisnik(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Korisnik user = bazaPodataka.PopisKorisnika.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialKorisnik",user);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Registracija()
        {
            Korisnik model = new Korisnik();

            var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Registracija(Korisnik model)
        {
            if (!String.IsNullOrWhiteSpace(model.korisnicko_ime))
            {
                var korImeZauzeto = bazaPodataka.PopisKorisnika.Any(x => x.korisnicko_ime == model.korisnicko_ime);
                if (korImeZauzeto)
                {
                    ModelState.AddModelError("korisnicko_ime", "Korisničko ime je vec zauzeto");
                }
            }

            if (ModelState.IsValid)
            {
                var hashLozinke = Misc.PasswordHelper.HashPassword(model.LozinkaUnos);
                model.salt = hashLozinke.Item1;
                model.lozinka = hashLozinke.Item2;

                bazaPodataka.PopisKorisnika.Add(model);
                bazaPodataka.SaveChanges();

               /* byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                }

                model.image = imageData;*/

                return RedirectToAction("Index","App");
            }

            var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti;

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult DodajKorisnika()
        {
            Korisnik model = new Korisnik();

            var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DodajKorisnika(Korisnik model)
        {
            if (!String.IsNullOrWhiteSpace(model.korisnicko_ime))
            {
                var korImeZauzeto = bazaPodataka.PopisKorisnika.Any(x => x.korisnicko_ime == model.korisnicko_ime);
                if (korImeZauzeto)
                {
                    ModelState.AddModelError("korisnicko_ime", "Korisničko ime je vec zauzeto");
                }
            }

            if (ModelState.IsValid)
            {
                var hashLozinke = Misc.PasswordHelper.HashPassword(model.LozinkaUnos);
                model.salt = hashLozinke.Item1;
                model.lozinka = hashLozinke.Item2;

                bazaPodataka.PopisKorisnika.Add(model);
                bazaPodataka.SaveChanges();

                /* byte[] imageData = null;
                 if (Request.Files.Count > 0)
                 {
                     HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];

                     using (var binary = new BinaryReader(poImgFile.InputStream))
                     {
                         imageData = binary.ReadBytes(poImgFile.ContentLength);
                     }
                 }

                 model.image = imageData;*/

                return RedirectToAction("Index", "Korisnik");
            }

            var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti;

            return View(model);
        }

        [HttpGet]
        public ActionResult Azuriraj(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var korisnik = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.korisnicko_ime == id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }

            KorisnikAzuriranje model = new KorisnikAzuriranje();
            model.KorisnickoIme = korisnik.korisnicko_ime;
            model.Ovlast = korisnik.ovlast_sifra;

            var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti;

            return View(model);
        }

        [HttpPost]
        public ActionResult Azuriraj(KorisnikAzuriranje model)
        {
            var korisnik = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.korisnicko_ime == model.KorisnickoIme);
            if (!String.IsNullOrWhiteSpace(model.KorisnickoIme))
            {
                var usernameZauzet = bazaPodataka.PopisKorisnika.Any(j => j.korisnicko_ime == model.KorisnickoIme);
                if (usernameZauzet)
                {
                    ModelState.AddModelError("KorisnickoIme", "Korisničko ime je već zauzeto!");
                }
            }

            if (ModelState.IsValid)
            {
                korisnik.korisnicko_ime = model.KorisnickoIme;
                korisnik.ovlast_sifra = model.Ovlast;

                bazaPodataka.Entry(korisnik).State = EntityState.Modified;
                bazaPodataka.Configuration.ValidateOnSaveEnabled = false;
                bazaPodataka.SaveChanges();

                return RedirectToAction("Index");
            }

            var ovlasti = bazaPodataka.PopisOvlasti.OrderBy(x => x.Naziv).ToList();
            ViewBag.Ovlasti = ovlasti;

            return View(model);
        }

        [HttpGet]
        public ActionResult ResetLozinke(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var korisnik = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.korisnicko_ime == id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }

            KorisnikResetLozinke model = new KorisnikResetLozinke();
            model.KorisnickoIme = korisnik.korisnicko_ime;

            return View(model);
        }

        [HttpPost]
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

                return RedirectToAction("Index","App");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Brisi(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var korisnik = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.korisnicko_ime == id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }

            return View(korisnik);
        }

        [HttpPost, ActionName("Brisi")]
        [ValidateAntiForgeryToken]
        public ActionResult BrisiPotvrda(string id)
        {
            var korisnik = bazaPodataka.PopisKorisnika.FirstOrDefault(x => x.korisnicko_ime == id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }

            bazaPodataka.PopisKorisnika.Remove(korisnik);
            bazaPodataka.SaveChanges();

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