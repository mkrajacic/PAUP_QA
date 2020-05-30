using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ubiety.Dns.Core;

namespace QA.Models
{
    public class MixModel
    {
        public IEnumerable<Pitanje> Pitanja { get; set; }
        public IEnumerable<Odgovor> Odgovori { get; set; }
        public IEnumerable<Kategorija> Kategorije { get; set; }
        public IEnumerable<Korisnik> Korisnici { get; set; }
    }
}