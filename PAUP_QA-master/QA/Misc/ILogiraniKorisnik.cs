﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace QA.Misc
{
    public interface ILogiraniKorisnik : IPrincipal
    {
        int Id { get; set; }
        string KorisnickoIme { get; set; }
        string Ovlast { get; set; }
    }
}