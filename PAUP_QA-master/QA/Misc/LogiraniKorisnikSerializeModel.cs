using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Misc
{
    public class LogiraniKorisnikSerializeModel
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }

        public string Ovlast { get; set; }
        internal void CopyFromUser(LogiraniKorisnik user)
        {
            this.KorisnickoIme = user.KorisnickoIme;
            this.Ovlast = user.Ovlast;
            this.Id = user.Id;

        }

    }
}