using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository.EntityMappings;
using TD.Core.Repository;

namespace TD.Office.Common.Repository
{
    public class OfficeDbContext : DbContext
    {
        public DbSet<KorisnikEntity> Korisnici { get; set; }

        public OfficeDbContext(DbContextOptions otpions) : base(otpions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KorisnikEntity>().AddMap(new KorisnikEntityMap());
        }
    }
}
