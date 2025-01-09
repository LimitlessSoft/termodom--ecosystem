using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Repository
{
    public class KomercijalnoDbContext(DbContextOptions<KomercijalnoDbContext> options)
        : LSCoreDbContext<KomercijalnoDbContext>(options)
    {
        public DbSet<Magacin> Magacini { get; set; }
        public DbSet<Dokument> Dokumenti { get; set; }
        public DbSet<VrstaDokMag> VrstaDokMag { get; set; }
        public DbSet<VrstaDok> VrstaDok { get; set; }
        public DbSet<RobaUMagacinu> RobaUMagacinu { get; set; }
        public DbSet<Partner> Partneri { get; set; }
        public DbSet<Roba> Roba { get; set; }
        public DbSet<Tarifa> Tarife { get; set; }
        public DbSet<Stavka> Stavke { get; set; }
        public DbSet<Komentar> Komentari { get; set; }
        public DbSet<NacinPlacanja> NaciniPlacanja { get; set; }
        public DbSet<Namena> Namene { get; set; }
        public DbSet<Mesto> Mesta { get; set; }
        public DbSet<PPKategorija> PPKategorije { get; set; }
        public DbSet<Izvod> Izvodi { get; set; }
        public DbSet<IstorijaUplata> IstorijaUplata { get; set; }
        public DbSet<Promena> Promene { get; set; }
        public DbSet<Parametar> Parametri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NacinPlacanja>().HasKey(x => x.NUID);

            modelBuilder.Entity<Namena>().HasKey(x => x.Id);

            modelBuilder
                .Entity<Dokument>()
                .HasKey(
                    nameof(Contracts.Entities.Dokument.VrDok),
                    nameof(Contracts.Entities.Dokument.BrDok)
                );

            modelBuilder
                .Entity<Dokument>()
                .HasOne(x => x.VrstaDok)
                .WithMany(x => x.Dokumenti)
                .HasForeignKey(x => x.VrDok);

            modelBuilder
                .Entity<VrstaDokMag>()
                .HasKey(
                    nameof(Contracts.Entities.VrstaDokMag.VrDok),
                    nameof(Contracts.Entities.VrstaDokMag.MagacinId)
                );

            modelBuilder
                .Entity<Roba>()
                .HasOne(x => x.Tarifa)
                .WithMany(x => x.Roba)
                .HasForeignKey(x => x.TarifaId);

            modelBuilder
                .Entity<Stavka>()
                .HasOne(x => x.Dokument)
                .WithMany(x => x.Stavke)
                .HasForeignKey(x => new { x.VrDok, x.BrDok });

            modelBuilder
                .Entity<Stavka>()
                .HasOne(x => x.Magacin)
                .WithMany(x => x.Stavke)
                .HasForeignKey(x => x.MagacinId);

            modelBuilder
                .Entity<Stavka>()
                .HasOne(x => x.Tarifa)
                .WithMany()
                .HasForeignKey(x => x.TarifaId);

            modelBuilder
                .Entity<RobaUMagacinu>()
                .HasKey(
                    nameof(Contracts.Entities.RobaUMagacinu.MagacinId),
                    nameof(Contracts.Entities.RobaUMagacinu.RobaId)
                );

            modelBuilder.Entity<Komentar>().HasKey(nameof(Komentar.VrDok), nameof(Komentar.BrDok));
        }
    }
}
