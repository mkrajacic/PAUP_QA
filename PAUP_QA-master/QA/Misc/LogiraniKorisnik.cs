using QA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace QA.Misc
{
        public class LogiraniKorisnik : ILogiraniKorisnik
        {
            public string KorisnickoIme { get; set; }
            public string Ovlast { get; set; }
            public IIdentity Identity { get; private set; }

            public bool IsInRole(string role)
            {
                if (role == Ovlast)
                {
                    return true;
                }
                return false;
            }
            public LogiraniKorisnik(Korisnik kor)
            {
                this.Identity = new GenericIdentity(kor.korisnicko_ime);
                this.KorisnickoIme = kor.korisnicko_ime;
                this.Ovlast = kor.ovlast_sifra;

            }
            public LogiraniKorisnik(string korisnickoIme)
            {
                this.Identity = new GenericIdentity(korisnickoIme);
                this.KorisnickoIme = korisnickoIme;
            }
        }
    }