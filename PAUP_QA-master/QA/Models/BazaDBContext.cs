using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QA.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BazaDbContext : DbContext
    {
        public DbSet<Pitanje> PopisPitanja { get; set; }
        public DbSet<Kategorija> PopisKategorija { get; set; }
        public DbSet<Korisnik> PopisKorisnika { get; set; }
        public DbSet<Ovlast> PopisOvlasti { get; set; }
       // public DbSet<Odgovor> PopisOdgovora { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            Database.SetInitializer<BazaDbContext>(null);

        }
    }
}