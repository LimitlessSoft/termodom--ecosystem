using Microsoft.EntityFrameworkCore;
using TD.TDOffice.Contracts.Entities;

namespace TD.TDOffice.Repository
{
    public class TDOfficeDbContext : DbContext
    {
        public DbSet<DokumentTagIzvod> DokumentTagIzvodi { get; set; }
        public DbSet<MCPartnerCenovnikKatBrRobaIdEntity> MCPartnerCenovnikKatBrRobaIds { get; set; }
        public DbSet<MCPartnerCenovnikItemEntity> MPPartnerCenovnikItems { get; set; }

        public TDOfficeDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
