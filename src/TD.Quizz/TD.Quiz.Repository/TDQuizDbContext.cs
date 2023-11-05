using Microsoft.EntityFrameworkCore;

namespace TD.Office.Common.Repository
{
    public class TDQuizDbContext : DbContext
    {
        //public DbSet<KorisnikEntity> Korisnici { get; set; }

        public TDQuizDbContext(DbContextOptions otpions) : base(otpions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<KorisnikEntity>().AddMap(new KorisnikEntityMap());
        }
    }
}
