using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Repository
{
    public class KomercijalnoDbContext : DbContext
    {
        public DbSet<Magacin> Magacini { get; set; }
        public DbSet<Dokument> Dokumenti { get; set; }
        public DbSet<VrstaDokMag> VrstaDokMag { get; set; }
        public DbSet<VrstaDok> VrstaDok { get; set; }


        public KomercijalnoDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dokument>()
                .HasKey(nameof(Contracts.Entities.Dokument.VrDok), nameof(Contracts.Entities.Dokument.BrDok));

            modelBuilder.Entity<VrstaDokMag>()
                .HasKey(nameof(Contracts.Entities.VrstaDokMag.VrDok), nameof(Contracts.Entities.VrstaDokMag.MagacinId));
        }
    }
}
